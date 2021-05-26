#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
#endregion


/// <summary>
///		The Resource Asset Folder Names 
/// </summary>
public enum ResourceFolder
{
	None = -1,
	MountablePrefabs = 0,
	PlatformPrefabs = 1,
	CursorPrefabs = 2,
	WizardPrefabs = 3,
	CinemachineCameraPrefabs = 4,
	PlayerCameraPrefabs = 5,
	PlayerSelectionUIPrefabs = 6,
	ControllerInputIcons = 7,
	ControllerUIIconsData = 8
}

/// <summary>
///		Types of Game Tag's 
/// </summary>
public enum GameTag
{
	MainCamera,
	GameController,
	CM_Camera,
	Cursor,
	PlayerJoinStatus,
	SelectionUIManager,
	CharacterSelectionInputManager,
	CinemachineCameraHolder,
	CinemachineCamera,
	Environment,
	CharacterTitle,
}

/// <summary>
///		Asset Resources - These are just your standard prefabs 
/// </summary>
public enum Asset
{
	None = -1,
	PlayerSelectPlatform,
	Cursor,
	Camera,
	CinemachineCamera,
	Wizard,
	Mountable,
	PS4,
	Xbox,
	NintendoSwitch
}


/// <summary>
///		Scene Assets - Game Object Assets with Index's in the scene 
/// </summary>
public enum SceneAsset { 
	None = -1,
	SelectionUI, 
	WizardSelectionSpawn,
	Cursor,
	StatusText,
	StatusIcon,
	SelectionUI_Next,
	SelectionUI_Prev,
	SelectionUI_ReadyUp,
	SelectionUI_BackButton,
	SelectionUI_CharacterName,
	SelectionUI_CharacterStats,
	SelectionUI_StatsTitle,
	SelectionUI_FlavourText,
	SelectionUI_Mobility,
	SelectionUI_Mobility_WeightSlider,
	SelectionUI_Mobility_WeightBarLabel,
	SelectionUI_Mobility_WeightBarTextValue,
	SelectionUI_Survivability,
	SelectionUI_Survivability_HealthRatingSlider,
	SelectionUI_Survivability_HealthRatingBarLabel,
	SelectionUI_Survivability_HealthRatingBarTextValue,
	SelectionUI_Survivability_DefenceRatingSlider,
	SelectionUI_Survivability_DefenceRatingBarLabel,
	SelectionUI_Survivability_DefenceRatingBarTextValue,
	SelectionUI_OffensiveAbility,
	SelectionUI_OffensiveAbility_AttackRatingSlider,
	SelectionUI_OffensiveAbility_AttackRatingBarLabel,
	SelectionUI_OffensiveAbility_AttackRatingBarTextValue,
	SelectionUI_OffensiveAbility_AbilityCooldownSlider,
	SelectionUI_OffensiveAbility_AbilityCooldownBarLabel,
	SelectionUI_OffensiveAbility_AbilityCooldownBarTextValue
}

/// <summary>
///		Game Controller Type - The type of input device controller 
/// </summary>
public enum GameControllerType
{
	XboxController = 1,
	Playstation4Controller = 2,
	NintendoSwitchController = 3,
	JoystickController = 4,
	MouseController = 5,
	KeyboardController = 6,
	UnknownController = -1,
}


/// <summary>
///		Split Screen Mode's 
/// </summary>
public enum SplitScreenMode { NoConnectedPlayers = 0, SinglePlayer = 1, TwoPlayer = 2, ThreePlayer = 3, FourPlayer = 4 };

/// <summary>
///		The different types of split screen camera view's 
/// </summary>
public enum CameraView { Fullscreen, TopScreen, BottomScreen, UpperLeft, UpperRight, LowerLeft, LowerRight };


/// <summary>
///		Static Utility Class for finding specific Unity Objects & UI 
/// </summary>
public static class GameEntity
{

	/// <summary>
	///		Reference to the Game Object Tags
	/// </summary>
	private static Dictionary<GameTag, string> m_GameObjectTags = new Dictionary<GameTag, string>
	{
		{ GameTag.MainCamera, "MainCamera" },
		{ GameTag.GameController, "GameController" },
		{ GameTag.CM_Camera, "CM_Camera" },
		{ GameTag.Cursor, "Cursor" },
		{ GameTag.PlayerJoinStatus, "PlayerJoinStatus" },
		{ GameTag.CharacterSelectionInputManager, "CharacterSelectionInputManager" },
		{ GameTag.SelectionUIManager, "PlayerSelectionUI" },
		{ GameTag.CinemachineCameraHolder, "CinemachineCameraHolder" },
		{ GameTag.CinemachineCamera, "CinemachineCamera" },
		{ GameTag.Environment, "Environment" },
		{ GameTag.CharacterTitle, "CharacterTitle" }
	};

	/// <summary>
	///		Reference to the types of asset folders to search 
	/// </summary>
	private static Dictionary<ResourceFolder, string> m_ResourceFolders = new Dictionary<ResourceFolder, string>
	{
		{ ResourceFolder.None, "" },
		{ ResourceFolder.MountablePrefabs, "MountablePrefabs" },
		{ ResourceFolder.PlatformPrefabs, "PlayerSelectionPlatformPrefabs" },
		{ ResourceFolder.CursorPrefabs, "CursorPrefabs" },
		{ ResourceFolder.WizardPrefabs, "WizardSelectionPrefabs" },
		{ ResourceFolder.CinemachineCameraPrefabs, "Cinemachine_CinemachineCameraPrefabs" },
		{ ResourceFolder.PlayerCameraPrefabs, "Cinemachine_PlayerCameraPrefabs" },
		{ ResourceFolder.PlayerSelectionUIPrefabs, "SelectionUIPrefabs" },
		{ ResourceFolder.ControllerInputIcons, "ControllerInputIcons" },
		{ ResourceFolder.ControllerUIIconsData, "ControllerInputIcons/Data" }
	};

	private static Dictionary<Asset, string> m_AssetTypes = new Dictionary<Asset, string>
	{
		{ Asset.PlayerSelectPlatform, "PlayerSelectPlatform" },
		{ Asset.Cursor, "Cursor" },
		{ Asset.Camera, "Camera" },
		{ Asset.CinemachineCamera, "CinemachineCamera" },
		{ Asset.Wizard, "SelectableWizard" },
		{ Asset.Mountable, "SelectableMountable" },
		{ Asset.PS4, "PS4/PS4" },
		{ Asset.Xbox, "Xbox/XboxOne" },
		{ Asset.NintendoSwitch, "NintendoSwitch/Switch" },
		{ Asset.None, "" }
	};

	/// <summary>
	///		Dictionary of scene assets - Searching 
	/// </summary>
	private static Dictionary<SceneAsset, string> m_SceneAssets = new Dictionary<SceneAsset, string>
	{
		{ SceneAsset.None, "" },
		{ SceneAsset.SelectionUI, "SelectionUI" },
		{ SceneAsset.WizardSelectionSpawn, "WizardSelectionSpawn" },
		{ SceneAsset.StatusText, "Status" },
		{ SceneAsset.StatusIcon, "Icon" },
		{ SceneAsset.SelectionUI_CharacterName, "CharacterName" },
		{ SceneAsset.SelectionUI_CharacterStats, "CharacterStats" },
		{ SceneAsset.SelectionUI_StatsTitle, "StatsTitle" },
		{ SceneAsset.SelectionUI_FlavourText, "FlavourText" },
		{ SceneAsset.SelectionUI_Next, "NextButton" },
		{ SceneAsset.SelectionUI_Prev, "PreviousButton" },
		{ SceneAsset.SelectionUI_ReadyUp, "ReadyUpButton" },
		{ SceneAsset.SelectionUI_BackButton, "BackButton" },
		{ SceneAsset.SelectionUI_Mobility, "Mobility" },
		{ SceneAsset.SelectionUI_Mobility_WeightSlider, "WeightSlider" },
		{ SceneAsset.SelectionUI_Mobility_WeightBarLabel, "WeightBarLabel" },
		{ SceneAsset.SelectionUI_Mobility_WeightBarTextValue, "WeightBar_ValueText" },
		{ SceneAsset.SelectionUI_Survivability, "Survivability" },
		{ SceneAsset.SelectionUI_Survivability_HealthRatingSlider, "HealthRatingSlider" },
		{ SceneAsset.SelectionUI_Survivability_HealthRatingBarLabel, "HealthRatingBarLabel" },
		{ SceneAsset.SelectionUI_Survivability_HealthRatingBarTextValue, "HealthRatingBar_ValueText" },
		{ SceneAsset.SelectionUI_Survivability_DefenceRatingSlider, "DefenceRatingSlider" },
		{ SceneAsset.SelectionUI_Survivability_DefenceRatingBarLabel, "DefenceRatingBarLabel" },
		{ SceneAsset.SelectionUI_Survivability_DefenceRatingBarTextValue, "DefenceRatingBar_ValueText" },
		{ SceneAsset.SelectionUI_OffensiveAbility, "OffensiveAbility" },
		{ SceneAsset.SelectionUI_OffensiveAbility_AttackRatingSlider, "AttackRatingSlider" },
		{ SceneAsset.SelectionUI_OffensiveAbility_AttackRatingBarLabel, "AttackRatingBarLabel" },
		{ SceneAsset.SelectionUI_OffensiveAbility_AttackRatingBarTextValue, "AttackRatingBar_ValueText" },
		{ SceneAsset.SelectionUI_OffensiveAbility_AbilityCooldownSlider, "AbilityCooldownSlider" },
		{ SceneAsset.SelectionUI_OffensiveAbility_AbilityCooldownBarLabel, "AbilityCooldownBarLabel" },
		{ SceneAsset.SelectionUI_OffensiveAbility_AbilityCooldownBarTextValue, "AbilityCooldownBar_ValueText" }
	};

	private static Dictionary<CameraView, Rect> m_CameraViewDictionary = new Dictionary<CameraView, Rect>
	{
		{ CameraView.Fullscreen, new Rect(0, 0, 1, 1) },
		{ CameraView.TopScreen, new Rect(0, 0.5f, 1, 1) },
		{ CameraView.BottomScreen, new Rect(0, -0.5f, 1, 1) },
		{ CameraView.UpperLeft, new Rect(-0.5f, 0.5f, 1, 1) },
		{ CameraView.UpperRight, new Rect(0.5f, 0.5f, 1, 1) },
		{ CameraView.LowerLeft, new Rect(0.5f, -0.5f, 1, 1 ) },
		{ CameraView.LowerRight, new Rect(-0.5f, -0.5f, 1, 1) }
	};

	private static Dictionary<string, GameControllerType> m_GameControllerTypes = new Dictionary<string, GameControllerType>
	{
		{ "Xbox Controller", GameControllerType.XboxController  },
		{ GameText.JoystickDevice, GameControllerType.JoystickController },
		{ GameText.NintendoSwitch, GameControllerType.NintendoSwitchController },
		{ "Wireless Controller", GameControllerType.Playstation4Controller },
		{ GameText.KeyboardDevice, GameControllerType.KeyboardController },
		{ GameText.MouseDevice, GameControllerType.MouseController }
	};


	/// <summary>
	///		Whether to debug the search strings for each result 
	/// </summary>
	public static bool Debugging = GameManager.Instance.Debugging;


	/// <summary>
	///		Finds a game object by its tag - Same as GameObject.FindGameObjectWithTag(tag)
	/// </summary>
	/// <param name="p_Tag"></param>
	/// <returns></returns>
	public static GameObject FindByTag(string p_Tag) => GameObject.FindGameObjectWithTag(p_Tag);

	/// <summary>
	///		Finds a game object by its game tag - Another search method for FindByTag 
	/// </summary>
	/// <param name="p_Tag"></param>
	/// <returns></returns>
	public static GameObject FindByTag(GameTag p_Tag)
	{
		string search = ReturnGameTag(p_Tag);

		if (Debugging)
		{
			Debug.Log("[GameEntity.FindByTag]: " + "(Query): `" + search + "`");
		}

		return FindByTag(search);
	}

	/// <summary>
	///		Finds all game objects by a tag 
	/// </summary>
	/// <param name="p_Tag"></param>
	/// <returns></returns>
	public static GameObject[] FindAllByTag(string p_Tag) => GameObject.FindGameObjectsWithTag(p_Tag);

	/// <summary>
	///		Finds all game objects with the GameTag enum 
	/// </summary>
	/// <param name="p_Tag"></param>
	/// <returns></returns>
	public static GameObject[] FindAllByTag(GameTag p_Tag)
	{
		string search = ReturnGameTag(p_Tag);

		if (Debugging)
		{
			Debug.Log("[GameEntity.FindAllByTag]: " + "(Query): `" + search + "`");
		}

		return FindAllByTag(search);
	}

	/// <summary>
	///		Finds an asset in the resource folder 
	/// </summary>
	/// <param name="p_AssetResourceFolder"></param>
	/// <param name="p_Querystring"></param>
	/// <returns></returns>
	public static GameObject FindAsset(ResourceFolder p_AssetResourceFolder = ResourceFolder.None, Asset p_Asset = Asset.None, SceneAsset p_SceneAsset = SceneAsset.None, bool UseSceneAsset = false)
	{
		string search = ReturnAssetFolder(p_AssetResourceFolder);


		if (UseSceneAsset == true)
		{
			search += ReturnAsset(p_SceneAsset);
		}
		else
		{
			search += ReturnAsset(p_Asset);
		}

		if (Debugging)
		{
			Debug.Log("[GameEntity.FindAsset]: " + "Query string: " + search + " Use Scene Asset: " + UseSceneAsset);
		}

		return Resources.Load<GameObject>(search);
	}

	/// <summary>
	///		Finds an indexed asset using Resources.Load - ResourceFolder/WizardSelectionPrefabs/WizardSelection1
	/// </summary>
	/// <param name="p_AssetIndex">The incrementing index</param>
	/// <param name="p_AssetResourceFolder">The Resources Asset Folder to search</param>
	/// <param name="p_IndexedAsset">The indexed asset prefab name</param>
	/// <returns></returns>
	public static GameObject[] FindAllIndexedAssets(ResourceFolder p_AssetResourceFolder = ResourceFolder.WizardPrefabs)
	{
		string search = ReturnAssetFolder(p_AssetResourceFolder);

		GameObject[] s_Resources = Resources.LoadAll<GameObject>(search);

		if (Debugging)
		{
			Debug.Log("[GameEntity.FindAllIndexedAssets]: " + "(Query): `" + search + "`");
		}

		if (s_Resources.Length > 0)
		{
			return s_Resources;
		}
		else
			return null;
	}

	/// <summary>
	///		Returns the total length of resource assets in a folder 
	/// </summary>
	/// <param name="p_AssetResourceFolder"></param>
	/// <returns></returns>
	public static int ReturnAssetLengthInFolder(ResourceFolder p_AssetResourceFolder)
	{
		string search = ReturnAssetFolder(p_AssetResourceFolder);

		return Resources.LoadAll<GameObject>(search).Length; 
	}

	/// <summary>
	///		Returns a Prefab Asset for a Player using their player index  
	/// </summary>
	/// <param name="p_AssetResourceFolder"></param>
	/// <param name="PlayerIndex">The current player's player index</param>
	/// <param name="p_Querystring"></param>
	/// <returns></returns>
	public static GameObject FindAsset(ResourceFolder p_AssetResourceFolder = ResourceFolder.None, int PlayerIndex = 1, Asset p_Asset = Asset.Cursor, SceneAsset p_SceneAsset = SceneAsset.None, bool UseSceneAsset = false)
	{
		string query = ReturnAssetFolder(p_AssetResourceFolder) + $"P{PlayerIndex}_";

		if (UseSceneAsset == true)
		{
			query += ReturnAsset(p_SceneAsset);
			
			if (Debugging)
			{
				Debug.Log("[GameEntity.FindAsset]: " + "Query string: " + query + " Use Scene Asset: " + UseSceneAsset + " Scene Asset: " + p_SceneAsset);
			}

		}
		else
		{
			query += ReturnAsset(p_Asset);

			if (Debugging)
			{
				Debug.Log("[GameEntity.FindAsset]: " + "Query string: " + query + " Use Scene Asset: " + UseSceneAsset + " Asset: " + p_Asset);
			}

		}


		return Resources.Load<GameObject>(query);
	}

	/// <summary>
	///		Finds a Sprite Asset from the asset resource folder 
	/// </summary>
	/// <param name="p_AssetResourceFolder">The asset resource folder to search</param>
	/// <param name="p_Asset">The asset to find, Options include PS4, Xbox & NintendoSwitch</param>
	/// <param name="p_Button">The button asset you are looking for</param>
	/// <returns></returns>
	public static Sprite FindAsset(ResourceFolder p_AssetResourceFolder = ResourceFolder.ControllerInputIcons, Asset p_Asset = Asset.Xbox, string p_Button = "B")
	{
		string query = $"{ReturnAssetFolder(p_AssetResourceFolder)}{ReturnAsset(p_Asset)}_{p_Button}";
		
		Debug.Log("Query: " + query);

		return Resources.Load<Sprite>(query);
	}

	/// <summary>
	///		Finds a cloned game asset transform using the players current index and the query string 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex"></param>
	/// <param name="p_QueryString"></param>
	/// <returns></returns>
	public static Transform FindGameObjectChildTransform(GameObject p_GameObjectParent, int p_CurrentPlayerIndex = 0, SceneAsset p_SceneAsset = SceneAsset.None)
	{

		string query = $"P{p_CurrentPlayerIndex}_{ReturnAsset(p_SceneAsset)}";

		if (!p_GameObjectParent.transform.Find(query))
		{
			Debug.LogWarning("[GameEntity.FindSceneAssetClone]: " + "Could not find game object parent child transform with search: " + query);
			return null;
		}
		else
		{
			return p_GameObjectParent.transform.Find(query).transform;
		}
	}

	/// <summary>
	///		Finds a scene asset by the scene asset name 
	/// </summary>
	/// <param name="p_SceneAsset"></param>
	/// <returns></returns>
	public static Transform FindSceneAsset(SceneAsset p_SceneAsset = SceneAsset.None)
	{
		string search = $"{ReturnAsset(p_SceneAsset)}";
		return GameObject.Find(search).transform;
	}

	/// <summary>
	///		Finds a scene asset by the current player's index 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex"></param>
	/// <param name="p_SceneAsset"></param>
	/// <returns></returns>
	public static Transform FindSceneAsset(int p_CurrentPlayerIndex = 0, SceneAsset p_SceneAsset = SceneAsset.None)
	{
		string search = $"P{p_CurrentPlayerIndex}_{ReturnAsset(p_SceneAsset)}";

		if (Debugging)
		{
			Debug.Log("[GameEntity.FindSceneAsset]: " + "Finding scene asset: " + search);
		}

		return GameObject.Find(search).transform;
	}

	/// <summary>
	///		Finds a scene asset clone 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex">The current players identity (index)</param>
	/// <param name="p_SceneAsset">The scene asset to search for</param>
	/// <returns></returns>
	public static Transform FindSceneAssetClone(int p_CurrentPlayerIndex = 0, SceneAsset p_SceneAsset = SceneAsset.None)
	{

		string asset = ReturnAsset(p_SceneAsset);


		string query = $"P{p_CurrentPlayerIndex}_{asset}(Clone)";
	
		if (GameObject.Find(query) != null)
		{
			return GameObject.Find(query).transform;
		}
		else
		{
			Debug.LogWarning("Could not find scene asset clone!");
			return null;
		}
	}

	/// <summary>
	///		Finds a prefab asset clone in the scene using the current player's index 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex">The current players ID index</param>
	/// <param name="p_Asset">The Asset to search for</param>
	/// <returns></returns>
	public static Transform FindAssetClone(int p_CurrentPlayerIndex = 0, Asset p_Asset = Asset.None)
	{
		string asset = ReturnAsset(p_Asset);


		string query = $"P{p_CurrentPlayerIndex}_{asset}(Clone)";

		if (Debugging)
		{
			Debug.Log("[GameEntity.FindAssetClone]: " + "Searching for asset " + query);
		}

		return GameObject.Find(query).transform;
	}

	/// <summary>
	///		Searchings the Game Object Tags dictionary using the GameTag enum, returning the string value 
	/// </summary>
	/// <param name="tag"></param>
	/// <returns></returns>
	public static string ReturnGameTag(GameTag tag)
	{
			
		if (m_GameObjectTags.TryGetValue(tag, out string value))
		{
			return value;
		}
		else
		{
			Debug.LogWarning("Could not find a game object with that tag!");
			return null;
		}
	}

	/// <summary>
	///		Returns an asset folder by the resource folder type 
	/// </summary>
	/// <param name="Folder"></param>
	/// <returns></returns>
	private static string ReturnAssetFolder(ResourceFolder Folder)
	{
		if (m_ResourceFolders.TryGetValue(Folder, out string s_AssetFolder))
		{
			return s_AssetFolder + "/";
		}
		else
		{
			Debug.LogWarning("[GameEntity.ReturnAssetFolder]: " + "Could not find an asset folder with that name!");
			return "";
		}
	}

	/// <summary>
	///		Returns an asset using the Asset enum type 
	/// </summary>
	/// <param name="asset"></param>
	/// <returns></returns>
	public static string ReturnAsset(Asset asset)
	{
		if (m_AssetTypes.TryGetValue(asset, out string s_ResourceAsset))
		{
			if (Debugging)
			{
				Debug.Log("[ReturnAsset]: " + "Asset: `" + asset + "`" + " Result: " + s_ResourceAsset);
			}
			return s_ResourceAsset;
		}
		else
		{
			Debug.LogWarning("Could not find asset!");
			return "";
		}
	}
	
	/// <summary>
	///		Retruns a scene asset using the scene asset enum type 
	/// </summary>
	/// <param name="asset"></param>
	/// <returns></returns>
	public static string ReturnAsset(SceneAsset asset)
	{


		if (m_SceneAssets.TryGetValue(asset, out string s_SceneAsset))
		{
			if (Debugging)
			{
				Debug.Log("[ReturnAsset]: " + "Scene Asset: `" + asset + "`" + " Result: " + s_SceneAsset);
			}
			return s_SceneAsset;
		}
		else
		{
			Debug.LogWarning("Could not find scene asset!");
			return "";
		}
	}

	/// <summary>
	///		Finds and gets the player camera layer 
	/// </summary>
	/// <param name="PlayerIndex"></param>
	/// <returns></returns>
	public static int GetPlayerCameraLayer(int PlayerIndex) => ReturnCameraLayer(PlayerIndex);

	/// <summary>
	///		Finds and gets the player's camera tag 
	/// </summary>
	/// <param name="PlayerIndex"></param>
	/// <returns></returns>
	public static string GetPlayerCameraTag(int PlayerIndex) => $"{ReturnGameTag(GameTag.CM_Camera)}{PlayerIndex}";

	/// <summary>
	///		Finds and returns the character selection title tag 
	/// </summary>
	/// <param name="p_CurrentPlayerIndex"></param>
	/// <returns></returns>
	public static string GetCharacterSelectionTitle(int p_CurrentPlayerIndex) => $"{ReturnGameTag(GameTag.CharacterTitle)}{p_CurrentPlayerIndex}";

	/// <summary>
	///		Returns the camera layer defined in the playercameralayer enum based on the player's index 
	/// </summary>
	/// <param name="PlayerIndex"></param>
	/// <returns></returns>
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

	/// <summary>
	///		The different types of camera layer index's 
	/// </summary>
	private enum PlayerCameraLayer { None = 0, Camera1 = 11, Camera2 = 12, Camera3 = 13, Camera4 = 14 };

	/// <summary>
	///		Set's the Split Screen Camera's Views
	///		Requires ACTIVE scene camera's and the SplitScreenMode 
	/// </summary>
	/// <param name="AvailableCameras"></param>
	/// <param name="Mode"></param>
	public static void SetSplitScreenCameras(List<Camera> AvailableCameras, SplitScreenMode Mode)
	{
		if (AvailableCameras.Count > 0 && AvailableCameras.Count <= GameManager.Instance.MaxPlayers)
		{ 
			foreach (KeyValuePair<CameraView, Rect> element in m_CameraViewDictionary)
			{

				switch (Mode)
				{
					case SplitScreenMode.NoConnectedPlayers:
						Debug.LogError("No connected players!");
						break;
					case SplitScreenMode.SinglePlayer:
						{
							if (element.Key == CameraView.Fullscreen)
							{
								AvailableCameras[0].rect = element.Value;
							}
						}
						break;
					case SplitScreenMode.TwoPlayer:
						{
							if (element.Key == CameraView.TopScreen)
							{
								AvailableCameras[0].rect = element.Value;
							}
							else if (element.Key == CameraView.BottomScreen)
							{
								AvailableCameras[1].rect = element.Value;
							}
						}
						break;
					case SplitScreenMode.ThreePlayer:
						{
							if (element.Key == CameraView.TopScreen)
							{
								AvailableCameras[0].rect = element.Value;
							}
							else if (element.Key == CameraView.LowerLeft)
							{
								AvailableCameras[1].rect = element.Value;
							}
							else if (element.Key == CameraView.LowerRight)
							{
								AvailableCameras[2].rect = element.Value;
							}
						}
						break;
					case SplitScreenMode.FourPlayer:
						{
							if (element.Key == CameraView.UpperLeft)
							{
								AvailableCameras[0].rect = element.Value;
							}
							else if (element.Key == CameraView.UpperRight)
							{
								AvailableCameras[1].rect = element.Value;
							}
							else if (element.Key == CameraView.LowerLeft)
							{
								AvailableCameras[2].rect = element.Value;
							}
							else if (element.Key == CameraView.LowerRight)
							{
								AvailableCameras[3].rect = element.Value;
							}
						}
						break;
				}
			}
		}
	}

	/// <summary>
	///		Sets children of a gameobject's layer to the player's camera layer
	/// </summary>
	/// <param name="p_GameObject"></param>
	/// <param name="p_PlayerIndex"></param>
	public static void SetLayerRecursively(GameObject p_GameObject, int p_PlayerIndex)
	{
		foreach (Transform trans in p_GameObject.GetComponentsInChildren<Transform>(true))
		{
			int layer = ReturnCameraLayer(p_PlayerIndex);
			trans.gameObject.layer = layer;
		}
	}

	/// <summary>
	///		Returns the game controller type, 
	/// </summary>
	/// <param name="p_GameControllerType"></param>
	/// <returns></returns>
	public static GameControllerType GetGameControllerType(Gamepad p_CurrentGamepadDevice)
	{
		

		if (m_GameControllerTypes.TryGetValue(p_CurrentGamepadDevice.displayName, out GameControllerType s_Result))
		{
			Debug.Log("[GameEntity.GetGameControllerType]: " + "Game Controller Found: " + s_Result);
			return s_Result;
		}
		else
		{
			Debug.Log("[GameEntity.GetGameControllerType]: " + "Game controller could not be found " + s_Result);
			return GameControllerType.UnknownController;
		}
	}

	/// <summary>
	///		Returns Game Controller Scriptable Object Data, depending on the type of game controller the player is using 
	/// </summary>
	/// <param name="p_ResourceFolder"></param>
	/// <param name="p_ControllerType"></param>
	/// <returns></returns>
	public static GameControllerUIData GetControllerUIData(GameControllerType p_ControllerType = GameControllerType.UnknownController)
	{
		string s_ResourceFolderData = $"{ReturnAssetFolder(ResourceFolder.ControllerUIIconsData)}";

		Debug.Log("[GameEntity.GetControllerUIData]: " + "Searching for asset resource in " + s_ResourceFolderData);
	
		GameControllerUIData[] s_GameControllerUIDataObjects = Resources.LoadAll<GameControllerUIData>(s_ResourceFolderData);

		bool s_ControllerDataExists = s_GameControllerUIDataObjects.Any(data => data.ControllerType == p_ControllerType);

		Debug.Log("[GameEntity.GetControllerUIData]: " + "Game Controller UI Data Exists: " + s_ControllerDataExists);

		if (s_GameControllerUIDataObjects.Length <= 0 || !s_ControllerDataExists)
		{
			Debug.LogWarning("[GameEntity.GetControllerUIData]: " + "Could not find UI data for " + p_ControllerType);
			return null;
		}
		else
		{
			Debug.Log("[GameEntity.GetControllerUIData]: " + "Found Game Controller Data for " + p_ControllerType);
			return s_GameControllerUIDataObjects.FirstOrDefault(controller => controller.ControllerType == p_ControllerType);
		}
	}
}
