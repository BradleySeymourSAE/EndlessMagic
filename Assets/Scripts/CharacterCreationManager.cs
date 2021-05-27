#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
#endregion


/// <summary>
///		Character Creation Manager 
///		- Handles Setting up wizard characters, cameras and objects 
///		- Handles Settings up players mountable vehicle selections 
/// </summary>
public class CharacterCreationManager : MonoBehaviour
{

	#region Public Variables 

	/// <summary>
	///		List of starting platform spawn positions 
	/// </summary>
	public List<Transform> platformSpawnPositions = new List<Transform>();

	public CharacterStats[] characterStatsData;

	#endregion

	#region Private Variables
	/// <summary>
	///		List of active player cameras 
	/// </summary>
	[Header("Debugging")]
	[SerializeField] private List<Camera> m_ActivePlayerCameras = new List<Camera>();

	/// <summary>
	///		List of selection UI 
	/// </summary>
	[SerializeField] private List<Canvas> m_SelectionUI = new List<Canvas>();

	/// <summary>
	///		List of the Wizard Prefab Spawn Positions 
	/// </summary>
	[SerializeField] private List<Transform> m_WizardCharacterPlatformSpawnPositions = new List<Transform>();

	/// <summary>
	///		List of all the platform prefab spawn points after they are set 
	/// </summary>
	[SerializeField] private List<Transform> m_PlatformPrefabSpawnPoints = new List<Transform>();

	/// <summary>
	///		Do we gave the wizard selection spawn points? 
	/// </summary>
	private bool hasWizardSelectionSpawnPoints = false;

	#endregion

	#region Debugging 

	[Header("--- Debugging ---")]
	[SerializeField] private int m_Players;

	[SerializeField] private Color m_PlatformSpawnPointColor = Color.red;

	[SerializeField] private Color m_WizardSelectionSpawnPointColor = Color.green;

	[SerializeField] private float m_GizmoSize = 0.5f;

	[SerializeField] private bool m_Debugging = true;

	[SerializeField] private bool m_UseWireSphere = true;

	#endregion

	#region Unity References 

	private void OnDrawGizmos()
	{
		if (m_Debugging)
		{ 


			if (platformSpawnPositions.Count > 0)
			{
				foreach (Transform platform in platformSpawnPositions)
				{
					Gizmos.color = m_PlatformSpawnPointColor;
					if (m_UseWireSphere)
					{
						Gizmos.DrawWireSphere(platform.position, m_GizmoSize);
					}
					else
					{
						Gizmos.DrawSphere(platform.position, m_GizmoSize);
					}
				}
			}

			if (hasWizardSelectionSpawnPoints)
			{
				foreach (Transform wizardSpawnPoint in m_WizardCharacterPlatformSpawnPositions)
				{
					Gizmos.color = m_WizardSelectionSpawnPointColor;

					if (m_UseWireSphere)
					{
						Gizmos.DrawWireSphere(wizardSpawnPoint.position, m_GizmoSize);
					}
					else
					{
						Gizmos.DrawSphere(wizardSpawnPoint.position, m_GizmoSize);
					}	
				}
			}


		}
	}


	private void Start()
	{
		// Remove all the platform prefab spawn points 
		m_PlatformPrefabSpawnPoints.Clear();
		m_ActivePlayerCameras.Clear();
		m_Players = GameManager.Instance.ConnectedPlayers;

		// Loop through potential platform spawn positions 
		if (platformSpawnPositions.Count > 0)
		{ 
			foreach (Transform platform in platformSpawnPositions)
			{
				m_PlatformPrefabSpawnPoints.Add(platform);
			}
		}
		


		Setup(m_Players);
	}


	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the Character Creation Scene - Based on the amount of players that have joined the game 
	/// </summary>
	/// <param name="PlayerCount"></param>
	public void Setup(int PlayerCount)
	{
		m_ActivePlayerCameras.Clear();
		Debug.Log("[CharacterCreationManager.Setup]: " + "Setting up camera & Game object references for " + PlayerCount + " players!");

		int s_CurrentPlayerIndex = 0;

		for (int i = 0; i < PlayerCount; i++)
		{
			s_CurrentPlayerIndex++;

			List<Transform> s_WizardSelectionsSpawnedIn = new List<Transform>();
			List<Transform> s_MountableVehicleSelectionsSpawnedIn = new List<Transform>();

			s_WizardSelectionsSpawnedIn.Clear();
			s_MountableVehicleSelectionsSpawnedIn.Clear();

			Transform s_Environment = GameEntity.FindByTag(GameTag.Environment).transform;
			Transform s_CameraHolder = GameEntity.FindByTag(GameTag.CinemachineCameraHolder).transform;


			GameObject s_PlatformAssetPrefab = GameEntity.FindAsset(ResourceFolder.PlatformPrefabs, s_CurrentPlayerIndex, Asset.PlayerSelectPlatform);
			GameObject s_PlayerCameraAssetPrefab = GameEntity.FindAsset(ResourceFolder.PlayerCameraPrefabs, s_CurrentPlayerIndex, Asset.Camera);
			GameObject s_CinemachineCameraAssetPrefab = GameEntity.FindAsset(ResourceFolder.CinemachineCameraPrefabs, s_CurrentPlayerIndex, Asset.CinemachineCamera);
			GameObject s_SelectionUIAssetPrefab = GameEntity.FindAsset(ResourceFolder.PlayerSelectionUIPrefabs, s_CurrentPlayerIndex, Asset.None, SceneAsset.SelectionUI, true);
			

			GameObject platform = Instantiate(s_PlatformAssetPrefab, m_PlatformPrefabSpawnPoints[s_CurrentPlayerIndex - 1].position, s_PlatformAssetPrefab.transform.rotation);
					   platform.transform.SetParent(s_Environment);
					   platform.layer = GameEntity.GetPlayerCameraLayer(s_CurrentPlayerIndex);
					   platform.tag = GameEntity.GetPlayerCameraTag(s_CurrentPlayerIndex);
			
			Transform s_CharacterPlatformSpawnPosition = GameEntity.FindGameObjectChildTransform(platform, s_CurrentPlayerIndex, SceneAsset.WizardSelectionSpawn);

			if (s_CharacterPlatformSpawnPosition != null)
			{
				m_WizardCharacterPlatformSpawnPositions.Add(s_CharacterPlatformSpawnPosition);
			}

			// Selection UI Manager is basically the Character Creation Scene's UI Manager - This just finds the tag for it 
			// I commonly translate unity func's to javascript similarities as it's easier for me to remember 
			// Example: Instead of GameObject.FindGameObjectsWithTag("string") -> GameEntity.FindByTag(GameTag.TAGNAME) || ("string")
			Transform s_SelectionUIManager = GameEntity.FindByTag(GameTag.SelectionUIManager).transform;

			// Instantiates the Selection UI that is specific to the player 
			GameObject s_CurrentPlayerSelectionUIPrefab = Instantiate(s_SelectionUIAssetPrefab, s_SelectionUIManager.position, Quaternion.identity);

			// Set the selection UI manager as the current player selection ui prefab's parent 
			s_CurrentPlayerSelectionUIPrefab.transform.SetParent(s_SelectionUIManager);

			// Set the player selectionUIPrefab's layer to the player's camera layer. Accepts player index of 1, 2, 3 or 4 -> returns 14,15,16,17 depending (which is the layer index) 
			s_CurrentPlayerSelectionUIPrefab.layer = GameEntity.GetPlayerCameraLayer(s_CurrentPlayerIndex);

		
			// All Wizard Prefab Assets in the Wizard Prefab Asset Resource Folder 
			GameObject[] s_WizardPrefabAssets = GameEntity.FindAllIndexedAssets(ResourceFolder.WizardPrefabs);

			// All Mountable Vehicle Prefab Assets in the Asset Resource Folder 
			GameObject[] s_MountablePrefabAssets = GameEntity.FindAllIndexedAssets(ResourceFolder.MountablePrefabs);

			// Loop through the wizard prefab assets 
			for (int j = 0; j < s_WizardPrefabAssets.Length; j++)
			{

				// Instantiate the wizard prefab 
				GameObject spawnedWizardPrefab = Instantiate(s_WizardPrefabAssets[j], s_CharacterPlatformSpawnPosition.position, s_WizardPrefabAssets[j].transform.rotation);

				WizardType s_WizardType = (WizardType)j;

				CharacterStats s_CharacterStatsType = GetCharacterStats(s_WizardType);

				spawnedWizardPrefab.AddComponent<CharacterStatsUI>().Setup(s_CurrentPlayerIndex, s_CharacterStatsType);


				// Setting the Transform's Parent, Layer and Tag (To hide it from other camera's view) 
				spawnedWizardPrefab.transform.SetParent(s_CharacterPlatformSpawnPosition); // Set the spawned wizard prefab's parent position 
				spawnedWizardPrefab.layer = s_CharacterPlatformSpawnPosition.gameObject.layer; // Set the wizard prefab's layer 
				spawnedWizardPrefab.tag = s_CharacterPlatformSpawnPosition.tag; // set the wizard prefab's tag 

				// Add the spawned wizard prefab to the wizard selections spawned list 
				s_WizardSelectionsSpawnedIn.Add(spawnedWizardPrefab.transform);
				
				// Utility function i created to basically set the layer like above, except for every child transform 
				GameEntity.SetLayerRecursively(spawnedWizardPrefab, s_CurrentPlayerIndex);
			}

			// All mountable prefab assets in the mountable prefab asset resource folder 
			for (int z = 0; z < s_MountablePrefabAssets.Length; z++)
			{
				// Instantiate the mountable prefab asset 
				GameObject spawnedMountablePrefab = Instantiate(s_MountablePrefabAssets[z], s_CharacterPlatformSpawnPosition.position, s_WizardPrefabAssets[z].transform.rotation);

				// Setting the Transform's parent, layer and Tag (To hide it from the other camera's view) 
				spawnedMountablePrefab.transform.SetParent(s_CharacterPlatformSpawnPosition);
				spawnedMountablePrefab.layer = s_CharacterPlatformSpawnPosition.gameObject.layer;
				spawnedMountablePrefab.tag = s_CharacterPlatformSpawnPosition.tag;

				// Add's the Mountable Vehicle Selection Asset to the spawned in list 
				s_MountableVehicleSelectionsSpawnedIn.Add(spawnedMountablePrefab.transform);

				// Sets the layer for every child transform 
				GameEntity.SetLayerRecursively(spawnedMountablePrefab, s_CurrentPlayerIndex);
			}


			GameObject playerCamera = Instantiate(s_PlayerCameraAssetPrefab, s_CameraHolder.position, Quaternion.identity); // The player camera 
			GameObject cinemachineCamera = Instantiate(s_CinemachineCameraAssetPrefab, s_CameraHolder.position, Quaternion.identity); // The cinemachine camera that controlls the player 

			playerCamera.transform.SetParent(s_CameraHolder); // Set the camera holder transform 
			cinemachineCamera.transform.SetParent(s_CameraHolder); // Set the camera holder transform 

			CinemachineVirtualCamera camera = cinemachineCamera.GetComponent<CinemachineVirtualCamera>(); // Get cinemachine component 
			camera.Follow = s_CharacterPlatformSpawnPosition; // Set the follow property 
			camera.LookAt = s_CharacterPlatformSpawnPosition; // Set the lookat property 

			// Reference to the current player's cursor, using FindAssetClone -> GameObject.Find("P{i}_Cursor(Clone)")
			Transform s_CurrentPlayerCursor = GameEntity.FindAssetClone(s_CurrentPlayerIndex, Asset.Cursor);

			// Reference to the cursor selection behaviour component 
			CursorSelectionBehaviour s_CurrentPlayerCursorBehaviour = s_CurrentPlayerCursor.GetComponent<CursorSelectionBehaviour>();

			// Set the cursor identity using the current player index 
			s_CurrentPlayerCursorBehaviour.SetCursorIdentity(s_CurrentPlayerIndex);

			// Begin spawning in the wizard prefabs & the selectable mountable vehicle prefabs 
			GameEvents.SetSelectableWizards?.Invoke(s_CurrentPlayerIndex, s_WizardSelectionsSpawnedIn);
			GameEvents.SetSelectableMountables?.Invoke(s_CurrentPlayerIndex, s_MountableVehicleSelectionsSpawnedIn);

			// Set the current player cursor transform's parent to the selectionUIPrefab's transform 
			s_CurrentPlayerCursor.SetParent(s_CurrentPlayerSelectionUIPrefab.transform);

			// Get the image component for the current player's cursor and disable it 
			s_CurrentPlayerCursor.gameObject.GetComponent<Image>().enabled = false;	

			// Get the player camera 
			var playerCam = playerCamera.GetComponent<Camera>();
			
			// Get the selection UI canvas component 
			var playerSelectionUICanvas = s_CurrentPlayerSelectionUIPrefab.GetComponent<Canvas>();
				
			// Set the player selection ui render mode to use the screen space 
			playerSelectionUICanvas.renderMode = RenderMode.ScreenSpaceCamera;

			// and set it's camera reference to the player camera 
			playerSelectionUICanvas.worldCamera = playerCam;

			// Scriptable Object Data that is attached to the CursorSelectionBehaviour Component 
			GameControllerUIData s_GameControllerData = s_CurrentPlayerCursorBehaviour.ControllerUI;

			if (s_GameControllerData != null)
			{
				Transform s_BackButton = GameEntity.FindSceneAsset(s_CurrentPlayerIndex, SceneAsset.SelectionUI_BackButton);
				Transform s_ReadyButton = GameEntity.FindSceneAsset(s_CurrentPlayerIndex, SceneAsset.SelectionUI_ReadyUp);

				GameObject s_BackButtonIcon = s_BackButton.Find("Icon").gameObject;
				GameObject s_ReadyUpButtonIcon = s_ReadyButton.Find("Icon").gameObject;

				
				// Set the button icon's sprite images using the GameControllerUIData Attached to the current player cursor behaviour 
				s_BackButtonIcon.GetComponent<Image>().sprite = s_GameControllerData.ButtonWest;
				s_ReadyUpButtonIcon.GetComponent<Image>().sprite = s_GameControllerData.ButtonSouth;

				// Set the back button as inactive 
				s_BackButton.gameObject.SetActive(false);
			}
			
			// Add the playerCamera to the active player camera's list 
			m_ActivePlayerCameras.Add(playerCam);

			// Add the playerSelectionUICanvas to the selection UI list 
			m_SelectionUI.Add(playerSelectionUICanvas);
		}

		// Set a local reference to the split screen mode defined in the game manager instance 
		SplitScreenMode Mode = GameManager.Instance.ScreenMode;

		// Setup the split screen camera's using the active player cameras list and the split screen mode 
		GameEntity.SetSplitScreenCameras(m_ActivePlayerCameras, Mode);
	}

	#endregion

	#region Private Methods
	/// <summary>
	///		Gets the character stats from the casted integer Wizard Type (index) provided
	/// </summary>
	/// <param name="p_Type"></param>
	/// <returns></returns>
	private CharacterStats GetCharacterStats(WizardType p_Type)
	{

		int value = (int)p_Type;

		if (characterStatsData[value] == null)
		{
			Debug.LogError("[CharacterStatsData.GetCharacterStats]: " + "Could not fetch stats for " + p_Type.ToString());
			return null;
		}
		else
		{
			return characterStatsData[value];
		}
	}

	#endregion

}
