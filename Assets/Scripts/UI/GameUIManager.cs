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
		GameEvents.HandleUpdateDevicesEvent += UpdatedDevices;
		GameEvents.HandleAllowMultipleDeviceInputEvent += SetAllowMultipleDeviceUIInput;
	}

	private void OnDisable()
	{
		GameEvents.PlayGUISelectedEvent -= PlayUISelected;
		GameEvents.PlayMenuTransitionEvent -= PlayMenuSwitched;
		GameEvents.HandleUpdateDevicesEvent -= UpdatedDevices;
		GameEvents.HandleAllowMultipleDeviceInputEvent -= SetAllowMultipleDeviceUIInput;
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

	private void Start()
	{
		// Sets up the UI References 
		MainMenuUI.Setup(this);
		CreditsMenuUI.Setup(this);
		SettingsMenuUI.Setup(this);
		PlayerJoinMenuUI.Setup(this);


		AudioManager.PlaySound(SoundCategory.UI_StartMenuBackgroundMusic);

		GameEvents.PlayMenuTransitionEvent?.Invoke();
		m_AllowMultipleDeviceInput = false;


		DisplayMainMenu(true); // Displays the Main Menu UI 
		DisplayCreditsMenu(false); // Hides the Settings Menu UI  
		DisplaySettingsMenu(false); // Hides the Settings Menu UI 
		DisplayPlayerCountMenu(false); // Hides the Player Count UI 
	
		if (GameObject.Find("StartButton"))
		{
			EventSystem.current.SetSelectedGameObject(GameObject.Find("StartButton"));
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
	///		Toggles allowing multiple device input while on the player join menu screen 
	/// </summary>
	/// <returns></returns>
	public bool AllowPlayerJoinUIInput() => PlayerJoinMenuUI.IsVisible() == true;

	/// <summary>
	///		Toggles allowing multiple controller device inputs for UI 
	/// </summary>
	/// <param name="AllowMultipleControllerInputs"></param>
	/// <returns></returns>
	public bool AllowingControllerUIInput() => m_AllowMultipleDeviceInput == true;

	#endregion

	#region Private Methods

	/// <summary>
	///		Sets whether multiple devices are able to use UI at the same time 
	/// </summary>
	/// <param name="ShouldAllowMultipleDevices"></param>
	private void SetAllowMultipleDeviceUIInput(bool ShouldAllowMultipleDevices) => m_AllowMultipleDeviceInput = ShouldAllowMultipleDevices;

	/// <summary>
	///		 Play the UI sound when you click on any of the UI elements 
	/// </summary>
	private void PlayUISelected() => AudioManager.PlaySound(SoundCategory.GUI_Selected);

	/// <summary>
	/// 	Play the UI sound when you transition between menu's 
	/// </summary>
	private void PlayMenuSwitched() => AudioManager.PlaySound(SoundCategory.GUI_MenuSwitched);

	/// <summary>
	///		Handles updating the currently connected devices for the player count menu UI 
	/// </summary>
	private void UpdatedDevices() => PlayerJoinMenuUI.UpdateConnectedDevices();

	#endregion

}
