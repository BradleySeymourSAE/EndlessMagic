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
///		Data class for the Main Menu UI 
/// </summary>
public class MainMenu
{

	#region Public Variables

	/// <summary>
	///		The Main Menu UI Screen Reference 
	/// </summary>
	public GameObject MainMenuScreen;

	/// <summary>
	///		Main Menu Title
	/// </summary>
	public TMP_Text title;

	/// <summary>
	///		Main Menu Subtitle 
	/// </summary>
	public TMP_Text subtitle;

	/// <summary>
	///		Game Version Text Field 
	/// </summary>
	public TMP_Text version;

	/// <summary>
	///		Coop player button reference 
	/// </summary>
	public Button StartButton;
	
	/// <summary>
	///		Credits button reference 
	/// </summary>
	public Button CreditsButton;
	
	/// <summary>
	///		Settings button reference 
	/// </summary>
	public Button SettingsButton;
	
	/// <summary>
	///		Quit button reference 
	/// </summary>
	public Button QuitButton;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to the Game UI Manager 
	/// </summary>
	private GameUIManager m_GameUIManager;

	private bool m_StartButtonIsSelected = false;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the Main Menu UI References  
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{
		m_GameUIManager = p_GameUIManager;


		title.text = GameText.MainMenuUI_Title;
		subtitle.text = GameText.MainMenuUI_Subtitle;
		version.text = GameText.MainMenuUI_Version;
		

		StartButton.GetComponentInChildren<Text>().text = GameText.MainMenuUI_StartButton;
		StartButton.onClick.RemoveAllListeners();
		StartButton.onClick.AddListener(OpenPlayerJoinMenu);

		CreditsButton.GetComponentInChildren<Text>().text = GameText.MainMenuUI_CreditsButton;
		CreditsButton.onClick.RemoveAllListeners();
		CreditsButton.onClick.AddListener(OpenCreditsMenu);

		SettingsButton.GetComponentInChildren<Text>().text = GameText.MainMenuUI_SettingsButton;
		SettingsButton.onClick.RemoveAllListeners();
		SettingsButton.onClick.AddListener(OpenSettingsMenu);

		QuitButton.GetComponentInChildren<Text>().text = GameText.MainMenuUI_QuitButton;
		QuitButton.onClick.RemoveAllListeners();
		QuitButton.onClick.AddListener(QuitApplication);
	}


	/// <summary>
	///		Toggles displaying the Main Menu UI Screen 
	/// </summary>
	/// <param name="ShouldDisplayScreen"></param>
	public void DisplayScreen(bool ShouldDisplayScreen)
	{
		MainMenuScreen.SetActive(ShouldDisplayScreen);

		if (ShouldDisplayScreen)
		{ 
			GameEvents.PlayMenuTransitionEvent?.Invoke();
			m_StartButtonIsSelected = true;
		}
		else
		{
			m_StartButtonIsSelected = false;
		}


		if (m_StartButtonIsSelected)
		{
			StartButton.Select();
		}
	}
	#endregion

	#region Private Methods 


	/// <summary>
	///		Starts the game in cooperative mode 
	/// </summary>
	private void OpenPlayerJoinMenu()
	{
		// Hide Main Menu UI 
		DisplayScreen(false);

		GameEvents.PlayGUISelectedEvent?.Invoke();

		//	Displays the player count menu ui 
		m_GameUIManager.DisplayPlayerJoinMenu(true);
	}


	/// <summary>
	///		Opens the credits menu ui 
	/// </summary>
	private void OpenCreditsMenu()
	{
		// Hide the Main Menu UI 
		DisplayScreen(false);

		GameEvents.PlayGUISelectedEvent?.Invoke();

		// Call the Game UI Manager to display the credits menu 
		m_GameUIManager.DisplayCreditsMenu(true);
	}

	/// <summary>
	///		Opens the settings menu ui 
	/// </summary>
	private void OpenSettingsMenu()
	{
		// Hide the Main Menu UI 
		DisplayScreen(false);

		GameEvents.PlayGUISelectedEvent?.Invoke();

		// Call the Game UI Manager to display settings menu 

		m_GameUIManager.DisplaySettingsMenu(true);
	}

	/// <summary>
	///		Quits the application 
	/// </summary>
	private void QuitApplication()
	{

		GameEvents.PlayGUISelectedEvent?.Invoke();

		#if UNITY_STANDALONE
				Debug.Log("[MainMenu.QuitApplication]: " + "Quitting Application!");
				Application.Quit();
		#endif

		// Running in the editor 

		#if UNITY_EDITOR
				// Stop playing the scene 
				Debug.Log("[MainMenu.QuitApplication]: " + "Running in the Editor - Editor application has stopped playing!");
				UnityEditor.EditorApplication.isPlaying = false;
		#endif

	}

	#endregion

}
