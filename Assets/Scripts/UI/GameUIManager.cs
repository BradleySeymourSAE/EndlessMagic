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

	/// <summary>
	///		Reference to the Pause Menu UI 
	/// </summary>
	public PauseMenu PauseMenuUI;

	#endregion

	#region Private Variables 

	/// <summary>
	///	 Reference to the player join start timer value 
	/// </summary>
	private float m_PlayerJoinTimer;

	/// <summary>
	///		Should begin the player join start timer begin? 
	/// </summary>
	private bool m_StartCountdown = false;

	/// <summary>
	///		Reference to the coroutine 
	/// </summary>
	private Coroutine m_Routine;

	[SerializeField] private bool m_DisplayingPlayerJoinMenu = false;

	#endregion

	#region Unity References 

	#region @TODO - ADD PAUSE MENU EVENTS 
	/// <summary>
	///		Need to Add Events to handle setting the current player controlling the pause menu UI 
	/// </summary>
	#endregion
	
	/// <summary>
	///		Subscribes to events 
	/// </summary>
	private void OnEnable()
	{
		GameEvents.PlayGUISelectedEvent += PlayUISelected;
		GameEvents.PlayMenuTransitionEvent += PlayMenuSwitched;

		GameEvents.SetPlayerJoinedEvent += SetConnectedControllers;
		GameEvents.SetPlayerReadyEvent += SetPlayerReady;

		GameEvents.UpdatePlayerJoinReadyTimer += BeginPlayerJoinCountdownTimer;
	}

	/// <summary>
	///		Unsubscribe from events 
	/// </summary>
	private void OnDisable()
	{
		GameEvents.PlayGUISelectedEvent -= PlayUISelected;
		GameEvents.PlayMenuTransitionEvent -= PlayMenuSwitched;

		GameEvents.SetPlayerJoinedEvent -= SetConnectedControllers;
		GameEvents.SetPlayerReadyEvent -= SetPlayerReady;

		GameEvents.UpdatePlayerJoinReadyTimer -= BeginPlayerJoinCountdownTimer;
	}

	/// <summary>
	///		Sets up the child class references 
	/// </summary>
	private void Start()
	{
		// Sets up the UI References 
		MainMenuUI.Setup(this);
		CreditsMenuUI.Setup(this);
		SettingsMenuUI.Setup(this);
		PlayerJoinMenuUI.Setup(this);

		PauseMenuUI.Setup(this);


		AudioManager.PlaySound(SoundEffect.UI_StartMenuBackgroundMusic);

		GameEvents.PlayMenuTransitionEvent?.Invoke();


		DisplayMainMenu(true); // Displays the Main Menu UI 
		DisplayCreditsMenu(false); // Hides the Settings Menu UI  
		DisplaySettingsMenu(false); // Hides the Settings Menu UI 
		DisplayPlayerJoinMenu(false); // Hides the Player Count UI 

		DisplayPauseMenu(false); // Hides the Pause Menu UI 
		DisplayPauseMenuConfirmModal(false); // Hides the Pause Menu Confirm Modal 
	
		if (GameObject.Find("StartButton"))
		{
			EventSystem.current.SetSelectedGameObject(GameObject.Find("StartButton"));
		}
	}

	/// <summary>
	///		Instantiates the GameUIManager Instance 
	/// </summary>
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

	/// <summary>
	///		Update - Handles Updating the UI Countdown Timers 
	/// </summary>
	private void Update()
	{

		m_DisplayingPlayerJoinMenu = IsDisplayingPlayerJoinMenu();

		if (m_StartCountdown)
		{
			// Sets the float to deduct from the initial start timer 
			m_PlayerJoinTimer -= 1 * Time.deltaTime;

			// Updates the player join timer UI 
			PlayerJoinMenuUI.SetJoinTimer(m_PlayerJoinTimer);
		}
		else
		{
			// If we aren't displaying the player join menu 
			if (!IsDisplayingPlayerJoinMenu())
			{
				// If the coroutine is still playing
				if (m_Routine != null)
				{
					// Stop the coroutine 
					StopCoroutine(m_Routine);
					PlayerJoinMenuUI.timer.text = "";
				}
			}
		}	
	}

	#endregion

	#region Public Methods

	// --- START MENU METHODS ---
	
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
	public void DisplayPlayerJoinMenu(bool show) => PlayerJoinMenuUI.DisplayScreen(show);

	/// <summary>
	///		Checks if the player join menu is currently displaying 
	/// </summary>
	/// <returns></returns>
	public bool IsDisplayingPlayerJoinMenu() => PlayerJoinMenuUI.PlayerJoinMenuScreen.active == true;

	// --- IN GAME METHODS --- 

	/// <summary>
	///		Displays the Pause menu ui 
	/// </summary>
	/// <param name="show"></param>
	public void DisplayPauseMenu(bool show) => PauseMenuUI.DisplayPauseMenu(show);

	/// <summary>
	///		Toggles Displaying the Pause Menu Confirm Leave Modal 
	/// </summary>
	/// <param name="show"></param>
	public void DisplayPauseMenuConfirmModal(bool show) => PauseMenuUI.DisplayConfirmModal(show);
	
	#endregion

	#region Private Methods

	/// <summary>
	///		 Once Called - Plays the UI sound for selecting
	/// </summary>
	private void PlayUISelected() => AudioManager.PlaySound(SoundEffect.GUI_Selected);

	/// <summary>
	/// 	Once Called - Plays the UI sound for menu transitions
	/// </summary>
	private void PlayMenuSwitched() => AudioManager.PlaySound(SoundEffect.GUI_MenuSwitched);

	/// <summary>
	///		Sets the currently connected controllers 
	/// </summary>
	/// <param name="ConnectedControllers"></param>
	private void SetConnectedControllers(int p_ConnectedControllers) => PlayerJoinMenuUI.UpdateConnectedDevices();

	/// <summary>
	///		Sets the player's cursor game object & ready events 
	/// </summary>
	/// <param name="p_CursorGameObject"></param>
	/// <param name="p_IsPlayerReady"></param>
	private void SetPlayerReady(GameObject p_CursorGameObject, int p_IsPlayerReady) => PlayerJoinMenuUI.SetPlayerReady(p_CursorGameObject, p_IsPlayerReady);

	/// <summary>
	///		Handles Setting Reference to the Current Player (controller) Controlling the Pause Menu UI 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex"></param>
	private void SetPauseMenuCurrentPlayer(int p_CurrentPlayerIndex) => PauseMenuUI.SetCurrentPlayer(p_CurrentPlayerIndex);

	/// <summary>
	///		 Begins the player join countdown timer event 
	/// </summary>
	/// <param name="ShouldBeginCountdown"></param>
	private void BeginPlayerJoinCountdownTimer(bool ShouldBeginCountdown = false)
	{
		if (ShouldBeginCountdown)
		{
			if (m_Routine != null)
			{
				StopCoroutine(m_Routine);
			}

			m_Routine = StartCoroutine(DisplayCountdownTimer());
		}
		else
		{
				StopCoroutine(m_Routine);
		}
	}

	/// <summary>
	///		Begins displaying the player join countdown timer 
	/// </summary>
	/// <returns></returns>
	IEnumerator DisplayCountdownTimer()
	{
		// Set the join timer to 10 seconds 
		m_PlayerJoinTimer = GameManager.Instance.JoinStartTimer;

		// Begin the countdown 
		m_StartCountdown = true;

		// Wait x amount of seconds 
		yield return new WaitForSeconds(GameManager.Instance.JoinStartTimer);

		
		if (!IsDisplayingPlayerJoinMenu())
		{
			Debug.LogWarning("Not moving to next scene - The player join menu is not being displayed");
			yield return null;
		}
		else
		{
			Debug.Log("[GameUIManager.DisplayCountdownTimer]: " + "Player join menu is displaying - Moving to the Handle Cooperative Character Creation Scene!");
			PlayerJoinMenuUI.HandleCooperativeCharacterCreation();
		}
	


		// Stop displaying the player join menu
		DisplayPlayerJoinMenu(false);
	
		// Stop counting down 
		m_StartCountdown = false;

		// Reset the timer 
		m_PlayerJoinTimer = GameManager.Instance.JoinStartTimer;


		yield return null;
	}

	#endregion

}