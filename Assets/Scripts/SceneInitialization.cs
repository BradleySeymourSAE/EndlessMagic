#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
#endregion

/// <summary>
///		TODO - Scene Initialization, I will work out another way of doing this part. Its a little messy lol 
/// </summary>
public class SceneInitialization : MonoBehaviour
{



	#region Unity References

	private void OnEnable()
	{
		SceneManager.activeSceneChanged += BeginSceneCheck;
	}

	private void OnDisable()
	{
		SceneManager.activeSceneChanged -= BeginSceneCheck;	
	}


	#endregion

	private void BeginSceneCheck(Scene p_FromScene, Scene p_ToScene)
	{
		/*
		 *		foreach (var scheme in CharacterObjectSpawnManager.playerControlSchemes) { Debug.Log(scheme); }
		 * 
		 */

	
		/* 
		if (CharacterObjectSpawnManager.m_ShouldSpawnSelectedPlayers)
		{
			HandleSpawnSelectedPlayers();
			CharacterObjectSpawnManager.m_ShouldSpawnSelectedPlayers = false;
		}
		*/
	}


	private void HandleSpawnSelectedPlayers()
	{
		
		/* foreach (var player in CharacterObjectSpawnManager.Controllers)
		{

			var playerController = CharacterObjectSpawnManager.Controllers[player.Key];
			var currentPlayerName = CharacterObjectSpawnManager.PlayerNames[player.Key];
			var currentPlayerControlScheme = CharacterObjectSpawnManager.ControlActionSchemes[player.Key];


			Debug.Log("Current Player Name: " + currentPlayerName);


			GameObject parentGameObject = new GameObject();

			for (int i = 0; i < currentPlayerName.Count; i++)
			{
				var currentSelectionPrefab = Resources.Load<GameObject>(currentPlayerName[i]);


				if (i == 0)
				{
					parentGameObject = currentSelectionPrefab;
					PlayerInput playerInput = PlayerInput.Instantiate(currentSelectionPrefab, player.Key, currentPlayerControlScheme, -1, playerController);

					// Activates the player input component on the prefab that was instantiated 

					currentSelectionPrefab.GetComponent<WizardInputControls>().SetAllowPlayerInput(true, playerInput);


					// The above code does not work - It appears as through the PlayerInput component on the prefab is starting off
					// disabled, which makes it not work. This next bit of code should force it to keep the device/scheme/etc that we
					// tried to assign earlier. 

					var inputUser = playerInput.user;

					Debug.Log("Input User: " + inputUser.id);

					playerInput.SwitchCurrentControlScheme(currentPlayerControlScheme);

					InputUser.PerformPairingWithDevice(playerController, inputUser, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
				}

				// Otherwise, If not the first object is unrelated, just instantiate and dont associate it with a PlayerInput type. 
				else
				{
					Instantiate(currentSelectionPrefab, parentGameObject.transform);
				}
			}
		}
		*/
	}
}