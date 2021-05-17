#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion





public class GameManager : MonoBehaviour
{ 


	public static GameManager Instance;

	[SerializeField] public int ConnectedDevices { get; private set; } = 0;
	
	[SerializeField] public int ConnectedPlayers { 
		get { return m_ConnectedPlayerIndex; } 
		private set {	m_ConnectedPlayerIndex = value; } }

	[Header("Debugging")]
	[SerializeField] private int m_ConnectedPlayerIndex;
	[HideInInspector] public int maximumAllowedPlayers = 4;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}


	private void OnEnable()
	{
		GameEvents.SetPlayerJoinedEvent += SetConnectedPlayers;
	}

	private void OnDisable()
	{
		GameEvents.SetPlayerJoinedEvent -= SetConnectedPlayers;
	}



	private void SetConnectedPlayers(int Players) => ConnectedPlayers = Players;
}
