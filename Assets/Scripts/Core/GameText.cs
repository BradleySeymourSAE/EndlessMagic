#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion


public enum GameAssetResource {
	BroomSelectionPrefabs = 0,
	WizardSelectionPlatformPrefabs = 1,
	CursorPrefabs = 2,
	WizardSelectionPrefabs = 3,
	None = -1,
};

/// <summary>
///		Static Class of UI text & button strings 
/// </summary>
public static class GameText
{

	#region UI 

	#region Main Menu

	public const string MainMenuUI_Title = "Endless Magic";
	public const string MainMenuUI_Subtitle = "Battle your friends n shiet";
	public const string MainMenuUI_Version = "Version 1.0.5 Development";
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
	public const string PlayerJoinUI_PlayerStatus_SlotTaken_ReadyUp = "Press B to ready up";
	public const string PlayerJoinUI_PlayerStatus_EmptySlot = "Slot is empty";

	#endregion

	#region Loading Screen Menu ( WIP ) 

	public const string LoadingScreenUI_Title = "Loading";
	public const string LoadingScreenUI_Progress = "0";

	#endregion

	#endregion

	#region Input System 

	#region Device Display Name Keys 

	public const string XboxControllerDevice = "Controller";
	public const string GamepadDevice = "Gamepad";
	public const string PS4ControllerDevice = "Sony Computer Entertainment";
	public const string MouseDevice = "Mouse";
	public const string KeyboardDevice = "Keyboard";
	public const string JoystickDevice = "Joystick";

	#endregion

	#region Action Control Scheme Keys

	public const string ActionControlScheme_Gamepad = "Gamepad";
	public const string ActionControlScheme_Joystick = "Joystick";
	public const string ActionControlScheme_KeyboardMouse = "Keyboard&Mouse";

	#endregion

	#endregion

	#region Game Object Tag Keys 
	
	public const string MainCameraTag = "MainCamera";
	public const string GameControllerTag = "GameController";
	public const string CM_Camera1Tag = "CM_Camera1";
	public const string CM_Camera2Tag = "CM_Camera2";
	public const string CM_Camera3Tag = "CM_Camera3";
	public const string CM_Camera4Tag = "CM_Camera4";
	public const string CursorTag = "Cursor";
	public const string PlayerJoinStatusTag = "PlayerJoinStatus";


	#endregion


	#region Layer Keys 

	public const string P1CameraLayer = "Camera_P1";
	public const string P2CameraLayer = "Camera_P2";
	public const string P3CameraLayer = "Camera_P3";
	public const string P4CameraLayer = "Camera_P4";

	#endregion


	#region Asset Resource Keys 

	public const string GameObjectResource_PlayerSelectionPlatform = "PlayerSelectPlatform";
	public const string GameObjectResource_WizardSelectionSpawn = "WizardSelectionSpawn";
	public const string GameObjectResource_Cursor = "Cursor";

	public const string AssetResourceCategory_BroomSelectionPrefabs = "BroomSelectionPrefabs";
	public const string AssetResourceCategory_CursorPrefabs = "CursorPrefabs";
	public const string AssetResourceCategory_WizardSelectionPlatformPrefabs = "PlayerSelectionPlatformPrefabs";
	public const string AssetResourceCategory_WizardSelectionPrefabs = "WizardSelectionPrefabs";

	#region @TODO 
	
	/* 
		public const string AssetResource_WizardDraco = "";
		public const string AssetResource_WizardHermione = "";
		public const string AssetResource_WizardSirius = "";
		public const string AssetResource_WizardSnape = "";
		public const string AssetResource_WizardYennefer = "";
	*/

	#endregion

	

	/// <summary>
	///		Returns an asset resource key string
	/// </summary>
	/// <param name="PlayerIndex">Player Device Index (1, 2, 3 or 4)</param>
	/// <param name="AssetResource">The Prefab that is retrieved from `Resources/FolderName` </param>
	/// <returns></returns>
	public static string ReturnAssetResourceKey(int PlayerIndex = 1, GameAssetResource AssetResource = GameAssetResource.None, string p_Query = "", bool LoadResourcesFromFolder = false)
	{
		if (LoadResourcesFromFolder)
		{
			string AssetResourceFolder = FindAssetResource(AssetResource);

			return $"{AssetResourceFolder}/P{PlayerIndex}_{p_Query}";
		}
		else
		{
			return $"P{PlayerIndex}_{p_Query}";
		}
	}


	private static string FindAssetResource(GameAssetResource Resource)
	{
		switch (Resource)
		{
			case GameAssetResource.BroomSelectionPrefabs:
					return AssetResourceCategory_BroomSelectionPrefabs;
			case GameAssetResource.WizardSelectionPlatformPrefabs:
				{
					return AssetResourceCategory_WizardSelectionPlatformPrefabs;
				}
			case GameAssetResource.CursorPrefabs:
				{
					return AssetResourceCategory_CursorPrefabs;
				}
			case GameAssetResource.WizardSelectionPrefabs:
				{
					return AssetResourceCategory_WizardSelectionPrefabs;
				}
				break;
			default:
				return null;
		}
	}


	#endregion

}
