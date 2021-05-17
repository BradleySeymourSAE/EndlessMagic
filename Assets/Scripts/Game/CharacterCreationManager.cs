#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
#endregion




public enum SplitScreenType { SinglePlayer = 1, TwoPlayer = 2, ThreePlayer = 3, FourPlayer = 4 };

/// <summary>
///		Character Creation Manager - Handles Creating & Selecting Characters from within
///		the character creation selection screen 
/// </summary>
public class CharacterCreationManager : MonoBehaviour
{


	#region Public Variables

	/// <summary>
	///		Reference to the Cinemachine Camera GameObject's in the scene 
	/// </summary>
	public List<Transform> cinemachinePlayerCameras = new List<Transform>();


	public SplitScreenType SplitScreen = SplitScreenType.SinglePlayer;

	/// <summary>
	///		Player prefab game object 
	/// </summary>
	public GameObject playerPrefab;

	#endregion


	#region Private Variables

	/// <summary>
	///		List of selectable Wizard Prefabs 
	/// </summary>
	[SerializeField] private List<GameObject> m_SelectableWizardPrefabs = new List<GameObject>();

	/// <summary>
	///		List of prefab spawn points 
	/// </summary>
	[SerializeField] private List<Transform> wizardPrefabSpawnPoints = new List<Transform>();


	[Header("--- Debugging ---")]
	/// <summary>
	///		Are we currently debugging? - Enables the gizmo debug tool 
	/// </summary>
	[SerializeField] private bool m_Debugging = true;

	/// <summary>
	///		Use Wire Mesh Sphere? 
	/// </summary>
	[SerializeField] private bool m_UseWireMeshSphere = true;

	/// <summary>
	///		The size of the gizmo sphere 
	/// </summary>
	[Range(0f, 3f)] [SerializeField] private float m_GizmoSphereSize = 0.25f;

	/// <summary>
	///		Color of the Gizmo Sphere 
	/// </summary>
	[SerializeField] private Color m_CameraGizmoColor = Color.red;

	/// <summary>
	///		Color of the Camera's Aim Position Target
	/// </summary>
	[SerializeField] private Color m_CameraAimPositionColor = Color.green;

	/// <summary>
	///		Camera View List of Views
	/// </summary>
	public Dictionary<Rect, View> cameraViewrectDictionary = new Dictionary<Rect, View>
	{
		{ new Rect(0, 0, 1, 1), View.Single },
		{ new Rect(0, 0.5f, 1, 1), View.Top },
		{ new Rect(0, -0.5f, 1, 1), View.Bottom },
		{ new Rect(-0.5f, 0.5f, 1, 1), View.TopLeft },
		{ new Rect(0.5f, 0.5f, 1, 1), View.TopRight },
		{ new Rect(0.5f, -0.5f, 1, 1), View.BottomRight },
		{ new Rect(-0.5f, -0.5f, 1, 1), View.BottomLeft }
	};

	/// <summary>
	///		Screen View Types 
	/// </summary>
	public enum View { Single, Top, TopLeft, TopRight, Bottom, BottomRight, BottomLeft };


	/// <summary>
	///		List of player cameras in the scene 
	/// </summary>
	[SerializeField] private List<Camera> m_PlayerCameras = new List<Camera>();

	/// <summary>
	///		The amount of enabled cameras in the scene 
	/// </summary>
	private int m_EnabledCameras;
	#endregion


	#region Unity References


	/// <summary>
	///		Debugging Positions 
	/// </summary>
	private void OnDrawGizmos()
	{
		if (m_Debugging)
		{
			Gizmos.color = m_CameraGizmoColor;
			
			if (cinemachinePlayerCameras.Count > 0)
			{
				for (int i = 0; i < cinemachinePlayerCameras.Count; i++)
				{
					if (m_UseWireMeshSphere)
					{
						Gizmos.DrawWireSphere(cinemachinePlayerCameras[i].position, m_GizmoSphereSize);
					}
					else
					{
						Gizmos.DrawSphere(cinemachinePlayerCameras[i].position, m_GizmoSphereSize);
					}
				}
			}

			if (wizardPrefabSpawnPoints.Count > 0)
			{
				Gizmos.color = m_CameraAimPositionColor;
				for (int i = 0; i < wizardPrefabSpawnPoints.Count; i++)
				{
					if (m_UseWireMeshSphere)
					{
						Gizmos.DrawWireSphere(wizardPrefabSpawnPoints[i].position, m_GizmoSphereSize);
					}
					else
					{
						Gizmos.DrawSphere(wizardPrefabSpawnPoints[i].position, m_GizmoSphereSize);
					}
				}
			}
		}
	}


	private void Awake()
	{
		if (GameManager.Instance)
		{
			m_EnabledCameras = GameManager.Instance.GetConnectedPlayersIndex;
		}

		Debug.Log("[CharacterCreationManager.Awake]: " + "Total Enabled Player Cameras: " + m_EnabledCameras);

		SplitScreen = GetScreenType(m_EnabledCameras);

		m_PlayerCameras.Clear();

		int currentIndex = 0; 
		for (int i = 0; i < cinemachinePlayerCameras.Count; i++)
		{
			currentIndex++;

			GameObject camera = cinemachinePlayerCameras[i].gameObject;

			// Check if the camera should be enabled 
			bool isCameraEnabled = m_EnabledCameras >= currentIndex == true;

			// Set the camera's active state 
			SetCameraActiveState(camera, isCameraEnabled);

			// Get the camer component 
			if (camera.GetComponent<Camera>())
			{ 	
				// Get the camera component 
				Camera s_Camera = camera.GetComponent<Camera>();

				// Add the camera to the list of player cameras 
				m_PlayerCameras.Add(s_Camera);
			}
		}

		// Set the camera's up for split screen 
		SetCameras(SplitScreen);

		// Spawn in the character prefabs for the amount of enabled characters 
		SetupPlayerCharacters(m_EnabledCameras);
	}

	#endregion

	#region Public Methods

	/// <summary>
	///		Sets up the player characters with the amount of players have joined 
	/// </summary>
	/// <param name="amount"></param>
	public void SetupPlayerCharacters(int amount)
	{
		// Loop through the amount of players 
		for (int i = 0; i < amount; i++)
		{
			// Get the wizard spawn point 
			Transform s_WizardSpawnPoint = wizardPrefabSpawnPoints[i];


			// Loop through the wizard prefabs 
			foreach (GameObject wizard in m_SelectableWizardPrefabs)
			{ 
				// Instantiate a new wizard option 
				GameObject s_WizardOption = Instantiate(wizard, s_WizardSpawnPoint.position, Quaternion.identity);

				// Set the transform's parent and layer so other camera's cant see it 
				s_WizardOption.transform.SetParent(s_WizardSpawnPoint);
				s_WizardOption.layer = s_WizardSpawnPoint.gameObject.layer;
			}



			WizardSelection s_WizardSelection = s_WizardSpawnPoint.gameObject.AddComponent<WizardSelection>();

			// Initialize the wizard selection component with the wizard spawn point's child count 
			s_WizardSelection.Setup(s_WizardSpawnPoint.childCount);
		}
	}

	#endregion

	/// <summary>
	///		Sets the camera's positions based of the split screen type required 
	/// </summary>
	/// <param name="SplitScreen"></param>
	private void SetCameras(SplitScreenType SplitScreen)
	{
			switch (SplitScreen)
			{
				case SplitScreenType.SinglePlayer:
					{
						foreach (KeyValuePair<Rect, View> element in cameraViewrectDictionary)
						{
							if (element.Value == View.Single)
							{
								m_PlayerCameras[0].rect = element.Key;
							}
						}
					}
					break;
				case SplitScreenType.TwoPlayer:
					{
						foreach (KeyValuePair<Rect, View> element in cameraViewrectDictionary)
						{
							if (element.Value == View.Top)
							{
								m_PlayerCameras[0].rect = element.Key;
							}
							else if (element.Value == View.Bottom)
							{
								m_PlayerCameras[1].rect = element.Key;
							}
						}
					}
					break;
				case SplitScreenType.ThreePlayer:
					{
						foreach (KeyValuePair<Rect, View> element in cameraViewrectDictionary)
						{
							if (element.Value == View.Top)
							{
								m_PlayerCameras[0].rect = element.Key;
							}
							else if (element.Value == View.BottomLeft)
							{
								m_PlayerCameras[1].rect = element.Key;
							}
							else if (element.Value == View.BottomRight)
							{
								m_PlayerCameras[2].rect = element.Key;
							}	
						}
					}
					break;
				case SplitScreenType.FourPlayer:
					{
						foreach (KeyValuePair<Rect, View> element in cameraViewrectDictionary)
						{
							if (element.Value == View.TopLeft)
							{
								m_PlayerCameras[0].rect = element.Key;
							}
							else if (element.Value == View.TopRight)
							{
								m_PlayerCameras[1].rect = element.Key;
							}	
							else if (element.Value == View.BottomLeft)
							{
								m_PlayerCameras[2].rect = element.Key;
							}
							else if (element.Value == View.BottomRight)
							{
								m_PlayerCameras[3].rect = element.Key;
							}
						}
					}
					break;
			}
	}

	/// <summary>
	///		Toggles setting the camera's active state 
	/// </summary>
	/// <param name="p_Camera"></param>
	/// <param name="ShouldCameraBeEnabled"></param>
	private void SetCameraActiveState(GameObject p_Camera, bool ShouldCameraBeEnabled) => p_Camera.SetActive(ShouldCameraBeEnabled);

	/// <summary>
	///		Returns the current screen type 
	/// </summary>
	/// <param name="p_TotalPlayers"></param>
	/// <returns></returns>
	private SplitScreenType GetScreenType(int p_TotalPlayers)
	{
		if (p_TotalPlayers > 0 && p_TotalPlayers <= 4)
		{
			return (SplitScreenType)p_TotalPlayers;
		}
		else
		{
			return SplitScreenType.SinglePlayer;
		}

		return (SplitScreenType)p_TotalPlayers;
	}

}
