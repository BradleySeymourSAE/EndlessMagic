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

	#region Starting Menu Scene 

	#region Main Menu

	public const string MainMenuUI_Title = "Endless Magic";
	public const string MainMenuUI_Subtitle = "Battle your friends n shiet";
	public const string MainMenuUI_Version = "Version 1.7.2 Development";
	public const string MainMenuUI_StartButton = "Start Game";
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

	#region Player Join Menu 

	public const string PlayerJoinUI_Title = "Co-op";
	public const string PlayerJoinUI_Subtitle = "Press the start button to join";
	public const string PlayerJoinUI_TimerText = "";
	public const string PlayerJoinUI_ReturnButton = "Return";

	public const string PlayerJoinUI_PlayerStatus_SlotTaken_Ready = "Ready!";
	public const string PlayerJoinUI_PlayerStatus_SlotTaken_ReadyUp = " to ready up";
	public const string PlayerJoinUI_PlayerStatus_EmptySlot = "Slot is empty";

	#endregion

	#endregion

	#region Character Selection Scene 

	#region Selection UI 

	public const string PlayerSelectionUI_ReadyButton = "OK";
	public const string PlayerSelectionUI_BackButton = "BACK";

	#endregion

	#endregion

	#region IN GAME

	public const string PauseMenu_Title = "Paused";
	public const string PauseMenu_Subtitle_Text1 = "Press";
	public const string PauseMenu_Subtitle_ButtonText = "B";
	public const string PauseMenu_Subtitle_Text2 = "to return";
	
	public const string PauseMenu_Resume = "Resume";
	public const string PauseMenu_Restart = "Restart";
	public const string PauseMenu_Options = "Options";
	public const string PauseMenu_Leave = "Leave Game";

	public const string PauseMenu_ConfirmModal_Title = "Leave Game";
	public const string PauseMenu_ConfirmModal_Subtitle = "Are you sure?";
	public const string PauseMenu_ConfirmModal_ConfirmQuit = "Quit Game";
	public const string PauseMenu_ConfirmModal_ConfirmCancel = "Cancel";

	#endregion

	#endregion

	#region Input System 

	#region Device Display Name Keys 

	public const string XboxControllerDevice = "Controller";
	public const string NintendoSwitch = "Nintendo Switch";
	public const string GamepadDevice = "Gamepad";
	public const string SonyControllerDevice = "Sony Computer Entertainment";
	public const string Playstation4Controller = "Wireless Device";
	public const string MouseDevice = "Mouse";
	public const string KeyboardDevice = "Keyboard";
	public const string JoystickDevice = "Joystick";

	#endregion

	#region Action Control Scheme Types - Keys 

	public const string ActionControlScheme_Gamepad = "Gamepad";
	public const string ActionControlScheme_Joystick = "Joystick";
	public const string ActionControlScheme_KeyboardMouse = "Keyboard&Mouse";

	#endregion

	#endregion

}
