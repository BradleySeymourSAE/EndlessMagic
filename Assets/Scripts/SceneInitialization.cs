﻿#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
#endregion


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


		if (CharacterObjectSpawnManager.m_ShouldSpawnSelectedPlayers)
		{
			HandleSpawnSelectedPlayers();
			CharacterObjectSpawnManager.m_ShouldSpawnSelectedPlayers = false;
		}
	}


	private void HandleSpawnSelectedPlayers()
	{
		
		foreach (var player in CharacterObjectSpawnManager.Controllers)
		{

			var controller = CharacterObjectSpawnManager.Controllers[player.Key];
			var currentPlayerName = CharacterObjectSpawnManager.PlayerNames[player.Key];
			var currentPlayerControlScheme = CharacterObjectSpawnManager.ControlSchemes[player.Key];

			GameObject s_ParentGameObject = new GameObject();

			for (int i = 0; i < currentPlayerName.Count; i++)
			{
				var currentSelectionPrefab = Resources.Load<GameObject>(currentPlayerName[i]);


				if (i == 0)
				{
					s_ParentGameObject = currentSelectionPrefab;
					PlayerInput playerInput = PlayerInput.Instantiate(currentSelectionPrefab, player.Key, currentPlayerControlScheme, -1, controller);

					// Activates the player input component on the prefab that was instantiated 

					currentSelectionPrefab.GetComponent<WizardInputControls>().SetAllowPlayerInput(true, playerInput);


					// The above code does not work - It appears as through the PlayerInput component on the prefab is starting off
					// disabled, which makes it not work. This next bit of code should force it to keep the device/scheme/etc that we
					// tried to assign earlier. 

					var inputUser = playerInput.user;

					playerInput.SwitchCurrentControlScheme(currentPlayerControlScheme);

					InputUser.PerformPairingWithDevice(controller, inputUser, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
				}

				// Otherwise, If not the first object is unrelated, just instantiate and dont associate it with a PlayerInput type. 
				else
				{
					Instantiate(currentSelectionPrefab, s_ParentGameObject.transform);
				}
			}
		}

	}
}