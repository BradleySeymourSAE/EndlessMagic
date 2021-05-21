#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
#endregion



public class CharacterCreationManager : MonoBehaviour
{


	#region Public Variables 

	/// <summary>
	///		List of starting platform spawn positions 
	/// </summary>
	public List<Transform> platformSpawnPositions = new List<Transform>();

	#endregion

	#region Private Variables
	/// <summary>
	///		List of active player cameras 
	/// </summary>
	[Header("Debugging")]
	[SerializeField] private List<Camera> m_ActivePlayerCameras = new List<Camera>();

	/// <summary>
	///		List of spawned player selection platforms 
	/// </summary>
	[SerializeField] private List<GameObject> m_SpawnedPlayerSelectionPlatforms = new List<GameObject>();

	/// <summary>
	///		List of selection UI 
	/// </summary>
	[SerializeField] private List<Canvas> m_SelectionUI = new List<Canvas>();

	/// <summary>
	///		List of the Wizard Prefab Spawn Positions 
	/// </summary>
	[SerializeField] private List<Transform> m_WizardPrefabSpawnPoints = new List<Transform>();

	/// <summary>
	///		List of all the platform prefab spawn points after they are set 
	/// </summary>
	[SerializeField] private List<Transform> m_PlatformPrefabSpawnPoints = new List<Transform>();

	/// <summary>
	///		Reference to all the cinemachine cameras in the scene 
	/// </summary>
	[SerializeField] private List<GameObject> m_CinemachineCameras = new List<GameObject>();

	[SerializeField] private List<GameObject> m_SpawnedWizardPrefabs = new List<GameObject>();
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
				foreach (Transform wizardSpawnPoint in m_WizardPrefabSpawnPoints)
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
		m_CinemachineCameras.Clear();
		m_SpawnedWizardPrefabs.Clear();
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

	/// <summary>
	///		Sets up the Character Creation Scene - Based on the amount of players that have joined the game 
	/// </summary>
	/// <param name="PlayerCount"></param>
	public void Setup(int PlayerCount)
	{
		m_ActivePlayerCameras.Clear();
		m_SpawnedPlayerSelectionPlatforms.Clear();

		Debug.Log("[CharacterCreationManager.Setup]: " + "Setting up camera & Game object references for " + PlayerCount + " players!");

		int s_CurrentPlayerIndex = 0;
		// Loop through the amount of players 
		for (int i = 0; i < PlayerCount; i++)
		{

			s_CurrentPlayerIndex++;

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
			
			Transform s_WizardSpawnPoint = GameEntity.FindGameObjectChildTransform(platform, s_CurrentPlayerIndex, SceneAsset.WizardSelectionSpawn);
			
			if (s_WizardSpawnPoint != null)
			{ 
				m_WizardPrefabSpawnPoints.Add(s_WizardSpawnPoint);
			}

			List<GameObject> playerWizardSelectionsSpawnedIn = new List<GameObject>();
			playerWizardSelectionsSpawnedIn.Clear();

			GameObject[] s_AvailableWizardSelections = GameEntity.FindAllIndexedAssets(ResourceFolder.WizardPrefabs);

			for (int j = 0; j < s_AvailableWizardSelections.Length; j++)
			{
				GameObject wizardPrefabAsset = Instantiate(s_AvailableWizardSelections[j], s_WizardSpawnPoint.position, s_AvailableWizardSelections[j].transform.rotation);


				wizardPrefabAsset.transform.SetParent(s_WizardSpawnPoint);
				wizardPrefabAsset.layer = s_WizardSpawnPoint.gameObject.layer;
				wizardPrefabAsset.tag = s_WizardSpawnPoint.tag;

				if (j == 0)
				{
					wizardPrefabAsset.SetActive(true);
				}
				else
				{
					wizardPrefabAsset.SetActive(false);
				}	
			}
			
			// Obviously set the first wizard selection game object to true 
			s_AvailableWizardSelections[0].SetActive(true);


			GameObject playerCamera = Instantiate(s_PlayerCameraAssetPrefab, s_CameraHolder.position, Quaternion.identity); // The player camera 
			GameObject cinemachineCamera = Instantiate(s_CinemachineCameraAssetPrefab, s_CameraHolder.position, Quaternion.identity); // The cinemachine camera that controlls the player 

			playerCamera.transform.SetParent(s_CameraHolder); // Set the camera holder transform 
			cinemachineCamera.transform.SetParent(s_CameraHolder); // Set the camera holder transform 

			CinemachineVirtualCamera camera = cinemachineCamera.GetComponent<CinemachineVirtualCamera>(); // Get cinemachine component 
			camera.Follow = s_WizardSpawnPoint; // Set the follow property 
			camera.LookAt = s_WizardSpawnPoint; // Set the lookat property 



			Transform s_SelectionUIManager = GameEntity.FindByTag(GameTag.SelectionUIManager).transform;
			
			GameObject s_CurrentPlayerSelectionUIPrefab = Instantiate(s_SelectionUIAssetPrefab, s_SelectionUIManager.position, Quaternion.identity);
			Transform s_CurrentPlayerCursor = GameEntity.FindSceneAssetClone(s_CurrentPlayerIndex, Asset.Cursor);

			s_CurrentPlayerCursor.GetComponent<CursorSelectionBehaviour>().wizardSelectionChoices = s_AvailableWizardSelections;
		
			s_CurrentPlayerSelectionUIPrefab.transform.SetParent(s_SelectionUIManager);
			s_CurrentPlayerSelectionUIPrefab.layer = GameEntity.GetPlayerCameraLayer(s_CurrentPlayerIndex);

			// The current player's cursor 
			s_CurrentPlayerCursor.SetParent(s_CurrentPlayerSelectionUIPrefab.transform);
			s_CurrentPlayerCursor.gameObject.GetComponent<Image>().enabled = false;	

			var playerCam = playerCamera.GetComponent<Camera>();
			
			var playerSelectionUICanvas = s_CurrentPlayerSelectionUIPrefab.GetComponent<Canvas>();
				
			playerSelectionUICanvas.renderMode = RenderMode.ScreenSpaceCamera;
			playerSelectionUICanvas.worldCamera = playerCam;
				
			m_ActivePlayerCameras.Add(playerCam);
			m_SelectionUI.Add(playerSelectionUICanvas);
		}

		SplitScreenMode Mode = GameManager.Instance.ScreenMode;
		GameEntity.SetSplitScreenCameras(m_ActivePlayerCameras, Mode);
		GameEvents.SetAllowCharacterSelectionEvent?.Invoke(true);
	}


}
