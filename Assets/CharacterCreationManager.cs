#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion



public class CharacterCreationManager : MonoBehaviour
{ 


	public List<Transform> playerSelectPlatformSpawnPoints = new List<Transform>();

	public GameObject[] playerSelectPlatformPrefabs;
	public GameObject[] wizardPrefabs;
	
	[SerializeField] private List<Transform> m_WizardSpawnPoints = new List<Transform>();

	[Header("Debugging")]
	[SerializeField] private List<Transform> m_SpawnedWizards = new List<Transform>();
	[SerializeField] private List<Transform> m_PlayerSelectionCameras = new List<Transform>();

	[SerializeField] private int m_Players;

	[SerializeField] private Color m_PlatformSpawnPoint = Color.red;

	[SerializeField] private float m_GizmoSize = 0.5f;
	
	[SerializeField] private bool m_Debugging = true;

	[SerializeField] private bool m_UseWireSphere = true;

	private GamepadJoinGameBehaviour behaviour;


	private void OnDrawGizmos()
	{
		if (m_Debugging)
		{ 


			if (playerSelectPlatformSpawnPoints.Count > 0)
			{
				foreach (Transform platform in playerSelectPlatformSpawnPoints)
				{
					Gizmos.color = m_PlatformSpawnPoint;
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
		
		if (FindObjectOfType<GamepadJoinGameBehaviour>() != null)
		{
			behaviour = FindObjectOfType<GamepadJoinGameBehaviour>();

			m_Players = behaviour.numberOfActivePlayers;
		}



		m_Players = GameManager.Instance.ConnectedPlayers;


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
		m_SpawnedWizards.Clear();
		m_WizardSpawnPoints.Clear();

		if (PlayerCount > 0)
		{

			for (int s_CurrentPlayerIndex = 0; s_CurrentPlayerIndex < PlayerCount; s_CurrentPlayerIndex++)
			{

				GameObject wizardPlatform = Resources.Load<GameObject>(GameText.ReturnAssetResourceKey(s_CurrentPlayerIndex, GameAssetResource.WizardSelectionPlatformPrefabs, GameText.GameObjectResource_PlayerSelectionPlatform, true));
				
				

				
				string platformWizardSpawnPoint = GameText.ReturnAssetResourceKey(s_CurrentPlayerIndex, GameAssetResource.None, GameText.GameObjectResource_WizardSelectionSpawn, false);

				Transform spawnPoint = wizardPlatform.transform.Find(platformWizardSpawnPoint).transform;
				
				m_WizardSpawnPoints.Add(spawnPoint);

				foreach (GameObject wizard in wizardPrefabs)
				{
					
					GameObject s_SpawnedWizard = Instantiate(wizard, spawnPoint.position, Quaternion.identity);
					
					m_SpawnedWizards.Add(s_SpawnedWizard.transform);
				}


			}
		}

	}
}
