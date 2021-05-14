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

	/// <summary>
	///		Reference to the Game Manager Instance 
	/// </summary>
	GameManager m_GameManager;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the credits menu UI 
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{
		m_GameUIManager = p_GameUIManager;

		if (Object.FindObjectOfType<GameManager>() != null)
		{
			m_GameManager = Object.FindObjectOfType<GameManager>();
		}	

		title.text = GameText.PlayerCountUI_Title;
		subtitle.text = GameText.PlayerCountUI_Subtitle;

		m_ConnectedDevices = m_GameManager.GetConnectedDevices;
		m_ConnectedPlayers = m_GameManager.ConnectedPlayers.Count;


		Debug.Log("[PlayerCountMenu.Setup]: " + "Connected Devices: " + m_ConnectedDevices);
		Debug.Log("[PlayerCountMenu.Setup]: " + "Connected Players: " + m_ConnectedPlayers);

		
		ContinueButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_ContinueButton;
		ContinueButton.onClick.RemoveAllListeners();
		ContinueButton.onClick.AddListener(HandleCooperativeCharacterCreation);
		ContinueButton.interactable = false;

		CloseButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_BackButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);


		UpdateTextFields();
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

	public void SetConnectedDevices()
	{
		m_ConnectedDevices = GameManager.Instance.GetConnectedDevices;
		m_ConnectedPlayers = GameManager.Instance.ConnectedPlayers.Count;


		Debug.Log("Connected Devices: " + m_ConnectedDevices);
		Debug.Log("Connected Players: " + m_ConnectedPlayers);



		UpdateTextFields();
	}

	private void UpdateTextFields()
	{
		// The current Index => 1, 2, 3, 4 
		int currentIndex = 0;

		// Loop through the text fields => 0, 1, 2, 3
		for (int i = 0; i < playerJoinStatusTextFields.Count; i++)
		{
			// Increase the index 
			currentIndex++;

			TMP_Text textField = playerJoinStatusTextFields[i];

			if (m_ConnectedPlayers >= currentIndex)
			{
				textField.text = GameText.PlayerCountUI_PlayerStatus_Locked;
			}
			else if (m_ConnectedPlayers < currentIndex && m_ConnectedDevices > currentIndex)
			{
				textField.text = "Press start to join";
			}
			else
			{
				textField.text = GameText.PlayerCountUI_PlayerStatus_ConnectController;
			}
		}
	}

	public void SetConnectedPlayers(int value) => m_ConnectedPlayers = value;
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
		
		
		SceneManager.LoadSceneAsync(GameScenes.SelectGameSceneBySceneType(Scenes.EndlessMagic_CharacterCreation));


		// Invoke the Coop Character Creation Start Event using the input amount of players.
		// This will be used as a reference to set up the camera's 

	


		if (m_ConnectedPlayers > 1)
		{

			Debug.LogWarning("[PlayerCountMenu.DisplayCharacterCreationScreen]: " + "Tried to call event from here with " + m_ConnectedDevices + " players - however ran into an issue");

			Debug.Break();
		}
	}



	/* 
	 *	This will likely need to be removed as it is not relevant to what we are creating now 
	 * 
	/// <summary>
	///		Handles the Changed Input Value 
	/// </summary>
	/// <param name="p_InputValue"></param>
	private void InputChanged(string p_InputValue)
	{
		// Local variable for storing the result 
		int result;

		// Check the input is a number value
		bool isNumberValue = int.TryParse(p_InputValue, out result);

		if (isNumberValue)
		{
			
			// Set the continue button to enabled only if the result is greater than 0 or less than or equal to 4 
			ContinueButton.interactable = result > 1 && result <= 4;
			
			
			m_PlayerCount = result;
			Debug.Log("[PlayerCountMenu.InputChanged]: " + " Player Count: " + m_PlayerCount);
		}
		else
		{
			// Is not a number, so we set the continue button as not interactable 
			ContinueButton.interactable = false;
		}
	}

	*/

	#endregion


}
