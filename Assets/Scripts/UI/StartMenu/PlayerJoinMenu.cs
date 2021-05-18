#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
#endregion


[System.Serializable]
/// <summary>
///		Data class for the Player Join Menu UI 
/// </summary>
public class PlayerJoinMenu
{

	#region Public Variables

	/// <summary>
	///		The Settings Menu UI Screen Reference 
	/// </summary>
	public GameObject PlayerJoinMenuScreen;

	/// <summary>
	///		Player Count Menu Title
	/// </summary>
	public TMP_Text title;

	/// <summary>
	///		Player Count Menu Subtitle
	/// </summary>
	public TMP_Text subtitle;

	/// <summary>
	///		Reference to the player join container
	/// </summary>
	public List<GameObject> playerJoinContainers;

	public List<TMP_Text> playerJoinStatusTextFields;

	/// <summary>
	///		Plays the game in cooperative mode 
	/// </summary>
	public Button ContinueButton;

	/// <summary>
	///		Close Settings Menu Button
	/// </summary>
	public Button CloseButton;

	#endregion

	#region Private Variables

	/// <summary>
	///		Reference to the Game UI Manager Instance 
	/// </summary>
	private GameUIManager m_GameUIManager;

	/// <summary>
	///		The player input container background color 
	/// </summary>
	[SerializeField] private Color m_PlayerInputContainerBackgroundColor = Color.black;

	[SerializeField] private Sprite m_PlayerInputBackgroundImage;

	[SerializeField] private int m_TotalReadyPlayers = 0;

	private List<GameObject> readyPlayers = new List<GameObject>();

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the credits menu UI 
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{

		playerJoinStatusTextFields.Clear();
		readyPlayers.Clear();

		m_GameUIManager = p_GameUIManager;
		m_TotalReadyPlayers = 0;

		title.text = GameText.PlayerJoinUI_Title;
		subtitle.text = GameText.PlayerJoinUI_Subtitle;
		
		ContinueButton.GetComponentInChildren<Text>().text = GameText.PlayerJoinUI_ContinueButton;
		ContinueButton.onClick.RemoveAllListeners();
		ContinueButton.onClick.AddListener(HandleCooperativeCharacterCreation);
		ContinueButton.interactable = false;

		CloseButton.GetComponentInChildren<Text>().text = GameText.PlayerJoinUI_ReturnButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);

		foreach (GameObject container in playerJoinContainers)
		{
			Image image = container.GetComponent<Image>();
			image.color = m_PlayerInputContainerBackgroundColor;

			for (int i = 1; i < container.transform.childCount; i++)
			{
				TMP_Text statusText = container.transform.GetChild(i).GetComponent<TMP_Text>();
				
				statusText.text = GameText.PlayerJoinUI_PlayerStatus_EmptySlot;

				playerJoinStatusTextFields.Add(statusText);
			}
		}

	}

	/// <summary>
	///		Toggles displaying the Settings menu ui 
	/// </summary>
	/// <param name="ShouldDisplayJoinMenu"></param>
	public void DisplayScreen(bool ShouldDisplayJoinMenu) 
	{
		GameEvents.PlayMenuTransitionEvent?.Invoke();

		PlayerJoinMenuScreen.SetActive(ShouldDisplayJoinMenu);
		
		if (ShouldDisplayJoinMenu)
		{
			ContinueButton.Select();
		}
	}

	/// <summary>
	///		Updates the UI to display the currently connected devices and set the continue button to interactable if the 
	///		amount of players is greater than 1 but less than or equal to 4 
	/// </summary>
	public void UpdateConnectedDevices(int p_ConnectedPlayers)
	{

		int s_ConnectedPlayers = GameManager.Instance.ConnectedPlayers;

		Debug.Log("Connected Players: " + s_ConnectedPlayers);
		
		int currentIndex = 0; // The current Index => 1, 2, 3, 4 

	
		ContinueButton.interactable = m_TotalReadyPlayers == s_ConnectedPlayers && s_ConnectedPlayers > 0;

		foreach (TMP_Text textStatus in playerJoinStatusTextFields)
		{
			currentIndex++;

			

			// If the amount of connected players is greater than the current index 
			if (p_ConnectedPlayers >= currentIndex)
			{
				textStatus.text = GameText.PlayerJoinUI_PlayerStatus_SlotTaken_ReadyUp;
			}
			else
			{
				textStatus.text = GameText.PlayerJoinUI_PlayerStatus_EmptySlot;
			}

		}
	}

	public void SetPlayerReady(int PlayerReadyValue)
	{
		m_TotalReadyPlayers += PlayerReadyValue;
	}

	#endregion

	#region Private Methods

	/// <summary>
	///		Returns to the main menu 
	/// </summary>
	private void ReturnToMainMenu()
	{
		// Hides the Credits Menu UI 
		DisplayScreen(false);


		// Displays the Main Menu 
		m_GameUIManager.DisplayMainMenu(true);
	}

	/// <summary>
	///		Begins the game in cooperative mode 
	/// </summary>
	private void HandleCooperativeCharacterCreation()
	{
		
		m_TotalReadyPlayers = 0; // Reset the total ready players value 

		DisplayScreen(false); // Hide the player count menu UI


		// Load the character creation scene 
		Debug.Log("[PlayerJoinMenu.HandleCooperativeCharacterCreation]: " + "Loading Co-op Character Creation Scene... " + GameScenes.EndlessMagic_CharacterCreation);
		
		// Load the Character Creation Scene Asyncronously
		SceneManager.LoadSceneAsync(GameScenes.SelectGameSceneBySceneType(Scenes.EndlessMagic_CharacterCreation));

	}

	#endregion

}
