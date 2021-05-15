#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#endregion


[System.Serializable]
/// <summary>
///		Data class for the Settings Menu UI 
/// </summary>
public class PlayerCountMenu
{

	#region Public Variables

	/// <summary>
	///		The Settings Menu UI Screen Reference 
	/// </summary>
	public GameObject PlayerCountMenuScreen;

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
	///		The total amount of currently connected devices 
	/// </summary>
	private int m_ConnectedDevices;

	private int m_ConnectedPlayers;

	/// <summary>
	///		The player input container background color 
	/// </summary>
	[SerializeField] private Color m_PlayerInputContainerBackgroundColor = Color.black;

	[SerializeField] private Sprite m_PlayerInputBackgroundImage;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the credits menu UI 
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{
		m_GameUIManager = p_GameUIManager;

		title.text = GameText.PlayerCountUI_Title;
		subtitle.text = GameText.PlayerCountUI_Subtitle;
		
		ContinueButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_ContinueButton;
		ContinueButton.onClick.RemoveAllListeners();
		ContinueButton.onClick.AddListener(HandleCooperativeCharacterCreation);
		ContinueButton.interactable = false;

		CloseButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_BackButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);


		UpdateConnectedDevices();
	}

	/// <summary>
	///		Toggles displaying the Settings menu ui 
	/// </summary>
	/// <param name="ShouldDisplayPlayerCountMenu"></param>
	public void DisplayScreen(bool ShouldDisplayPlayerCountMenu) 
	{
		GameEvents.PlayMenuTransitionEvent?.Invoke();

		PlayerCountMenuScreen.SetActive(ShouldDisplayPlayerCountMenu);
	}

	/// <summary>
	///		Returns whether the player count menu screen is currently visible  
	/// </summary>
	/// <returns></returns>
	public bool IsVisible() => PlayerCountMenuScreen.active == true;

	/// <summary>
	///		Updates the UI to display the currently connected devices and set the continue button to interactable if the 
	///		amount of players is greater than 1 but less than or equal to 4 
	/// </summary>
	public void UpdateConnectedDevices()
	{
		// Check the game manager instance exists 
		if (GameManager.Instance)
		{ 
			m_ConnectedDevices = GameManager.Instance.GetConnectedDeviceIndex;
			m_ConnectedPlayers = GameManager.Instance.GetConnectedPlayersIndex;
		}
		else
		{
			Debug.LogWarning("[PlayerCountMenu.UpdateConnectedDevices]: " + "There is no GameManager Instance!");
			m_ConnectedDevices = 0;
			m_ConnectedPlayers = 0;
		}

		Debug.Log("[PlayerCountMenu.UpdateConnectedDevices]: " + "Connected Devices: " + m_ConnectedDevices);
		Debug.Log("[PlayerCountMenu.UpdateConnectedDevices]: " + "Connected Players: " + m_ConnectedPlayers);


		// If the amount of connected players is greater than 1 and less than or equal to 4 then the continue button can be pressed 
		ContinueButton.interactable = m_ConnectedPlayers > 0 && m_ConnectedPlayers <= 4;

		
		int currentIndex = 0; // The current Index => 1, 2, 3, 4 

		for (int i = 0; i < playerJoinStatusTextFields.Count; i++)  // i = 0, 1, 2, 3
		{
			
			currentIndex++; // Increase the current index 

			TMP_Text textField = playerJoinStatusTextFields[i]; 

			if (m_ConnectedPlayers >= currentIndex) // if the connected players index is greater than or equal to the current index value
			{
				textField.text = GameText.PlayerCountUI_PlayerStatus_Locked; // then the player has joined 
			}
			// Otherwise, if the connected players index is less than the current index and the total connected devices is greater than the current index 
			else if (m_ConnectedPlayers < currentIndex && m_ConnectedDevices > currentIndex)
			{
				textField.text = GameText.PlayerCountUI_PlayerStatus_PressStart; // then the player can join by pressing start on their controller 
			}
			else
			{
				// Otherwise, there is no controller connected 
				textField.text = GameText.PlayerCountUI_PlayerStatus_ConnectController;
			}
		}
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

		// Hide the player count menu UI
		DisplayScreen(false);


		// Load the character creation scene 
		Debug.Log("[PlayerCountMenu.HandleCooperativeCharacterCreation]: " + "Loading Co-op Character Creation Scene... " + GameScenes.EndlessMagic_CharacterCreation);
		
		// Load the Character Creation Scene Asyncronously
		SceneManager.LoadSceneAsync(GameScenes.SelectGameSceneBySceneType(Scenes.EndlessMagic_CharacterCreation));

	}

	#endregion

}
