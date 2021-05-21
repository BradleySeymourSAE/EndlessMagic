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

	#region @TODO - #future-brad Will eventually stop hardcoding this and use Resources.Load<GameObject>() to get the amount of items in the folder 
	/// <summary>
	///		Reference to the total amount of wizard prefabs
	/// </summary>
	public int TotalWizards = 5;

	#endregion


	/// <summary>
	///		Should players be allowed to join in? 
	/// </summary>
	public bool AllowPlayerJoining { get { return m_AllowPlayerJoining; } set { m_AllowPlayerJoining = value; } }

	/// <summary>
	///		Allow Character Selecting 
	/// </summary>
	public bool AllowCharacterSelection { get {  return m_AllowCharacterSelecting; } set { m_AllowCharacterSelecting = value; } }

	/// <summary>
	///		The amount of time to wait after all players have ready'd up 
	/// </summary>
	[Min(3f)] public float JoinStartTimer = 5f;

	#endregion

	#region Private Variables 

	[SerializeField] private List<CursorSelectionBehaviour> m_SelectionCursors = new List<CursorSelectionBehaviour>();

	[SerializeField] private bool m_AllowPlayerJoining;

	[SerializeField] private bool m_AllowCharacterSelecting;

	/// <summary>
	///		Reference to the Game UI Manager Instance 
	/// </summary>
	private GameUIManager m_GameUIManager;

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
		GameEvents.SetAllowCharacterSelectionEvent += HandleAllowCharacterSelection;
	}

	/// <summary>
	///		Unsubscribe from events 
	/// </summary>
	private void OnDisable()
	{
		GameEvents.SetPlayerJoinedEvent -= SetConnectedPlayers;

		GameEvents.SetCharacterCreationCursorEvent -= SetCharacterCreationCursors;
		GameEvents.SetAllowCharacterSelectionEvent -= HandleAllowCharacterSelection;
	}

	private void Update()
	{
		CheckPlayerJoiningIsAllowed();

		CheckAllowCharacterSelecting();
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
	private void CheckPlayerJoiningIsAllowed()
	{
		if (GameUIManager.Instance)
		{
			AllowPlayerJoining = GameUIManager.Instance.IsDisplayingPlayerJoinMenu() == true;
		}
	}

	private bool CheckAllowCharacterSelecting() => AllowCharacterSelection == true;


	private void HandleAllowCharacterSelection(bool ShouldAllowCharacterSelection) => m_AllowCharacterSelecting = ShouldAllowCharacterSelection;

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
