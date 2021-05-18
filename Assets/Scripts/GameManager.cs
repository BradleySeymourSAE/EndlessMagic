#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion



/// <summary>
/// Game Manager
/// </summary>
[System.Serializable]
public class GameManager : MonoBehaviour
{ 


	public static GameManager Instance;

	public int ConnectedDevices { get; private set; } = 0;
	public int ConnectedPlayers { get; private set; } = 0;


	[HideInInspector] public int maximumAllowedPlayers = 4;

	[SerializeField] public bool AllowPlayerJoining;

	GameUIManager m_GameUIManager;

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

		AllowPlayerJoining = false;

		if (GameUIManager.Instance)
		{
			m_GameUIManager = GameUIManager.Instance;
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

	private void Update()
	{
		CheckPlayerJoiningIsAllowed();
	}

	/// <summary>
	///		Sets the amount of connected players 
	/// </summary>
	/// <param name="Players"></param>
	private void SetConnectedPlayers(int Players) => ConnectedPlayers += Players;

	/// <summary>
	///		Are player's allowed to join into the game? 
	/// </summary>
	private void CheckPlayerJoiningIsAllowed() => AllowPlayerJoining = GameUIManager.Instance.IsDisplayingPlayerJoinMenu();
}
