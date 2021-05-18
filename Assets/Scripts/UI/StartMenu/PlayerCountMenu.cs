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

	/// <summary>
	///		Reference to the player join status text fields
	/// </summary>
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

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the credits menu UI 
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{
		m_GameUIManager = p_GameUIManager;

		title.text = GameText.PlayerJoinUI_Title;
		subtitle.text = GameText.PlayerJoinUI_Subtitle;
		
		ContinueButton.GetComponentInChildren<Text>().text = GameText.PlayerJoinUI_ContinueButton;
		ContinueButton.onClick.RemoveAllListeners();
		ContinueButton.onClick.AddListener(HandleCooperativeCharacterCreation);
		ContinueButton.interactable = false;

		CloseButton.GetComponentInChildren<Text>().text = GameText.PlayerJoinUI_ReturnButton;
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
	///		Updates the UI to display the currently connected devices and set the continue button to interactable if the 
	///		amount of players is greater than 1 but less than or equal to 4 
	/// </summary>
	public void UpdateConnectedDevices()
	{
		
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
