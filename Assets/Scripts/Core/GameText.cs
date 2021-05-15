#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion


/// <summary>
///		Static Class of UI text & button strings 
/// </summary>
public static class GameText
{

	#region UI 

	#region Main Menu

	public const string MainMenuUI_Title = "Endless Magic";
	public const string MainMenuUI_Subtitle = "Fight against your friends";
	public const string MainMenuUI_Version = "Version 1.0.0 Development";
	public const string MainMenuUI_SinglePlayerButton = "Single Player";
	public const string MainMenuUI_CoopButton = "Co-op";
	public const string MainMenuUI_CreditsButton = "Credits";
	public const string MainMenuUI_SettingsButton = "Settings";
	public const string MainMenuUI_QuitButton = "Quit";


	#endregion


	#region Credits Menu 

	public const string CreditsMenuUI_Title = "Endless Magic - Credits";
	public const string CreditsMenuUI_DevelopmentTeam = "Development Team";
	public const string CreditsMenuUI_Developer_01 = "Bradley Seymour";
	public const string CreditsMenuUI_Developer_02 = "David Seymour";
	public const string CreditsMenuUI_CloseButton = "Back";

	#endregion


	#region Settings Menu

	public const string SettingsMenuUI_Title = "Settings";
	public const string SettingsMenuUI_Tab_Video = "Video";
	public const string SettingsMenuUI_Tab_Display = "Display";
	public const string SettingsMenuUI_Tab_Audio = "Audio";
	public const string SettingsMenuUI_ApplyChangesButton = "Apply Changes";
	public const string SettingsMenuUI_CloseButton = "Back";

	#endregion


	#region Player Count Menu 

	public const string PlayerCountUI_Title = "Co-op";
	public const string PlayerCountUI_Subtitle = "Press start to ready up";
	public const string PlayerCountUI_ContinueButton = "Continue";
	public const string PlayerCountUI_BackButton = "Return";

	public const string PlayerCountUI_PlayerStatus_Locked = "--- Locked ---";
	public const string PlayerCountUI_PlayerStatus_PressStart = "Press start to join!";
	public const string PlayerCountUI_PlayerStatus_ConnectController = "Connect a controller";


	#endregion


	#region Loading Screen UI 

	public const string LoadingScreenUI_Title = "Loading";
	public const string LoadingScreenUI_Progress = "0";


	#endregion

	#endregion

	#region Input Keys 

	public const string XboxControllerDevice = "";

	public const string PS4ControllerDevice = "Sony Interactive Entertainment";

	public const string MouseDevice = "Mouse";

	public const string KeyboardDevice = "Keyboard";


	#endregion


}
