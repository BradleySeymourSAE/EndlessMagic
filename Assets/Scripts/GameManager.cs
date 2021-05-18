#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Game Manager - Handles Game Logic 
/// </summary>
[System.Serializable]
public class GameManager : MonoBehaviour
{

	#region Static 
	
	/// <summary>
	///		Reference to the Game Manager Instance 
	/// </summary>
	public static GameManager Instance;

	#endregion

	#region Public Getter / Setter's  

	/// <summary>
	///		The total amount of Connected Devices 
	/// </summary>
	public int ConnectedDevices { get; private set; } = 0;

	/// <summary>
	///		The total amount of Connected Players 
	/// </summary>
	public int ConnectedPlayers { get; private set; } = 0;

	#endregion

	#region Public Variables 

	/// <summary>
	///		The Maximum Allowed Players 
	/// </summary>
	[HideInInspector] public int MaxPlayers = 4;

	/// <summary>
	///		Should players be allowed to join in? 
	/// </summary>
	[SerializeField] public bool AllowPlayerJoining;

	/// <summary>
	///		The amount of time to wait after all players have ready'd up 
	/// </summary>
	[Min(3f)] public float JoinStartTimer = 5f;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to the Game UI Manager Instance 
	/// </summary>
	GameUIManager m_GameUIManager;

	#endregion

	#region Unity References 

	/// <summary>
	///		Create the Game Manager Instance 
	/// </summary>
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

	/// <summary>
	///		Subscribe to events 
	/// </summary>
	private void OnEnable()
	{
		GameEvents.SetPlayerJoinedEvent += SetConnectedPlayers;
	}

	/// <summary>
	///		Unsubscribe from events 
	/// </summary>
	private void OnDisable()
	{
		GameEvents.SetPlayerJoinedEvent -= SetConnectedPlayers;
	}

	private void Update()
	{
		CheckPlayerJoiningIsAllowed();
	}

	#endregion

	#region Private Methods 

	/// <summary>
	///		Sets the amount of connected players 
	/// </summary>
	/// <param name="Players"></param>
	private void SetConnectedPlayers(int Players) => ConnectedPlayers += Players;

	/// <summary>
	///		Are player's allowed to join into the game? 
	/// </summary>
	private void CheckPlayerJoiningIsAllowed() => AllowPlayerJoining = GameUIManager.Instance.IsDisplayingPlayerJoinMenu();

	#endregion

}
