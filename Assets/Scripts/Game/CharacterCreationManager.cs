#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
#endregion




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

	/// <summary>
	///		The active Player Camera's in the Scene 
	/// </summary>
	[SerializeField] private List<GameObject> m_ActivePlayerCameras = new List<GameObject>();


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


	#endregion


	#region Unity References


	private void OnEnable()
	{
		GameEvents.CoopCharacterCreationStartEvent += Setup;
	}


	private void OnDisable()
	{
		GameEvents.CoopCharacterCreationStartEvent -= Setup;
	}


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


	#endregion


	#region Public Methods


	#endregion


	#region Private Methods

	/// <summary>
	///		Sets up the camera's 
	/// </summary>
	/// <param name="p_PlayerCount"></param>
	private void Setup(int p_PlayerCount)
	{

		Debug.Log("[CharacterCreationManager.Setup]: " + "Character Creation Manager has been called! Setting up cameras for " + p_PlayerCount + " players!");

		// Clear the active player camera's list 
		m_ActivePlayerCameras.Clear();

			// Loop through the amount of cameras needed 
			for (int i = 0; i < p_PlayerCount; i++)
			{
			

				// Get the player camera game object 
				GameObject s_VirtualCamera = cinemachinePlayerCameras[i].gameObject;


		

		



				m_ActivePlayerCameras.Add(s_VirtualCamera.gameObject);

				
			}

	}


	#endregion

}
