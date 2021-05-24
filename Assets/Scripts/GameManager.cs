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

	/// <summary>
	///		Should players be allowed to join in? 
	/// </summary>
	public bool AllowPlayerJoining { get { return m_AllowPlayerJoining; } set { m_AllowPlayerJoining = value; } }

	/// <summary>
	///		Allow Character Selecting 
	/// </summary>
	public bool AllowCharacterSelection { get { return m_AllowCharacterSelecting; } set { m_AllowCharacterSelecting = value; } }

	/// <summary>
	///		Allow Debug Messages 
	/// </summary>
	public bool Debugging { get { return m_Debugging; } private set { m_Debugging = value; } }


	/// <summary>
	///		Returns the current split screen mode 
	/// </summary>
	[HideInInspector] public SplitScreenMode ScreenMode
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
	///		The amount of time to wait after all players have ready'd up 
	/// </summary>
	[Min(3f)] public float JoinStartTimer = 5f;

	#endregion

	#region Private Variables 

	[SerializeField] private List<CursorSelectionBehaviour> m_SelectionCursors = new List<CursorSelectionBehaviour>();

	/// <summary>
	///		Begin debugging - Toggles between debug states 
	/// </summary>
	[SerializeField] private bool startDebugging = false;

	/// <summary>
	///		Reference to the Game UI Manager Instance 
	/// </summary>
	private GameUIManager m_GameUIManager;

	[Header("--- DEBUGGING ---")]
	/// <summary>
	///		If debugging - Will log debug messages to the console 
	/// </summary>
	[SerializeField] private bool m_Debugging = true;

	/// <summary>
	///		Are controllers allowed to join the game 
	/// </summary>
	[SerializeField] private bool m_AllowPlayerJoining;

	/// <summary>
	///		Are controllers input enabled to allow for character selection 
	/// </summary>
	[SerializeField] private bool m_AllowCharacterSelecting;

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
		AllowCharacterSelection = false;

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

		CheckAllowCharacterSelecting();

		if (startDebugging)
		{
			ToggleDebug();
			startDebugging = false;
		}
	}

	#endregion

	#region Private Methods 

	/// <summary>
	///		Resets the startDebugging param after x seconds 
	/// </summary>
	private void Reset()
	{
		startDebugging = false;
	}

	/// <summary>
	///		Sets the amount of connected players 
	/// </summary>
	/// <param name="Players"></param>
	private void SetConnectedPlayers(int Players) => ConnectedPlayers = Players;

	/// <summary>
	///		Are player's allowed to join into the game? 
	/// </summary>
	private void CheckPlayerJoiningIsAllowed()
	{
		if (GameUIManager.Instance)
		{
			AllowPlayerJoining = GameUIManager.Instance.IsDisplayingPlayerJoinMenu() == true;
		}
	}

	/// <summary>
	///		Check if the player is allowed to start selecting their character 
	/// </summary>
	/// <returns></returns>
	private bool CheckAllowCharacterSelecting() => AllowCharacterSelection == true;
	
	/// <summary>
	///		Toggles debug messages 
	/// </summary>
	private void ToggleDebug()
	{
		m_Debugging = !m_Debugging;
		
		if (m_Debugging)
		{
			Debug.LogWarning("[GameManager.ToggleDebug]: " + " --- Debugging has been turned on! --- ");
		}
		else
		{
			Debug.LogWarning("[GameManager.ToggleDebug]: " + " --- Debugging has been turned off! --- ");
		}
	}

	/// <summary>
	///		Sets the character creation cursors after the controller inputs have been received 
	/// </summary>
	/// <param name="cursors"></param>
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
