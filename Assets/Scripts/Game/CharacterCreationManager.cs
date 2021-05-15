#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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

	#endregion


	#region Private Variables

	/// <summary>
	///		List of selectable Wizard Prefabs 
	/// </summary>
	[SerializeField] private List<GameObject> m_SelectableWizardPrefabs = new List<GameObject>();

	/// <summary>
	///		Selection index for a wizard 
	/// </summary>
	private int m_WizardSelectionIndex = 0;
	
	/// <summary>
	///		List of selectable Brooms 
	/// </summary>
	[SerializeField] private List<GameObject> m_SelectableBroomPrefabs = new List<GameObject>();

	/// <summary>
	///		Selection index for the broom selection 
	/// </summary>
	private int m_BroomSelectionIndex = 0;
	
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

	public enum View { Single, Top, TopLeft, TopRight, Bottom, BottomRight, BottomLeft };



	[SerializeField] private List<Camera> m_PlayerCameras = new List<Camera>();

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

		SplitScreen = GetScreenType(m_EnabledCameras);

		m_PlayerCameras.Clear();


		int currentIndex = 0; 
		for (int i = 0; i < cinemachinePlayerCameras.Count; i++)
		{
			currentIndex++;

			GameObject camera = cinemachinePlayerCameras[i].gameObject;

			Debug.Log("Total Enabled Player Cameras: " + m_EnabledCameras);

			// Check if the camera should be enabled 
			bool isCameraEnabled = m_EnabledCameras >= currentIndex == true;

			// Set the camera's active state 
			SetCameraActiveState(camera, isCameraEnabled);


			Camera s_Camera = camera.GetComponent<Camera>();


		
			m_PlayerCameras.Add(s_Camera);
		}

		SetCameras(SplitScreen);
	}

	#endregion

	#region Public Methods


	#endregion

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

	private SplitScreenType GetScreenType(int p_TotalPlayers)
	{
		switch (p_TotalPlayers)
		{
			case 1:
				return SplitScreenType.SinglePlayer;
			case 2:
				return SplitScreenType.TwoPlayer;
			case 3:
				return SplitScreenType.ThreePlayer;
			case 4:
				return SplitScreenType.FourPlayer;
			default:
				Debug.LogWarning("[CharacterCreationManager.GetScreenType]: " + "Could not determine the split screen type.");
				return SplitScreenType.SinglePlayer;
		}

	}

}
