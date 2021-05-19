#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


public enum SplitScreenMode { NoConnectedPlayers = 0, SinglePlayer = 1, TwoPlayer = 2, ThreePlayer = 3, FourPlayer = 4 };



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

	/// <summary>
	///		Returns the current split screen mode 
	/// </summary>
	public SplitScreenMode ScreenMode
	{
		get
		{
			return (SplitScreenMode)ConnectedPlayers;
		}
	}

	#endregion

	#region Public Variables 

	/// <summary>
	///		The Maximum Allowed Players 
	/// </summary>
	[HideInInspector] public int MaxPlayers = 4;

	/// <summary>
	///		Should players be allowed to join in? 
	/// </summary>
	public bool AllowPlayerJoining;

	/// <summary>
	///		Allow Character Selecting 
	/// </summary>
	public bool AllowCharacterSelecting = false;

	/// <summary>
	///		The amount of time to wait after all players have ready'd up 
	/// </summary>
	[Min(3f)] public float JoinStartTimer = 5f;

	#endregion

	#region Private Variables 

	[SerializeField] private List<CursorSelectionBehaviour> m_SelectionCursors = new List<CursorSelectionBehaviour>();

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
		m_SelectionCursors.Clear();


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
		AllowCharacterSelecting = false;

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

		GameEvents.SetCharacterCreationCursorEvent += SetCharacterCreationCursors;
	}

	/// <summary>
	///		Unsubscribe from events 
	/// </summary>
	private void OnDisable()
	{
		GameEvents.SetPlayerJoinedEvent -= SetConnectedPlayers;

		GameEvents.SetCharacterCreationCursorEvent -= SetCharacterCreationCursors;
	}

	private void Update()
	{
		CheckPlayerJoiningIsAllowed();

		CheckAllowCharacterCreation();
	}

	#endregion

	#region Private Methods 

	/// <summary>
	///		Sets the amount of connected players 
	/// </summary>
	/// <param name="Players"></param>
	private void SetConnectedPlayers(int Players) => ConnectedPlayers = Players;

	/// <summary>
	///		Are player's allowed to join into the game? 
	/// </summary>
	private void CheckPlayerJoiningIsAllowed() => AllowPlayerJoining = GameUIManager.Instance.IsDisplayingPlayerJoinMenu();

	private void CheckAllowCharacterCreation() => AllowCharacterSelecting = Instance.m_SelectionCursors.Count > 0 == true;


	private void SetCharacterCreationCursors(List<GameObject> cursors)
	{
		m_SelectionCursors.Clear();

		for (int i = 0; i < cursors.Count; i++)
		{
			if (!cursors[i].GetComponent<CursorSelectionBehaviour>())
			{
				return;
			}
			
				Transform s_CursorTransform = cursors[i].transform;

				s_CursorTransform.SetParent(transform);

				m_SelectionCursors.Add(cursors[i].GetComponent<CursorSelectionBehaviour>());

		}



	}

	#endregion

}
