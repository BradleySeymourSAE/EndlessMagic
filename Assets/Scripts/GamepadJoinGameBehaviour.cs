#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;
using System.Linq;
using TMPro;
#endregion



/// <summary>
/// /	Handles Gamepad Joining Behaviour 
/// </summary>
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

	[SerializeField] List<CursorSelectionBehaviour> m_CurrentPlayers = new List<CursorSelectionBehaviour>();

    InputAction inputAction = new InputAction(binding: "/*/<button>");

	[SerializeField] private List<string> m_InputActionControlNames = new List<string>();

	private bool Debugging;

	#endregion

	#region Unity References 

	private void Start()
	{
		m_InputActionControlNames.Clear();

		m_CurrentPlayers.Clear();

		inputAction.performed += action =>
		{
			if (action.control.displayName.Contains("Start") || action.control.displayName.Contains("Options"))
			{ 
				AddGamepad(action.control.device);
			}
		};
	
		inputAction.Enable();
	}

	private void OnDisable()
	{
		inputAction.performed -= action =>
		{
			if (action.control.displayName.Contains("Start") || action.control.displayName.Contains("Options"))
			{ 
				AddGamepad(action.control.device);
			}
		};

		inputAction.Disable();
	}

	private void Update()
	{
		if (GameManager.Instance)
		{
			Debugging = GameManager.Instance.Debugging;
		}
	}


	#endregion


	#region Private Methods

	private void AddGamepad(InputDevice device)
	{
		
		if (!GameManager.Instance.AllowPlayerJoining)
		{
			return;
		}

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

		// If the device is not a joystick, gamepad or controller type (OR sony controller device) 
		if (!device.displayName.Contains("Controller") &&
			!device.displayName.Contains("Joystick") && 
			!device.displayName.Contains("Gamepad") &&
			!device.description.manufacturer.Contains(GameText.SonyControllerDevice)
		) {
			// Then we want to return
			return;
		}

		// Make this device the current one 
		device.MakeCurrent();

		// Get the current player index from the PlayerInput.all array 
		var s_CurrentPlayerIndex = PlayerInput.all.Count + 1; // Find the current players index

		string s_ActionControlScheme = ReturnControlScheme(device); // Get the Action Control Scheme

		// Find the current player cursor asset using the current player device index
		GameObject playerCursor = GameEntity.FindAsset(ResourceFolder.CursorPrefabs, s_CurrentPlayerIndex, Asset.Cursor, SceneAsset.None, false);

		// check if the players cursor is active in the Hierarchy 
		if (!playerCursor.activeInHierarchy)
		{
		
			// Instantiate a playerInputCursor using the action control schema 
			PlayerInput playerInputCursor = PlayerInput.Instantiate(playerCursor, -1, s_ActionControlScheme, -1, device);
			
			// Set the player input cursor's tag 
			playerInputCursor.gameObject.tag = GameEntity.ReturnGameTag(GameTag.Cursor);

			// Set the cursor's parent transform
			RectTransform s_ParentTransform = m_PlayerJoinContainers[s_CurrentPlayerIndex - 1].GetComponent<RectTransform>();

			// Set the player input cursor parent transform to the player join containers current index's transform rect  
			playerInputCursor.transform.SetParent(s_ParentTransform);

			// Get the status text to update the button to be pressed 
			TMP_Text s_StatusText = GameEntity.FindSceneAsset(s_CurrentPlayerIndex, SceneAsset.StatusText).GetComponentInChildren<TMP_Text>();
			Image s_StatusIcon = GameEntity.FindSceneAsset(s_CurrentPlayerIndex, SceneAsset.StatusIcon).GetComponent<Image>();

			// Get the current controller type 
			GameControllerType s_ControllerType = GameEntity.GetGameControllerType(Gamepad.current);

			// Get the Cursor Selection Behaviour component attached to the player input cursor 
			CursorSelectionBehaviour s_CursorSelectionBehaviour = playerInputCursor.GetComponent<CursorSelectionBehaviour>();

			// Set the game controller type for the cursor selection behaviour reference 
			s_CursorSelectionBehaviour.SetGameControllerType(s_ControllerType);

			// Get the game controller ui data by passing in the controller type 
			GameControllerUIData s_GameControllerData = GameEntity.GetControllerUIData(s_ControllerType);

			// Set the game controller UI data for the cursor selection behaviour reference 
			s_CursorSelectionBehaviour.SetGameControllerUIData(s_GameControllerData);

			// Set the status text to ready up 
			s_StatusText.text = GameText.PlayerJoinUI_PlayerStatus_SlotTaken_ReadyUp;

			// Reset the status text position by grabbing the local position of the text 
			Vector3 newStatusTextPosition = s_StatusText.GetComponent<RectTransform>().localPosition;

			// I just know that this value should either be 55 (With the Icon), or 23 - Without the icon 
			newStatusTextPosition.x = 55f;

			// Set the status text local position to the newStatusTextPosition 
			s_StatusText.GetComponent<RectTransform>().localPosition = newStatusTextPosition;

			// Set the status icon sprite image to the controller's button south image 
			s_StatusIcon.sprite = s_CursorSelectionBehaviour.ControllerUI.ButtonSouth;

			// Set the status icon visibility to be enabled
			s_StatusIcon.enabled = true;

			// Simple check, if we are debuggin this is handy 
			if (Debugging)
			{ 
				Debug.LogWarning(
					" Parent Transform Name: " + s_ParentTransform.name + 
					" Player Cursor: " + playerInputCursor.name + 
					" Action Control Scheme: " + s_ActionControlScheme
				);
			}

			// Get the current rect transform for the cursor 
			RectTransform cursorTransform = playerInputCursor.GetComponent<RectTransform>();

			cursorTransform.localPosition = new Vector3(0,0,0); // Set the local position of the cursor to zero (or it shits bricks) 
			cursorTransform.anchorMin = new Vector2(0.5f, 1); // Anchor to the center, middle 
			cursorTransform.anchorMax = new Vector2(0.5f, 1); // Anchor to the center, middle 
			playerCursor.transform.localScale = new Vector3(1, 1, 1); // Set the local scale of the transform to 1 
		}
	}

	/// <summary>
	///		Handles when a player joins on the Player Join Screen  
	/// </summary>
	/// <param name="input"></param>
	public void OnPlayerJoin(PlayerInput input)
	{
		if (Debugging)
		{
			Debug.LogWarning("[GamepadJoinGameBehaviour.OnPlayerJoin]: " + "There are currently " + numberOfActivePlayers + " players.");
		}

		numberOfActivePlayers = PlayerInput.all.Count;
	

		// Check if we are currently allowing player joining 
		if (GameManager.Instance.AllowPlayerJoining == true)
		{
			// If the input has the CursorSelectionBehaviourComponent script 
			if (input.GetComponent<CursorSelectionBehaviour>())
			{
				// Set reference in the Game Manager Instance 
				CursorSelectionBehaviour s_CurrentPlayerCursor = input.GetComponent<CursorSelectionBehaviour>();
				
				// Add the current cursor to the current players list 
				m_CurrentPlayers.Add(s_CurrentPlayerCursor);

				// Invoke the Player Joined Event taking in the number of active players 

				GameEvents.SetPlayerJoinedEvent?.Invoke(numberOfActivePlayers);

				// Activate input 
				input.ActivateInput();
			}
		}
	}

	/// <summary>
	///		Handles when a player leaves on the Player Join Screen 
	/// </summary>
	/// <param name="input"></param>
	public void OnPlayerLeft(PlayerInput input)
	{
		if (Debugging)
		{
			Debug.LogWarning("[GamepadJoinGameBehaviour.OnPlayerLeft]: " + "Player left the game. There are " + numberOfActivePlayers + " remaining players.");
		}

		// Update the number of active players remaining 
		numberOfActivePlayers = PlayerInput.all.Count;
		
		// If allow player joining is true
		if (GameManager.Instance.AllowPlayerJoining == true)
		{
			// If the PlayerInput (player) has the CursorSelectionBehaviour script 
			if (input.GetComponent<CursorSelectionBehaviour>())
			{ 
				// Get a reference to the current player cursor 
				CursorSelectionBehaviour s_CurrentPlayerCursor = input.GetComponent<CursorSelectionBehaviour>();

				// Remove the player cursor from the current active players list in the GameManager Instance 
				m_CurrentPlayers.Remove(s_CurrentPlayerCursor);
			
				// Invoke the SetPlayerJoinedEvent taking in the updated amount of active players 
				GameEvents.SetPlayerJoinedEvent?.Invoke(numberOfActivePlayers);

				// Deactivate the input 
				input.DeactivateInput();
			}
		}
	}

	#endregion

	#region Private Methods


	/// <summary>
	///		Returns the current control scheme using the input device display name 
	/// </summary>
	/// <param name="device"></param>
	/// <returns></returns>
	private string ReturnControlScheme(InputDevice device)
	{
		if (
			device.displayName.Contains(GameText.XboxControllerDevice) || 
			device.displayName.Contains(GameText.GamepadDevice) || 
			device.displayName.Contains(GameText.SonyControllerDevice))
		{
			return GameText.ActionControlScheme_Gamepad;
		}
		else if (device.displayName.Contains(GameText.JoystickDevice))
		{
			return GameText.ActionControlScheme_Joystick;
		}
		else if (device.displayName.Contains(GameText.KeyboardDevice) || device.displayName.Contains(GameText.MouseDevice))
		{
			return GameText.ActionControlScheme_KeyboardMouse;
		}
		else
		{
			return device.displayName;
		}
	}

	#endregion

}
