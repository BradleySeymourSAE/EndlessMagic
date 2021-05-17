#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#endregion

public class CharacterObjectSpawnManager : MonoBehaviour
{

	/// <summary>
	///		To preserve the players selections and the controllers they have used, use dictionaries to keep track of both 
	///		and destroy the cursor and selected objects, then reinstantiate 
	/// </summary>
	public static Dictionary<int, InputDevice> Controllers = new Dictionary<int, InputDevice>();


	/// <summary>
	///		List of names of the game objects we want to instantiate as the player when progressing to the next 
	///		scene, in this case, treat the first object as the one with the player input, and just instantiate the rest 
	///		parenting, placing etc - That can be performed after once the scene has been loaded.
	/// </summary>
	public static Dictionary<int, List<string>> PlayerNames = new Dictionary<int, List<string>>();
	public static Dictionary<int, string> ControlActionSchemes = new Dictionary<int, string>();

	[SerializeField] public static bool m_ShouldSpawnSelectedPlayers = false;
	[SerializeField] public static bool m_ShouldPersistCursorObjects = false;


	#region Unity References 

	private void OnEnable()
	{
		CursorSelectionBehaviour.HandleOnCharacterSelectedEvent += PlayerSelectedCharacter;
	}

	private void OnDisable()
	{
		CursorSelectionBehaviour.HandleOnCharacterSelectedEvent -= PlayerSelectedCharacter;
	}


	#endregion

	private void PlayerSelectedCharacter(object sender, EventArgs e)
	{

		GameObject[] currentPlayerCursors = GameObject.FindGameObjectsWithTag("PlayerCursor");


		foreach (var cursor in currentPlayerCursors)
		{
			if (!cursor.GetComponent<CursorSelectionBehaviour>().objectSelected)
			{
				Debug.Log(cursor + " object has not selected a player!");
				return;
			}
		}


		for (int i = 0; i < currentPlayerCursors.Length; i++)
		{
			var playerInput = currentPlayerCursors[i].GetComponent<PlayerInput>();
			var playerSelection = currentPlayerCursors[i].GetComponent<CursorSelectionBehaviour>().playerSelection.name;

			var playerIndex = playerInput.playerIndex;

			Controllers.Add(playerIndex, playerInput.devices[0]);

			if (!PlayerNames.ContainsKey(playerIndex))
			{
				PlayerNames.Add(playerIndex, new List<string>() {  playerSelection });
			}
			else
			{
				var currentCharacterList = PlayerNames[playerIndex];

				currentCharacterList.Add(playerSelection);
			}


			ControlActionSchemes.Add(playerInput.playerIndex, playerInput.currentControlScheme);
		}


	

		// Load the scene 
		SceneManager.LoadSceneAsync(GameScenes.EndlessMagic_GameLevel_01);

		Debug.Log("Enable player spawning in 3 seconds!");
		Invoke(nameof(EnablePlayerSpawning), 3f);
	}


	private void EnablePlayerSpawning()
	{
		Debug.Log("Enabling player spawning!");
		// Set this if done selecting players, then run the next scene - Where they will be spawned in 
		m_ShouldSpawnSelectedPlayers = true;

		// Set this if going through multiple selection screens - in this case we dont want to spawn the cursors 
		m_ShouldPersistCursorObjects = false;

	}
}