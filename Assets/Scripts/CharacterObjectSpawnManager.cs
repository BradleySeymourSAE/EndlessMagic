#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#endregion



/// <summary>
///		@TODO - At the moment this script is not doing anything and can likely be removed in the future
///		This would generally be a good way to go about spawning the players. However I couldn't really get it to work 
/// </summary>
public class CharacterObjectSpawnManager : MonoBehaviour
{
	/*
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


	private void PlayerSelectedCharacter(object sender, EventArgs e)
	{

		GameObject[] currentPlayerCursors = GameEntity.FindAllByTag(GameTag.Cursor);

		foreach (var cursor in currentPlayerCursors)
		{
			if (!cursor.GetComponent<CursorSelectionBehaviour>().hasSelectedWizardCharacter)
			{
				Debug.Log(cursor + " object has not selected a player!");
				return;
			}
		}


		for (int i = 0; i < currentPlayerCursors.Length; i++)
		{
			var playerInput = currentPlayerCursors[i].GetComponent<PlayerInput>();
			
			var cursor = currentPlayerCursors[i].GetComponent<CursorSelectionBehaviour>();

			var wizardSelectionName = cursor.GetWizard();

			var playerIndex = playerInput.playerIndex;

			Controllers.Add(playerIndex, playerInput.devices[0]);

			if (!PlayerNames.ContainsKey(playerIndex))
			{
				PlayerNames.Add(playerIndex, new List<string>() {  wizardSelectionName });
			}
			else
			{
				var currentCharacterList = PlayerNames[playerIndex];

				currentCharacterList.Add(wizardSelectionName);
			}


			ControlActionSchemes.Add(playerInput.playerIndex, playerInput.currentControlScheme);
		}


		m_ShouldPersistCursorObjects = false;

		m_ShouldSpawnSelectedPlayers = true;
	

		// Load the scene 
		GameScenes.LoadScene(Scenes.EndlessMagic_GameLevel_01, true);
	}

	*/
}