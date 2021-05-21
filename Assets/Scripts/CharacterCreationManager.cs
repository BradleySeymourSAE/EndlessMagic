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
	///		Reference to all the cinemachine cameras in the scene 
	/// </summary>
	private List<GameObject> m_CinemachineCameras = new List<GameObject>();

	public List<Transform> platformSpawnPositions = new List<Transform>();

	#endregion

	#region Private Variables 

	private List<Camera> m_ActivePlayerCameras = new List<Camera>();

	[SerializeField] private List<GameObject> m_SpawnedPlayerSelectionPlatforms = new List<GameObject>();

	[SerializeField] private List<Canvas> m_SelectionUI = new List<Canvas>();

	[SerializeField] private List<Transform> m_WizardPrefabSpawnPoints = new List<Transform>();

	private List<Transform> m_PlatformPrefabSpawnPoints = new List<Transform>();

	[SerializeField] private List<string> m_AssetResourceQueries = new List<string>();

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
		m_AssetResourceQueries.Clear();
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

	private void OnEnable()
	{
		GameEvents.SpawnSelectableCharacters += SpawnWizardSelections;
	}

	private void OnDisable()
	{
		GameEvents.SpawnSelectableCharacters -= SpawnWizardSelections;
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



			GameObject playerCamera = Instantiate(s_PlayerCameraAssetPrefab, s_CameraHolder.position, Quaternion.identity);
			GameObject cinemachineCamera = Instantiate(s_CinemachineCameraAssetPrefab, s_CameraHolder.position, Quaternion.identity);

			playerCamera.transform.SetParent(s_CameraHolder);
			cinemachineCamera.transform.SetParent(s_CameraHolder);

			CinemachineVirtualCamera camera = cinemachineCamera.GetComponent<CinemachineVirtualCamera>();
			camera.Follow = s_WizardSpawnPoint;
			camera.LookAt = s_WizardSpawnPoint;



			Transform s_SelectionUIManager = GameEntity.FindByTag(GameTag.SelectionUIManager).transform;
			
			GameObject s_CurrentPlayerSelectionUIPrefab = Instantiate(s_SelectionUIAssetPrefab, s_SelectionUIManager.position, Quaternion.identity);
			Transform s_CurrentPlayerCursor = GameEntity.FindSceneAssetClone(s_CurrentPlayerIndex, Asset.Cursor);

			s_CurrentPlayerSelectionUIPrefab.transform.SetParent(s_SelectionUIManager);
			s_CurrentPlayerSelectionUIPrefab.layer = GameEntity.GetPlayerCameraLayer(s_CurrentPlayerIndex);


			s_CurrentPlayerCursor.SetParent(s_CurrentPlayerSelectionUIPrefab.transform);
			s_CurrentPlayerCursor.gameObject.GetComponent<Image>().enabled = false;

			

			var playerCam = playerCamera.GetComponent<Camera>();
			
			var playerSelectionUICanvas = s_CurrentPlayerSelectionUIPrefab.GetComponent<Canvas>();
				
			playerSelectionUICanvas.renderMode = RenderMode.ScreenSpaceCamera;
			playerSelectionUICanvas.worldCamera = playerCam;
				
			m_ActivePlayerCameras.Add(playerCam);
			m_SelectionUI.Add(playerSelectionUICanvas);
		}

		hasWizardSelectionSpawnPoints = m_WizardPrefabSpawnPoints.Count > 0 && m_WizardPrefabSpawnPoints.Count <= GameManager.Instance.MaxPlayers;

		SplitScreenMode Mode = GameManager.Instance.ScreenMode;
		GameEntity.SetSplitScreenCameras(m_ActivePlayerCameras, Mode);

		if (hasWizardSelectionSpawnPoints)
		GameEvents.SpawnSelectableCharacters?.Invoke(m_Players);
	}


	#region @TODO - README 

	///		Spawns the selectable Wizards in - I can't remember whether I handle that from inside here
	///		Or whether I should handle that from inside character object spawn manager... reeeeeee
	///		Am going down the road to get dinner and will sort this once I get back 
	private void SpawnWizardSelections(int p_TotalPlayers)
	{
			
		Debug.Log("Spawning the wizard selections into the game!");

	}

	#endregion
}
