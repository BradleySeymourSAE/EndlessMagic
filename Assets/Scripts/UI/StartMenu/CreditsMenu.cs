#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion



[System.Serializable]
/// <summary>
///		Data class for the Credits Menu UI 
/// </summary>
public class CreditsMenu
{

	#region Public Variables

	/// <summary>
	///		The Credits Menu UI Screen Reference 
	/// </summary>
	public GameObject CreditsMenuScreen;

	/// <summary>
	///		Credits Menu Title
	/// </summary>
	public TMP_Text title;

	/// <summary>
	///		Development Team Text 
	/// </summary>
	public TMP_Text developmentTeam;

	/// <summary>
	///		Developer text 
	/// </summary>
	public TMP_Text developer_01;

	/// <summary>
	///		Developer text 
	/// </summary>
	public TMP_Text developer_02;

	/// <summary>
	///		Close Credits Menu Button
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

		title.text = GameText.CreditsMenuUI_Title;
		developmentTeam.text = GameText.CreditsMenuUI_DevelopmentTeam;
		developer_01.text = GameText.CreditsMenuUI_Developer_01;
		developer_02.text = GameText.CreditsMenuUI_Developer_02;

		CloseButton.GetComponentInChildren<Text>().text = GameText.CreditsMenuUI_CloseButton;
		CloseButton.onClick.RemoveAllListeners();
		CloseButton.onClick.AddListener(ReturnToMainMenu);
	}

	/// <summary>
	///		Toggles displaying the credits menu ui 
	/// </summary>
	/// <param name="ShouldDisplayCreditsMenu"></param>
	public void DisplayScreen(bool ShouldDisplayCreditsMenu)
	{

		if (ShouldDisplayCreditsMenu)
		{
			GameEvents.PlayMenuTransitionEvent?.Invoke();
		}

		CreditsMenuScreen.SetActive(ShouldDisplayCreditsMenu);
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

		GameEvents.PlayGUISelectedEvent?.Invoke();


		// Displays the Main Menu 
		m_GameUIManager.DisplayMainMenu(true);
	}

	#endregion

}
