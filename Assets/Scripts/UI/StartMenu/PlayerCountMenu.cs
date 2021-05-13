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
	///		Player Count Menu Input Field
	/// </summary>
	public TMP_InputField PlayerCountInputField;

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
	///		The amount of players we want to add into the character creation screen 
	/// </summary>
	private int m_PlayerCount;

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

		m_PlayerCount = 0;
		
		ContinueButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_ContinueButton;
		ContinueButton.onClick.RemoveAllListeners();
		ContinueButton.onClick.AddListener(HandleCooperativeCharacterCreation);
		ContinueButton.interactable = false;

		PlayerCountInputField.text = m_PlayerCount.ToString();
		PlayerCountInputField.onValueChanged.RemoveAllListeners();
		PlayerCountInputField.onValueChanged.AddListener(InputChanged);

		CloseButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_BackButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);
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
		
		
		AsyncOperation s_LoadingOperation = SceneManager.LoadSceneAsync(GameScenes.SelectGameSceneBySceneType(Scenes.EndlessMagic_CharacterCreation));


		// Invoke the Coop Character Creation Start Event using the input amount of players.
		// This will be used as a reference to set up the camera's 

	


		if (m_PlayerCount > 1 && m_PlayerCount <= 4)
		{

			Debug.LogWarning("[PlayerCountMenu.DisplayCharacterCreationScreen]: " + "Tried to call event from here with " + m_PlayerCount + " players - however ran into an issue");


			// Tried to start a loading event here, as I need the amount of players before the scene loads in order to set up the cameras.. -__-

			// However, Now that I think about it I could just transfer the stuff from the Character Creation Scene and build it into the start menu which would probably
			// work better anyways.

			// Will work on this once I get back from picking casey up :) 


		}
	}



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

	#endregion

}
