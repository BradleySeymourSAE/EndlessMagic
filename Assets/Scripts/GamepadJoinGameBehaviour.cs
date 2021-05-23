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

		// If the device is not a joystick, gamepad or ps4 controller
		if (!device.displayName.Contains("Controller") &&
			!device.displayName.Contains("Joystick") && 
			!device.displayName.Contains("Gamepad") &&
			!device.description.manufacturer.Contains(GameText.PS4ControllerDevice)
		) {
			// Then we want to return
			return;
		}

	
		device.MakeCurrent();   // make this device the current device 

		var s_CurrentPlayerIndex = PlayerInput.all.Count + 1; // Find the current players index

		string s_ActionControlScheme = ReturnControlScheme(device); // Get the Action Control Scheme

		// Find the current player cursor asset using the current player device index
		GameObject playerCursor = GameEntity.FindAsset(ResourceFolder.CursorPrefabs, s_CurrentPlayerIndex, Asset.Cursor, SceneAsset.None, false);
	
		// Set the player cursor's type (Using the action control scheme) 

		playerCursor.GetComponent<CursorSelectionBehaviour>().SetControllerType(s_ActionControlScheme);


		// check if the players cursor is active in the Hierarchy 
		if (!playerCursor.activeInHierarchy)
		{
		
			// Instantiate a playerInputCursor using the action control schema 
			PlayerInput playerInputCursor = PlayerInput.Instantiate(playerCursor, -1, s_ActionControlScheme, -1, device);
			
			// Set the player input cursor's tag 
			playerInputCursor.gameObject.tag = GameEntity.ReturnGameTag(GameTag.Cursor);

			// Set the cursor's parent transform
			RectTransform s_ParentTransform = m_PlayerJoinContainers[s_CurrentPlayerIndex - 1].GetComponent<RectTransform>();

			// Get the status text to update the button to be pressed 
			TMP_Text s_StatusText = GameEntity.FindSceneAsset(s_CurrentPlayerIndex, SceneAsset.StatusText).GetComponentInChildren<TMP_Text>();
			
			// Find the button east display name 
			var buttonText = Gamepad.current.buttonSouth.displayName;

			// If the status text is not null 
			if (buttonText != null)
			{
				// Set the status text with the button text 
				s_StatusText.text = $"Press {buttonText} to ready up!";
			}
			else
			{
				// Otherwise - Set the default text 
				s_StatusText.text = GameText.PlayerJoinUI_PlayerStatus_SlotTaken_ReadyUp;
			}
			
			if (Debugging)
			{ 
				Debug.LogWarning(
					" Parent Transform Name: " + s_ParentTransform.name + 
					" Player Cursor: " + playerInputCursor.name + 
					" Action Control Scheme: " + s_ActionControlScheme
				);
			}

			playerInputCursor.transform.SetParent(s_ParentTransform);

			RectTransform cursorTransform = playerInputCursor.GetComponent<RectTransform>();

			cursorTransform.localPosition = new Vector3(0,0,0);
			cursorTransform.anchorMin = new Vector2(0.5f, 1);
			cursorTransform.anchorMax = new Vector2(0.5f, 1);
			playerCursor.transform.localScale = new Vector3(1, 1, 1);


			foreach (InputControl control in device.allControls)
			{

				playerCursor.GetComponent<CursorSelectionBehaviour>().Controls.Add(control.displayName);

			}
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
			Debug.Log("[GamepadJoinGameBehaviour.HandleOnPlayerJoin]: " + "There are currently " + numberOfActivePlayers + " players.");
		}

		numberOfActivePlayers = PlayerInput.all.Count;
	


		if (GameManager.Instance.AllowPlayerJoining == true)
		{ 
			CursorSelectionBehaviour cursor = input.GetComponent<CursorSelectionBehaviour>();

			m_CurrentPlayers.Add(cursor);
			GameEvents.SetPlayerJoinedEvent?.Invoke(numberOfActivePlayers);
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
			Debug.Log("[GamepadJoinGameBehaviour.HandleOnPlayerLeft]: " + "Player left the game. There are " + numberOfActivePlayers + " remaining players.");
		}


		numberOfActivePlayers = PlayerInput.all.Count;
		

		if (GameManager.Instance.AllowPlayerJoining == true)
		{ 
			CursorSelectionBehaviour cursor = input.GetComponent<CursorSelectionBehaviour>();

			m_CurrentPlayers.Remove(cursor);
			
	
			GameEvents.SetPlayerJoinedEvent?.Invoke(numberOfActivePlayers);
			input.DeactivateInput();
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
			device.displayName.Contains(GameText.PS4ControllerDevice))
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
