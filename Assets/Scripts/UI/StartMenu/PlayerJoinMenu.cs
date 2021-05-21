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
	///		Player Join Menu - Title Text 
	/// </summary>
	public TMP_Text title;

	/// <summary>
	///		Player Join Menu - Subtitle Text 
	/// </summary>
	public TMP_Text subtitle;

	/// <summary>
	///		Player Join - Countdown timer text 
	/// </summary>
	public TMP_Text timer;

	/// <summary>
	///		Reference to the player join container
	/// </summary>
	public List<GameObject> playerJoinContainers;

	/// <summary>
	///		Reference to the status text fields text 
	/// </summary>
	public List<TMP_Text> playerJoinStatusTextFields;

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

	/// <summary>
	///		Reference to the player join sprite background image 
	/// </summary>
	[SerializeField] private Sprite m_PlayerInputBackgroundImage;

	/// <summary>
	///		List of players that have ready'd up 
	/// </summary>
	[SerializeField] private List<GameObject> readyPlayerCursors = new List<GameObject>();

	/// <summary>
	///		Reference to start the countdown timer 
	/// </summary>
	[SerializeField] private bool shouldStartCountdown = false;
	
	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the credits menu UI 
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{
		playerJoinStatusTextFields.Clear();
		readyPlayerCursors.Clear();
		m_GameUIManager = p_GameUIManager;

		title.text = GameText.PlayerJoinUI_Title;
		subtitle.text = GameText.PlayerJoinUI_Subtitle;
		timer.text = GameText.PlayerJoinUI_TimerText;

		CloseButton.GetComponentInChildren<Text>().text = GameText.PlayerJoinUI_ReturnButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);

		foreach (GameObject container in playerJoinContainers)
		{
			Image image = container.GetComponent<Image>();
			image.color = m_PlayerInputContainerBackgroundColor;

			for (int i = 1; i < container.transform.childCount; i++)
			{
				TMP_Text statusText = container.transform.GetChild(i).GetComponentInChildren<TMP_Text>();
				
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
			CloseButton.Select();
		}
	}

	/// <summary>
	///		Begins the game in cooperative mode 
	/// </summary>
	public void HandleCooperativeCharacterCreation()
	{
		// Invoke setting the character creation cursors (With the cursors that have been set as ready) 
		GameEvents.SetCharacterCreationCursorEvent?.Invoke(readyPlayerCursors);

		// Hide the Player Join Menu UI 
		DisplayScreen(false); 


		// Load the character creation scene 
		Debug.Log("[PlayerJoinMenu.HandleCooperativeCharacterCreation]: " + "Loading Co-op Character Creation Scene... " + GameScenes.EndlessMagic_CharacterCreation);

		
		// Load the Character Creation Scene Asyncronously
		GameScenes.LoadScene(Scenes.EndlessMagic_CharacterCreation, true);
	}

	/// <summary>
	///		Updates the UI to display the currently connected devices and set the continue button to interactable if the 
	///		amount of players is greater than 1 but less than or equal to 4 
	/// </summary>
	public void UpdateConnectedDevices()
	{
		// The current amount of connected players 
		int s_ConnectedPlayers = GameManager.Instance.ConnectedPlayers;

		// If the readyPlayers count is equal to the amount of connected players then the button is interactable 
		shouldStartCountdown = readyPlayerCursors.Count >= s_ConnectedPlayers && s_ConnectedPlayers > 1 ? true : false;


		if (shouldStartCountdown)
		{
			GameEvents.UpdatePlayerJoinReadyTimer?.Invoke(shouldStartCountdown);
		}
	}

	/// <summary>
	///		Sets the timer UI 
	/// </summary>
	/// <param name="p_Seconds"></param>
	public void SetJoinTimer(float p_Seconds) => timer.GetComponentInChildren<TMP_Text>().text = "Starting in " + p_Seconds.ToString("0");
	
	/// <summary>
	///		Handles Setting the player's ready status / un-ready status 
	/// </summary>
	/// <param name="PlayerGameObject"></param>
	/// <param name="PlayerReadyValue"></param>
	public void SetPlayerReady(GameObject PlayerGameObject, int PlayerReadyValue)
	{
		bool isPlayerReady = PlayerReadyValue > 0 == true;

		int playerIndex = ReturnPlayerIndex(PlayerGameObject.name.ToString());
		string s_StatusString = $"P{playerIndex}_Status";


		// If the player is ready
		if (isPlayerReady)
		{
			// If the player already exists in the list 
			if (readyPlayerCursors.Contains(PlayerGameObject))
			{
				// Return 
				return;
			}
			else
			{
				// Otherwise, try to find the game object by the status string 

				TMP_Text statusText = GameObject.Find(s_StatusString).GetComponentInChildren<TMP_Text>();

				// Check that the status text is not null 
				if (statusText != null)
				{
					// If it isnt, set the status text to Ready!  
					statusText.text = GameText.PlayerJoinUI_PlayerStatus_SlotTaken_Ready;
				}
		

				// Add the player to the ready players list 
				readyPlayerCursors.Add(PlayerGameObject);
			}
		}
		// Otherwise, if the player is not ready or has un-ready'd up 
		else
		{
			// If the list does not contain the playerCurso Game Object then we want to return 
			if (!readyPlayerCursors.Contains(PlayerGameObject))
			{
				return;
			}
			else
			{

				// Otherwise, Try to find Status GameObject TMP_Text using the status string 
				TMP_Text statusText = GameObject.Find(s_StatusString).GetComponentInChildren<TMP_Text>();

				// Check the status text is not null 
				if (statusText != null)
				{
					// If it isnt, set the status text to `Ready up`
					statusText.text = GameText.PlayerJoinUI_PlayerStatus_SlotTaken_ReadyUp;
				}

				
				// Remove the player from the readyPlayers list 
				readyPlayerCursors.Remove(PlayerGameObject);
			}
		}


		// Update the currently connected devices  
		UpdateConnectedDevices();
	}

	#endregion

	#region Private Methods

	/// <summary>
	///		Returns to the main menu 
	/// </summary>
	private void ReturnToMainMenu()
	{
	
		if (GameEntity.FindAllByTag(GameTag.Cursor).Length > 0)
		{ 
			foreach (GameObject cursor in GameEntity.FindAllByTag(GameTag.Cursor))
			{
				Object.Destroy(cursor);
			}

			foreach (GameObject status in GameEntity.FindAllByTag(GameTag.PlayerJoinStatus))
			{
				TMP_Text statusText = status.GetComponentInChildren<TMP_Text>();
				
				statusText.text = GameText.PlayerJoinUI_PlayerStatus_EmptySlot;
			}
		}

		readyPlayerCursors.Clear();

		// Hides the Credits Menu UI 
		DisplayScreen(false);

		// Displays the Main Menu 
		m_GameUIManager.DisplayMainMenu(true);
	}

	private int ReturnPlayerIndex(string name)
	{
		const string P1_Cursor = "P1_Cursor(Clone)";
		const string P2_Cursor = "P2_Cursor(Clone)";
		const string P3_Cursor = "P3_Cursor(Clone)";
		const string P4_Cursor = "P4_Cursor(Clone)";

		switch (name)
		{
			case P1_Cursor:
				return 1;
			case P2_Cursor:
				return 2;
			case P3_Cursor:
				return 3;
			case P4_Cursor:
				return 4;
			default:
				return 0;
		}
	}

	#endregion

}
