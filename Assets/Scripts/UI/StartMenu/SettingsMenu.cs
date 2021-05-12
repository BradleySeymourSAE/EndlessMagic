#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion



[System.Serializable]
/// <summary>
///		Data class for the Settings Menu UI 
/// </summary>
public class SettingsMenu
{

	#region Public Variables

	/// <summary>
	///		The Settings Menu UI Screen Reference 
	/// </summary>
	public GameObject SettingsMenuScreen;

	/// <summary>
	///		Settings Menu Title
	/// </summary>
	public TMP_Text title;

	/// <summary>
	///		Video Tab Settings Text
	/// </summary>
	public TMP_Text tab_Video;

	/// <summary>
	///		Display Tab Settings Text
	/// </summary>
	public TMP_Text tab_Display;

	/// <summary>
	///		Audio Tab Settings Text
	/// </summary>
	public TMP_Text tab_Audio;

	/// <summary>
	///		Applies the Saved Changes 
	/// </summary>
	public Button ApplyChangesButton;

	/// <summary>
	///		Close Settings Menu Button
	/// </summary>
	public Button CloseButton;

	#endregion

	#region Private Variables

	private GameUIManager m_GameUIManager;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the credits menu UI 
	/// </summary>
	public void Setup(GameUIManager p_GameUIManager)
	{
		m_GameUIManager = p_GameUIManager;

		title.text = GameText.SettingsMenuUI_Title;
		tab_Video.text = GameText.SettingsMenuUI_Tab_Video;
		tab_Display.text = GameText.SettingsMenuUI_Tab_Display;
		tab_Audio.text = GameText.SettingsMenuUI_Tab_Audio;

		ApplyChangesButton.GetComponentInChildren<Text>().text = GameText.SettingsMenuUI_ApplyChangesButton;
		ApplyChangesButton.onClick.RemoveAllListeners();
		ApplyChangesButton.onClick.AddListener(HandleSavedChanges);


		CloseButton.GetComponentInChildren<Text>().text = GameText.SettingsMenuUI_CloseButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);
	}

	/// <summary>
	///		Toggles displaying the Settings menu ui 
	/// </summary>
	/// <param name="ShouldDisplayCreditsMenu"></param>
	public void DisplayScreen(bool ShouldDisplayCreditsMenu) 
	{
		
		if (ShouldDisplayCreditsMenu)
		{
			GameEvents.PlayMenuTransitionEvent?.Invoke();
		}
		
		SettingsMenuScreen.SetActive(ShouldDisplayCreditsMenu);
	}
	#endregion

	#region Private Methods

	/// <summary>
	///		Returns to the main menu 
	/// </summary>
	private void ReturnToMainMenu()
	{
		// Hides the Settings Menu UI 
		DisplayScreen(false);

		GameEvents.PlayGUISelectedEvent?.Invoke();

		// Displays the Main Menu 
		m_GameUIManager.DisplayMainMenu(true);
	}

	private void HandleSavedChanges()
	{
		

		Debug.Log("[SettingsMenu.HandleSavedChanges]: "+ "Handling Applying of Saved Changes!");
		
		// Returns to the main menu 
		ReturnToMainMenu();

	}

	#endregion

}
