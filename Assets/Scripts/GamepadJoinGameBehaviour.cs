#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;
#endregion


public class GamepadJoinGameBehaviour : MonoBehaviour
{

	#region Public Variables

	/// <summary>
	///		Reference to the number of active players in the game 
	/// </summary>
	public int numberOfActivePlayers { get; private set; } = 0;

	#endregion


	#region Private Variables
	/// <summary>
	///		The selection panel of the canvas 
	/// </summary>
	[SerializeField] List<Transform> m_PlayerJoinContainers = new List<Transform>();

	#endregion


	private void Start()
	{

		var myAction = new InputAction(binding: "/*/<button>");

		myAction.performed += action =>
		{
			AddGamepad(action.control.device);
		};


		myAction.Enable();
	}


	#region Private Methods

	private void AddGamepad(InputDevice device)
	{
		
		// Loop through all the player inputs 
		foreach (var currentPlayer in PlayerInput.all)
		{

			// if you find an identical device, return 
			foreach (var playerDevice in currentPlayer.devices)
			{
				if (device == playerDevice)
				{
					return;
				}
			}
		}

		// If the device is not a controller, joystick or gamepad, return 
		if (!device.displayName.Contains("Controller") &&
			!device.displayName.Contains("Joystick") && 
			!device.displayName.Contains("Gamepad") &&
			!device.description.manufacturer.Contains(GameText.PS4ControllerDevice)
		) {
			return;
		}

		var s_CurrentPlayerIndex = PlayerInput.all.Count + 1;

		string s_ActionControlScheme = "";

		if (device.displayName.Contains("Controller") || device.displayName.Contains("Gamepad"))
		{
			s_ActionControlScheme = "Gamepad";
		}
		else if (device.displayName.Contains("Joystick"))
		{
			s_ActionControlScheme = "Joystick";
		}
		else if (device.displayName.Contains("Keyboard") || device.displayName.Contains("Mouse"))
		{
			s_ActionControlScheme = "Keyboard&Mouse";
		}


		Debug.Log("Action Control Scheme: " + s_ActionControlScheme);


		// Load up the cursor prefabs 
		GameObject playerCursor = Resources.Load<GameObject>($"CursorPrefabs/P{s_CurrentPlayerIndex}_Cursor");

		// check if the player is active in the Hierarchy 
		if (!playerCursor.activeInHierarchy)
		{
			PlayerInput playerInputCursor = PlayerInput.Instantiate(playerCursor, -1, s_ActionControlScheme, -1, device);
		
			RectTransform s_ParentTransform = m_PlayerJoinContainers[s_CurrentPlayerIndex - 1].GetComponent<RectTransform>();


			Debug.LogWarning(
				"Parent Transform Name: " + s_ParentTransform.name + 
				"Player Cursor: " + playerInputCursor.name + 
				"Action Control Scheme: " + s_ActionControlScheme
			);


			playerInputCursor.transform.SetParent(s_ParentTransform);

			RectTransform cursorTransform = playerInputCursor.GetComponent<RectTransform>();

			cursorTransform.localPosition = new Vector3(0,0,0);
			cursorTransform.anchorMin = new Vector2(0.5f, 1);
			cursorTransform.anchorMax = new Vector2(0.5f, 1);




			playerCursor.transform.localScale = new Vector3(1, 1, 1);
		}
	}



	public void OnPlayerJoin(PlayerInput input)
	{
		numberOfActivePlayers = PlayerInput.all.Count;
		Debug.Log("[GamepadJoinGameBehaviour.HandleOnPlayerJoin]: " + "There are currently " + numberOfActivePlayers + " players.");
		GameEvents.SetPlayerJoinedEvent?.Invoke(1);
	}

	public void OnPlayerLeft(PlayerInput input)
	{
		numberOfActivePlayers = PlayerInput.all.Count;
		Debug.Log("[GamepadJoinGameBehaviour.HandleOnPlayerLeft]: " + "There are currently " + numberOfActivePlayers + " players.");

		GameEvents.SetPlayerJoinedEvent?.Invoke(-1);
	}
	#endregion

}
