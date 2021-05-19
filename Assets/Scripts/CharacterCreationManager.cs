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

	private List<GameObject> m_SpawnedPlayerSelectionPlatforms = new List<GameObject>();

	private List<Canvas> m_SelectionUI = new List<Canvas>();

	[SerializeField] private List<Transform> m_WizardPrefabSpawnPoints = new List<Transform>();

	private List<Transform> m_PlatformPrefabSpawnPoints = new List<Transform>();

	private List<string> m_AssetResourceQueries = new List<string>();

	private Dictionary<CameraView, Rect> m_CameraViewDictionary = new Dictionary<CameraView, Rect>
	{
		{ CameraView.Fullscreen, new Rect(0, 0, 1, 1) },
		{ CameraView.TopScreen, new Rect(0, 0.5f, 1, 1) },
		{ CameraView.BottomScreen, new Rect(0, -0.5f, 1, 1) },
		{ CameraView.UpperLeft, new Rect(-0.5f, 0.5f, 1, 1) },
		{ CameraView.UpperRight, new Rect(0.5f, 0.5f, 1, 1) },
		{ CameraView.LowerLeft, new Rect(0.5f, -0.5f, 1, 1 ) },
		{ CameraView.LowerRight, new Rect(-0.5f, -0.5f, 1, 1) }
	};

	public enum CameraView { Fullscreen, TopScreen, BottomScreen, UpperLeft, UpperRight, LowerLeft, LowerRight };


	#endregion

	#region Debugging 

	[Header("--- Debugging ---")]
	[SerializeField] private int m_Players;

	[SerializeField] private Color m_PlatformSpawnPointColor = Color.red;

	[SerializeField] private float m_GizmoSize = 0.5f;

	[SerializeField] private bool m_Debugging = true;

	[SerializeField] private bool m_UseWireSphere = true;

	#endregion


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



	#region NOTES -----

		/// <summary>
		///		Everything works literally right up until this point - I have no fucking idea what I was trying to achieve here but I will look into it tomorrow 
		///		when my eyes aren't sore as fuck 
		/// </summary>
		/// <param name="PlayerCount"></param>

	#endregion
	public void Setup(int PlayerCount)
	{
		m_ActivePlayerCameras.Clear();
		m_SpawnedPlayerSelectionPlatforms.Clear();

		Debug.Log("[CharacterCreationManager.Setup]: " + "Setting up camera's for " + PlayerCount + " players!");

		int s_CurrentPlayerIndex = 0;
		// Loop through the amount of players 
		for (int i = 0; i < PlayerCount; i++)
		{

			s_CurrentPlayerIndex++;

			Transform s_Environment = GameText.GetGameObject(GameText.EnvironmentTag).transform;
			Transform s_CameraHolder = GameText.GetGameObject(GameText.CinemachineCameraHolderTag).transform;

			string s_PlayerSelectPlatformAssetResource = GameText.FindPlayerAssetInResourceFolder(ResourceAssetFolder.WizardSelectionPlatformPrefabs, s_CurrentPlayerIndex, GameText.ResourceAsset_PlayerSelectPlatform);
			string s_PlayerCameraAssetResource = GameText.FindPlayerAssetInResourceFolder(ResourceAssetFolder.CameraPrefabs_Cinemachine_CMPlayerCameraPrefabs, s_CurrentPlayerIndex, GameText.ResourceAsset_Camera);
			string s_CinemachineCameraAssetResource = GameText.FindPlayerAssetInResourceFolder(ResourceAssetFolder.CameraPrefabs_Cinemachine_PlayerCameraPrefabs, s_CurrentPlayerIndex, GameText.ResourceAsset_CinemachineCamera);
			string s_SelectionUIAssetResource = GameText.FindPlayerAssetInResourceFolder(ResourceAssetFolder.SelectionUIPrefabs, s_CurrentPlayerIndex, GameText.SceneObject_SelectionUI);

			// PLATFORM SETUP & CHARACTER SETUP 

			GameObject platformAssetPrefab = Resources.Load<GameObject>(s_PlayerSelectPlatformAssetResource);

			GameObject platform = Instantiate(platformAssetPrefab, m_PlatformPrefabSpawnPoints[s_CurrentPlayerIndex - 1].position, platformAssetPrefab.transform.rotation);

			platform.transform.SetParent(s_Environment);

			platform.layer = GameText.GetPlayerCameraLayer(s_CurrentPlayerIndex);
			platform.tag = GameText.GetPlayerCameraTag(s_CurrentPlayerIndex);

			m_SpawnedPlayerSelectionPlatforms.Add(platform);
			
			Transform s_WizardSpawnPoint = platform.transform.Find(GameText.FindPlayerGameObjectSceneAsset(s_CurrentPlayerIndex, GameText.SceneObject_WizardSelectionSpawn));

			m_WizardPrefabSpawnPoints.Add(s_WizardSpawnPoint);

			//	 CAMERAS & SCREEN SETUP 
			GameObject playerCamera = Instantiate(Resources.Load<GameObject>(s_PlayerCameraAssetResource), s_CameraHolder.position, Quaternion.identity);
			GameObject cinemachineCamera = Instantiate(Resources.Load<GameObject>(s_CinemachineCameraAssetResource), s_CameraHolder.position, Quaternion.identity);

			if (cinemachineCamera.GetComponent<CinemachineVirtualCamera>() != null)
			{
				CinemachineVirtualCamera camera = cinemachineCamera.GetComponent<CinemachineVirtualCamera>();

				camera.Follow = s_WizardSpawnPoint;
				camera.LookAt = s_WizardSpawnPoint;
			}

			Transform selectionUI = GameText.GetGameObject(GameText.SelectionUIManagerTag).transform;

			GameObject playerSelectionUI = Instantiate(Resources.Load<GameObject>(s_SelectionUIAssetResource), selectionUI.position, Quaternion.identity);

			string s_Cursor = $"P{s_CurrentPlayerIndex}_Cursor(Clone)";

			Transform currentPlayerCursor = GameObject.Find(s_Cursor).transform;

			playerSelectionUI.transform.SetParent(selectionUI);
			playerSelectionUI.layer = GameText.GetPlayerCameraLayer(s_CurrentPlayerIndex);

			currentPlayerCursor.SetParent(playerSelectionUI.transform);
			
			currentPlayerCursor.gameObject.GetComponent<Image>().enabled = false;
			

			playerCamera.transform.SetParent(s_CameraHolder);
			cinemachineCamera.transform.SetParent(s_CameraHolder);

			var s_PlayerCamera = playerCamera.GetComponent<Camera>();
			var uiSelectionCanvas = playerSelectionUI.GetComponent<Canvas>();

			if (s_PlayerCamera)
			{
				m_ActivePlayerCameras.Add(s_PlayerCamera);
			}
			
			if (uiSelectionCanvas)
			{
				uiSelectionCanvas.renderMode = RenderMode.ScreenSpaceCamera;
				uiSelectionCanvas.worldCamera = s_PlayerCamera;


				m_SelectionUI.Add(uiSelectionCanvas);
			}


			if (m_Debugging && s_CurrentPlayerIndex <= 1)
			{ 
				Debug.LogWarning("[CharacterCreationManager.Setup]: " + "Debugging! - Showing Asset Resource Queries");
					m_AssetResourceQueries.Add(s_PlayerSelectPlatformAssetResource);
					m_AssetResourceQueries.Add(s_PlayerCameraAssetResource);
					m_AssetResourceQueries.Add(s_CinemachineCameraAssetResource);
					m_AssetResourceQueries.Add(s_SelectionUIAssetResource);
			}
		}


		SplitScreenMode Mode = GameManager.Instance.ScreenMode;
		SetSplitScreenCameras(Mode);
	}


	private void SetSplitScreenCameras(SplitScreenMode Mode)
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
							m_ActivePlayerCameras[0].rect = element.Value;
						}
					}
					break;
				case SplitScreenMode.TwoPlayer:
					{
						if (element.Key == CameraView.TopScreen)
						{
							m_ActivePlayerCameras[0].rect = element.Value;
						}
						else if (element.Key == CameraView.BottomScreen)
						{
							m_ActivePlayerCameras[1].rect = element.Value;
						}
					}
					break;
				case SplitScreenMode.ThreePlayer:
					{
						if (element.Key == CameraView.TopScreen)
						{
							m_ActivePlayerCameras[0].rect = element.Value;
						}
						else if (element.Key == CameraView.LowerLeft)
						{
							m_ActivePlayerCameras[1].rect = element.Value;
						}
						else if (element.Key == CameraView.LowerRight)
						{
							m_ActivePlayerCameras[2].rect = element.Value;
						}
					}
					break;
				case SplitScreenMode.FourPlayer:
					{
						if (element.Key == CameraView.UpperLeft)
						{
							m_ActivePlayerCameras[0].rect = element.Value;
						}
						else if (element.Key == CameraView.UpperRight)
						{
							m_ActivePlayerCameras[1].rect = element.Value;
						}
						else if (element.Key == CameraView.LowerLeft)
						{
							m_ActivePlayerCameras[2].rect = element.Value;
						}
						else if (element.Key == CameraView.LowerRight)
						{
							m_ActivePlayerCameras[3].rect = element.Value;
						}
					}
					break;
			}

		}
	}
}
