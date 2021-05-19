#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion


public enum ResourceAssetFolder {
	None = -1,
	BroomSelectionPrefabs = 0,
	WizardSelectionPlatformPrefabs = 1,
	CursorPrefabs = 2,
	WizardSelectionPrefabs = 3,
	CameraPrefabs_Cinemachine_CMPlayerCameraPrefabs = 4,
	CameraPrefabs_Cinemachine_PlayerCameraPrefabs = 5,
	SelectionUIPrefabs = 6
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

	#region Action Control Scheme Types - Keys 

	public const string ActionControlScheme_Gamepad = "Gamepad";
	public const string ActionControlScheme_Joystick = "Joystick";
	public const string ActionControlScheme_KeyboardMouse = "Keyboard&Mouse";

	#endregion

	#endregion

	#region GameObject Tags - Keys  
	
	public const string MainCameraTag = "MainCamera";
	public const string GameControllerTag = "GameController";
	public const string CM_Camera1Tag = "CM_Camera1";
	public const string CM_Camera2Tag = "CM_Camera2";
	public const string CM_Camera3Tag = "CM_Camera3";
	public const string CM_Camera4Tag = "CM_Camera4";
	public const string CursorTag = "Cursor";
	public const string PlayerJoinStatusTag = "PlayerJoinStatus";
	public const string SelectionUIManagerTag = "PlayerSelectionUI";

	public const string CinemachineCameraHolderTag = "CinemachineCameraHolder";
	public const string CinemachineCameraTag = "CinemachineCamera";
	public const string EnvironmentTag = "Environment";

	public static string GetPlayerCameraTag(int PlayerIndex = 1) => $"CM_Camera{PlayerIndex}";

	#endregion

	public static GameObject GetGameObject(string GameObjectTag) => GameObject.FindGameObjectWithTag(GameObjectTag);

	public static GameObject[] GetCinemachineCameras(string CinemachineCameraTag) => GameObject.FindGameObjectsWithTag(CinemachineCameraTag);


	#region Layer Keys 

	public const string PlayerCamera_Layer = "Camera_";

	public enum PlayerCameraLayer { None = 0, Camera1 = 11, Camera2 = 12, Camera3 = 13, Camera4 = 14 };

	public static int GetPlayerCameraLayer(int PlayerIndex) => ReturnCameraLayer(PlayerIndex);

	private static int ReturnCameraLayer(int PlayerIndex)
	{
		switch (PlayerIndex)
		{
			case 1:
				return (int)PlayerCameraLayer.Camera1;
			case 2:
				return (int)PlayerCameraLayer.Camera2;
			case 3:
				return (int)PlayerCameraLayer.Camera3;
			case 4:
				return (int)PlayerCameraLayer.Camera4;
			default:
				return (int)PlayerCameraLayer.None;
		}
	}

	#endregion


	#region Asset Resource Keys 

	public const string ResourceAsset_PlayerSelectPlatform = "PlayerSelectPlatform";
	public const string ResourceAsset_Cursor = "Cursor";
	public const string ResourceAsset_Camera = "Camera";
	public const string ResourceAsset_CinemachineCamera = "CinemachineCamera";


	public const string SelectableResourceAsset_WizardPrefab = "SelectableWizard";
	public const string SelectableResourceAsset_BroomstickPrefab = "SelectableBroom";


	public const string SceneObject_WizardSelectionSpawn = "WizardSelectionSpawn";
	public const string SceneObject_SelectionUI = "SelectionUI";

	#endregion


	#region Resource Asset Folders  

	public const string ResourceAssetFolder_CameraPrefabs_CMPlayerCameraPrefabs = "Cinemachine_PlayerCameraPrefabs";
	public const string ResourceAssetFolder_CameraPrefabs_CinemachineCamera = "Cinemachine_CinemachineCameraPrefabs";
	public const string ResourceAssetFolder_BroomSelectionPrefabs = "BroomSelectionPrefabs";
	public const string ResourceAssetFolder_CursorPrefabs = "CursorPrefabs";
	public const string ResourceAssetFolder_PlayerSelectionPlatformPrefabs = "PlayerSelectionPlatformPrefabs";
	public const string ResourceAssetFolder_WizardSelectionPrefabs = "WizardSelectionPrefabs";
	public const string ResourceAssetFolder_SelectionUIPrefabs = "SelectionUIPrefabs";
	public const string ResourceAssetFolder_None = "";

	#endregion

	#region TESTING 

	public static class ResourceAssets
	{
		public static string Cursor => nameof(ResourceAsset_Cursor);

		public static string Camera => nameof(ResourceAsset_Camera);

		public static string CinemachineCamera => nameof(ResourceAsset_CinemachineCamera);

		public static string WizardCharacter => nameof(SelectableResourceAsset_WizardPrefab);

		public static string Broomstick => nameof(SelectableResourceAsset_BroomstickPrefab);
	}


	#endregion

	#region Public Static Methods 

	/// <summary>
	///		Finds a prefab asset in the resources folder 
	/// </summary>
	/// <param name="folder">The Resources folder type to search</param>
	/// <param name="Querystring">The asset you are trying to locate inside the folder</param>
	/// <returns></returns>
	public static string FindAsset(ResourceAssetFolder folder = ResourceAssetFolder.None, string Querystring = "") => ReturnResourceAssetFolderName(folder).ToString() + $"/{Querystring}";

	/// <summary>
	///		Finds an asset that has an indexed value
	///		Example - SelectableWizard1, SelectableWizard2, SelectableWizard3
	/// </summary>
	/// <param name="folder">The resource folder to search </param>
	/// <param name="Index">The index of the asset</param>
	/// <param name="Querystring">The asset name string</param>
	/// <returns>
	///		String | null - {ResourceAssetFolder}/{QueryString}{Index}
	///		Example - `WizardSelectionPrefabs/SelectableWizard1` 
	/// </returns>
	public static string FindAssetWithIndex(ResourceAssetFolder folder = ResourceAssetFolder.None, string Querystring = "", int Index = 1) => ReturnResourceAssetFolderName(folder).ToString() + $"/{Querystring}{Index}";

	/// <summary>
	///		Formats the search string for locating the player asset in the current scene 
	/// </summary>
	/// <param name="PlayerIndex">The current player's index</param>
	/// <param name="QueryString">The search string</param>
	/// <returns>
	///		String | null - "P{PlayerIndex}_{QueryString}
	/// </returns>
	public static string FindPlayerGameObjectSceneAsset(int PlayerIndex = 1, string QueryString = "") => $"P{PlayerIndex}_{QueryString}";

	/// <summary>
	///		Formats the search string for locating a prefab specific asset in the resources folder using the players index 
	/// </summary>
	/// <param name="AssetFolder">The ResourceAssetFolder type to search</param>
	/// <param name="PlayerIndex">The current players index (1, 2, 3, 4)</param>
	/// <param name="Querystring">The player asset item you are searching for, Example - CursorPrefabs/P1_Cursor</param>
	/// <returns></returns>
	public static string FindPlayerAssetInResourceFolder(ResourceAssetFolder AssetFolder = ResourceAssetFolder.None, int PlayerIndex = 1, string Querystring = "") => ReturnResourceAssetFolderName(AssetFolder).ToString() + $"/P{PlayerIndex}_{Querystring}";

	#endregion

	#region Private Static Methods 

	/// <summary>
	///		Returns the name of the resource asset folder 
	/// </summary>
	/// <param name="Folder"></param>
	/// <returns></returns>
	private static string ReturnResourceAssetFolderName(ResourceAssetFolder Folder)
	{
		switch (Folder)
		{
			case ResourceAssetFolder.BroomSelectionPrefabs:
					return ResourceAssetFolder_BroomSelectionPrefabs;
			case ResourceAssetFolder.WizardSelectionPlatformPrefabs:
					return ResourceAssetFolder_PlayerSelectionPlatformPrefabs;
			case ResourceAssetFolder.CursorPrefabs:
					return ResourceAssetFolder_CursorPrefabs;
			case ResourceAssetFolder.WizardSelectionPrefabs:
					return ResourceAssetFolder_WizardSelectionPrefabs;
			case ResourceAssetFolder.CameraPrefabs_Cinemachine_CMPlayerCameraPrefabs:
					return ResourceAssetFolder_CameraPrefabs_CMPlayerCameraPrefabs;
			case ResourceAssetFolder.CameraPrefabs_Cinemachine_PlayerCameraPrefabs:
					return ResourceAssetFolder_CameraPrefabs_CinemachineCamera;
			case ResourceAssetFolder.SelectionUIPrefabs:
					return ResourceAssetFolder_SelectionUIPrefabs;
			default:
				return ResourceAssetFolder_None;
		}
	}

	#endregion

}
