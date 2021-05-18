#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
#endregion

/// <summary>
///		Game UI Manager - Handles the UI Views across Scenes  
/// </summary>
public class GameUIManager : MonoBehaviour
{

	#region Static 

	/// <summary>
	///		Reference to the Game UI Manager Instance 
	/// </summary>
	public static GameUIManager Instance;

	#endregion

	#region Public Variables 

	/// <summary>
	///		Reference to the Main Menu UI 
	/// </summary>
	public MainMenu MainMenuUI;
	
	/// <summary>
	///		Reference to the Credits Menu UI 
	/// </summary>
	public CreditsMenu CreditsMenuUI;
	
	/// <summary>
	///		Reference to the Settings Menu UI 
	/// </summary>
	public SettingsMenu SettingsMenuUI;

	/// <summary>
	///		Reference to the Player Count Menu UI 
	/// </summary>
	public PlayerJoinMenu PlayerJoinMenuUI;

	#endregion

	#region Private Variables

	private bool m_AllowMultipleDeviceInput;

	#endregion

	#region Unity References 

	private void OnEnable()
	{
		GameEvents.PlayGUISelectedEvent += PlayUISelected;
		GameEvents.PlayMenuTransitionEvent += PlayMenuSwitched;

		GameEvents.SetPlayerJoinedEvent += SetConnectedControllers;
		GameEvents.SetPlayerReadyEvent += SetPlayerReady;
	}

	private void OnDisable()
	{
		GameEvents.PlayGUISelectedEvent -= PlayUISelected;
		GameEvents.PlayMenuTransitionEvent -= PlayMenuSwitched;

		GameEvents.SetPlayerJoinedEvent -= SetConnectedControllers;
		GameEvents.SetPlayerReadyEvent -= SetPlayerReady;
	}

	private void Start()
	{
		// Sets up the UI References 
		MainMenuUI.Setup(this);
		CreditsMenuUI.Setup(this);
		SettingsMenuUI.Setup(this);
		PlayerJoinMenuUI.Setup(this);


		AudioManager.PlaySound(SoundCategory.UI_StartMenuBackgroundMusic);

		GameEvents.PlayMenuTransitionEvent?.Invoke();


		DisplayMainMenu(true); // Displays the Main Menu UI 
		DisplayCreditsMenu(false); // Hides the Settings Menu UI  
		DisplaySettingsMenu(false); // Hides the Settings Menu UI 
		DisplayPlayerCountMenu(false); // Hides the Player Count UI 
	
		if (GameObject.Find("StartButton"))
		{
			EventSystem.current.SetSelectedGameObject(GameObject.Find("StartButton"));
		}
	}


	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	///		Displays the main menu ui 
	/// </summary>
	/// <param name="show"></param>
	public void DisplayMainMenu(bool show) => MainMenuUI.DisplayScreen(show);

	/// <summary>
	///		Displays the Credits Menu UI 
	/// </summary>
	/// <param name="show"></param>
	public void DisplayCreditsMenu(bool show) => CreditsMenuUI.DisplayScreen(show); 

	/// <summary>
	///		Displays the settings menu ui 
	/// </summary>
	/// <param name="show"></param>
	public void DisplaySettingsMenu(bool show) => SettingsMenuUI.DisplayScreen(show);

	/// <summary>
	///		Displays the player count menu 
	/// </summary>
	/// <param name="show"></param>
	public void DisplayPlayerCountMenu(bool show) => PlayerJoinMenuUI.DisplayScreen(show);

	/// <summary>
	///		Checks if the player join menu is currently displaying 
	/// </summary>
	/// <returns></returns>
	public bool IsDisplayingPlayerJoinMenu() => PlayerJoinMenuUI.PlayerJoinMenuScreen.activeInHierarchy == true;
	#endregion

	#region Private Methods

	/// <summary>
	///		 Once Called - Plays the UI sound for selecting
	/// </summary>
	private void PlayUISelected() => AudioManager.PlaySound(SoundCategory.GUI_Selected);

	/// <summary>
	/// 	Once Called - Plays the UI sound for menu transitions
	/// </summary>
	private void PlayMenuSwitched() => AudioManager.PlaySound(SoundCategory.GUI_MenuSwitched);

	/// <summary>
	///		Sets the currently connected controllers 
	/// </summary>
	/// <param name="ConnectedControllers"></param>
	private void SetConnectedControllers(int ConnectedControllers) => PlayerJoinMenuUI.UpdateConnectedDevices(ConnectedControllers);

	private void SetPlayerReady(int PlayerReady) => PlayerJoinMenuUI.SetPlayerReady(PlayerReady);

	#endregion

}
