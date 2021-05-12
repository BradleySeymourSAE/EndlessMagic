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

	private GameUIManager m_GameUIManager;

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
		
		ContinueButton.GetComponentInChildren<Text>().text = GameText.PlayerCountUI_ContinueButton;
		ContinueButton.onClick.RemoveAllListeners();
		ContinueButton.onClick.AddListener(HandleBeginCooperativeMode);

		PlayerCountInputField.text = GameText.PlayerCountUI_InputPlaceholder;
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
	private void HandleBeginCooperativeMode()
	{
		// Hide the player count menu UI
		DisplayScreen(false);
		
		// Should also do a check here for multiple players 

		// Load's the character creation scene 
		SceneManager.LoadScene(GameScenes.EndlessMagic_CharacterCreation);
	}

	private void InputChanged(string p_InputValue)
	{
		// Local variable for storing the result 
		int result;

		// Check the input is a number value
		bool isNumberValue = int.TryParse(p_InputValue, out result);

		if (isNumberValue)
		{
			
			// Set the continue button to enabled only if the result is greater than 0 or less than or equal to 4 
			ContinueButton.interactable = result > 0 && result <= 4;
			
		}
		else
		{
			// Is not a number, so we set the continue button as not interactable 
			ContinueButton.interactable = false;
		}
	}

	#endregion

}
