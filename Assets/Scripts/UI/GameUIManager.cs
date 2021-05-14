#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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
	public PlayerCountMenu PlayerCountMenuUI;

	#endregion

	#region Unity References 

	private void OnEnable()
	{
		GameEvents.PlayGUISelectedEvent += PlayUISelected;
		GameEvents.PlayMenuTransitionEvent += PlayMenuSwitched;
		GameEvents.HandleUpdateConnectedDevicesUI += UpdateConnectedDevicesUI;
	}

	private void OnDisable()
	{
		GameEvents.PlayGUISelectedEvent -= PlayUISelected;
		GameEvents.PlayMenuTransitionEvent -= PlayMenuSwitched;
		GameEvents.HandleUpdateConnectedDevicesUI -= UpdateConnectedDevicesUI;
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
		PlayerCountMenuUI.Setup(this);


		AudioManager.PlaySound(SoundCategory.UI_StartMenuBackgroundMusic);

		GameEvents.PlayMenuTransitionEvent?.Invoke();


		DisplayMainMenu(true); // Displays the Main Menu UI 
		DisplayCreditsMenu(false); // Hides the Settings Menu UI  
		DisplaySettingsMenu(false); // Hides the Settings Menu UI 
		DisplayPlayerCountMenu(false); // Hides the Player Count UI 
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
	public void DisplayPlayerCountMenu(bool show) => PlayerCountMenuUI.DisplayScreen(show);


	#region @TODO - Scene Async Loading 


	/*  Currently this is not working, I didn't have the time to finish this up today so I am just leaving this out for now 
	 *  and I will work on it a bit more when I get back from picking up casey haha :P 
	 *  
	 *  
	 *  
	 *  
	 *  
	/// <summary>
	///		Begins loading the scene 
	/// </summary>
	/// <param name="Scene"></param>
	/// <returns></returns>
	public IEnumerator LoadSceneAsync(Scenes p_SceneType)
	{
		DisplayLoadingScreen(true);


		yield return StartCoroutine(FadeLoadingScreen(1, 1));



		AsyncOperation operation = SceneManager.LoadSceneAsync(GameScenes.SelectGameSceneBySceneType(p_SceneType));

		while (!operation.isDone)
		{
			yield return null;
		}

		yield return StartCoroutine(FadeLoadingScreen(0, 1));

		DisplayLoadingScreen(false);

		yield return null;
	}

	/// <summary>
	///		Fades the loading screen 
	/// </summary>
	/// <param name="p_FadeAlpha"></param>
	/// <param name="p_Duration"></param>
	/// <returns></returns>
	private IEnumerator FadeLoadingScreen(float p_FadeAlpha, float p_Duration)
	{
		float start = LoadingScreenUI.canvasGroup.alpha;
		float time = 0;

		while (time < p_Duration)
		{
			LoadingScreenUI.canvasGroup.alpha = Mathf.Lerp(start, p_FadeAlpha, time / p_Duration);

			time += Time.deltaTime;

			yield return null;
		}

		LoadingScreenUI.canvasGroup.alpha = p_FadeAlpha;
	}

	*/

	#endregion

	#endregion

	#region Private Methods


	/// <summary>
	///		 Play the UI sound when you click on any of the UI elements 
	/// </summary>
	private void PlayUISelected() => AudioManager.PlaySound(SoundCategory.GUI_Selected);


	/// <summary>
	/// 	Play the UI sound when you transition between menu's 
	/// </summary>
	private void PlayMenuSwitched() => AudioManager.PlaySound(SoundCategory.GUI_MenuSwitched);

	private void UpdateConnectedDevicesUI() => PlayerCountMenuUI.SetConnectedDevices();


	#endregion

}
