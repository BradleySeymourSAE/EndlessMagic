#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion



/// <summary>
///		Data class for the Pause Menu UI 
/// </summary>
[System.Serializable]
public class PauseMenu
{

	#region Public Methods 

	/// <summary>
	///		The Pause Menu Screen Game Object 
	/// </summary>
	public GameObject PauseMenuScreen;

	/// <summary>
	///		The Quit Confirmation Modal (That is displayed when the user presses the leave / quit button)
	/// </summary>
	public GameObject ConfirmQuitModal;

	#region Pause Menu UI 
	[Header("--- Pause Menu UI ---")]
	/// <summary>
	///		Reference to the Pause Menu Title
	/// </summary>
	public TMP_Text pauseTitle;

	/// <summary>
	///		Reference to the Pause Menu Subtitle Field
	/// </summary>
	public GameObject pauseSubtitle;

	/// <summary>
	///		Resumes playing the game 
	/// </summary>
	public Button ResumeButton;

	/// <summary>
	///		Call's an event to wait for other players to accept the game restart 
	/// </summary>
	public Button RestartButton;

	/// <summary>
	///		Opens the options menu for the player 
	/// </summary>
	public Button OptionsButton;
	
	/// <summary>
	///		Displays the Confirm Quit Modal (For leaving the game) 
	/// </summary>
	public Button LeaveButton;

	#endregion

	#region Confirm Modal 

	[Header("--- Confirm Modal UI ---")]

	/// <summary>
	///		Confirm quit modal title 
	/// </summary>
	public TMP_Text confirmTitle;

	/// <summary>
	///		Confirm quit modal subtitle 
	/// </summary>
	public TMP_Text confirmSubtitle;

	/// <summary>
	///		Confirms quit - Leave's the game and returns to the home screen 
	/// </summary>
	public Button ConfirmQuit;

	/// <summary>
	///		Cancel's leaving the game - closes the modal. 
	/// </summary>
	public Button ConfirmCancel;

	#endregion

	#endregion

	#region Private Methods

	/// <summary>
	///		Reference to the Pause Menu Screen Tint
	/// </summary>
	private Image m_PauseMenuScreenTint;

	/// <summary>
	///		Reference to the GameUIManager Instance
	/// </summary>
	private GameUIManager m_GameUIManager;

	/// <summary>
	///		The starting text for the pause menu subtitle 
	/// </summary>
	private TMP_Text m_PauseSubtitleTextStart;

	/// <summary>
	///		The pause menu subtitle image icon 
	/// </summary>
	private Image m_PauseSubtitleImageIcon;

	/// <summary>
	///		The pause menu subtitle text end 
	/// </summary>
	private TMP_Text m_PauseSubtitleTextEnd;

	/// <summary>
	///		Reference to the current player that is controlling the UI 
	/// </summary>
	private int m_CurrentPlayerIndex;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up starting values and reference to the GameUIManager Instance
	/// </summary>
	/// <param name="p_GameUIManager"></param>
	public void Setup(GameUIManager p_GameUIManager)
	{
		m_GameUIManager = p_GameUIManager;

		// Pause Menu UI 

		m_PauseMenuScreenTint = PauseMenuScreen.transform.GetChild(0).GetComponent<Image>();
		pauseTitle.text = GameText.PauseMenu_Title;
		
		m_PauseSubtitleTextStart = pauseSubtitle.transform.GetChild(0).GetComponentInChildren<TMP_Text>();	
		m_PauseSubtitleTextStart.text = GameText.PauseMenu_Subtitle_Text1;

		m_PauseSubtitleImageIcon = pauseSubtitle.transform.GetChild(1).GetComponent<Image>();
		m_PauseSubtitleImageIcon.enabled = false;

		m_PauseSubtitleTextEnd = pauseSubtitle.transform.GetChild(2).GetComponentInChildren<TMP_Text>();
		m_PauseSubtitleTextEnd.text = GameText.PauseMenu_Subtitle_Text2;

		ResumeButton.GetComponentInChildren<Text>().text = GameText.PauseMenu_Resume;
		ResumeButton.onClick.RemoveAllListeners();
		ResumeButton.onClick.AddListener(HandleResumeGame);

		RestartButton.GetComponentInChildren<Text>().text = GameText.PauseMenu_Restart;
		RestartButton.onClick.RemoveAllListeners();
		RestartButton.onClick.AddListener(HandleOnGameRestart);

		OptionsButton.GetComponentInChildren<Text>().text = GameText.PauseMenu_Options;
		OptionsButton.onClick.RemoveAllListeners();
		OptionsButton.onClick.AddListener(DisplayPlayerOptionsMenu);

		LeaveButton.GetComponentInChildren<Text>().text = GameText.PauseMenu_Leave;
		LeaveButton.onClick.RemoveAllListeners();
		LeaveButton.onClick.AddListener(HandleLeave);


		// Confirm Modal UI

		confirmTitle.text = GameText.PauseMenu_ConfirmModal_Title;
		confirmSubtitle.text = GameText.PauseMenu_ConfirmModal_Subtitle;
		
		ConfirmQuit.GetComponentInChildren<Text>().text = GameText.PauseMenu_ConfirmModal_ConfirmQuit;
		ConfirmQuit.onClick.RemoveAllListeners();
		ConfirmQuit.onClick.AddListener(HandleConfirmQuitGame);

		ConfirmCancel.GetComponentInChildren<Text>().text = GameText.PauseMenu_ConfirmModal_ConfirmCancel;
		ConfirmCancel.onClick.RemoveAllListeners();
		ConfirmCancel.onClick.AddListener(HandleConfirmCancelLeave);

		m_CurrentPlayerIndex = 0;

		if (ConfirmQuitModal.activeInHierarchy)
		{
			DisplayConfirmModal(false);
		}
	}

	/// <summary>
	///		Toggles displaying the Pause Menu UI Screen
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void DisplayPauseMenu(bool ShouldDisplay) => PauseMenuScreen.SetActive(ShouldDisplay);

	/// <summary>
	///		Toggles Displaying the Confirmation Modal 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void DisplayConfirmModal(bool ShouldDisplay) => ConfirmQuitModal.SetActive(ShouldDisplay);

	/// <summary>
	///		Sets a reference to the current player controlling the Pause Menu UI 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex"></param>
	public void SetCurrentPlayer(int p_CurrentPlayerIndex) => m_CurrentPlayerIndex = p_CurrentPlayerIndex;

	#endregion

	#region Private Methods 

	/// <summary>
	///		Handles Resuming the Game 
	/// </summary>
	private void HandleResumeGame()
	{
		Debug.Log("[PauseMenu.HandleResumeGame]: " + "Resuming the game!");

		// Play confirm sound effect 
		AudioManager.PlaySound(SoundEffect.GUI_Confirm);

		// Hides the pause menu 
		DisplayPauseMenu(false);

	}

	/// <summary>
	///		Invokes an event to wait for input from other players to restart the game 
	/// </summary>
	private void HandleOnGameRestart()
	{
		Debug.Log("[PauseMenu.HandleOnGameRestart]: " + "Game Restart button pressed! - Waiting for other players response...");

		// GameEvents.HandleGameRestartWaitForPlayerInputEvent?.Invoke();

	}

	/// <summary>
	///		@TODO - To do this, we will need to store a reference to the current player's ID in this menu
	///			  - Maybe this could be done through the use of an event? 
	///		Displays the Player Options UI Menu
	/// </summary>
	private void DisplayPlayerOptionsMenu()
	{
		Debug.Log("[PauseMenu.DisplayPlayerOptionsMenu]: " + "Displaying the player options menu ui!");
	}

	/// <summary>
	///		Displays Confirm Leave Modal 
	/// </summary>
	private void HandleLeave()
	{

		Debug.Log("[PauseMenu.HandleLeave]: " + "Leave button pressed - Displaying Confirm Quit Modal!");
		
		AudioManager.PlaySound(SoundEffect.GUI_Confirm);

		AudioManager.PlaySound(SoundEffect.GUI_MenuSwitched);


		DisplayConfirmModal(true);

	}

	/// <summary>
	///		Handles Quitting the Game - Returns to the Main Menu 
	/// </summary>
	private void HandleConfirmQuitGame()
	{
		DisplayPauseMenu(false);

		Debug.Log("[PauseMenu.HandleConfirmQuitGame]: " + "Quitting the game - Loading scene " + Scenes.EndlessMagic_StartingMenu.ToString());

		m_GameUIManager.DisplayMainMenu(true);

		GameScenes.LoadScene(Scenes.EndlessMagic_StartingMenu, true);
	}

	/// <summary>
	///		ConfirmModal Cancel Button Event - Displays the pause menu UI and hides the confirm modal 
	/// </summary>
	private void HandleConfirmCancelLeave()
	{

		AudioManager.PlaySound(SoundEffect.GUI_Confirm);

		AudioManager.PlaySound(SoundEffect.GUI_MenuSwitched);

		DisplayConfirmModal(false);

	}

	#endregion

}
