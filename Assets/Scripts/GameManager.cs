#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
#endregion


/// <summary>
///		Game Manager
///		- Handles the core gameplay loop
///		- Spawns the players into the game 
///		- Creates the racetrack procedurally 
/// </summary>
public class GameManager : MonoBehaviour
{

	#region Static

	/// <summary>
	///		Reference to the Game Manager Instance 
	/// </summary>
	public static GameManager Instance;

	#endregion

	#region Private Variables

	/// <summary>
	///		Current total connected devices
	/// </summary>
	[SerializeField] private int m_TotalConnectedDevices;

	/// <summary>
	///		The current total connected players
	/// </summary>
	[SerializeField] private int m_TotalConnectedPlayers;

	/// <summary>
	///		List of currently connected devices 
	/// </summary>
	[SerializeField] private List<GameInputDevice> m_ConnectedDevices = new List<GameInputDevice>();

	/// <summary>
	///		List of currently connected players (With Player Identity's) 
	/// </summary>
	[SerializeField] private List<PlayerInputDevice> m_ConnectedPlayers = new List<PlayerInputDevice>();

	[SerializeField] private List<Wizard> m_Wizards = new List<Wizard>();

	/// <summary>
	///		Whether we are currently debugging 
	/// </summary>
	[SerializeField] private bool m_Debugging = true;

	/// <summary>
	///		Reference to the game start coroutine 
	/// </summary>
	private Coroutine m_GameStartRoutine;

	/// <summary>
	///		Whether to allow multiple device input 
	/// </summary>
	[SerializeField] private bool m_AllowMultipleDeviceInput = false;

	#endregion

	#region Unity References

	/// <summary>
	///		Events Subscribing 
	/// </summary>
	private void OnEnable()
	{
		InputSystem.onDeviceChange += HandleOnControllerInputDeviceChanged;

		GameEvents.onSetCurrentConnectedDevices += UpdateCurrentConnectedDevices;
		GameEvents.LoadConnectedDevicesEvent += LoadDevices;
		GameEvents.AddPlayerInputDeviceEvent += AddPlayerInputDevice;
	}

	/// <summary>
	///		Events Unsubscribing
	/// </summary>
	private void OnDisable()
	{
		InputSystem.onDeviceChange -= HandleOnControllerInputDeviceChanged;
	
		GameEvents.onSetCurrentConnectedDevices -= UpdateCurrentConnectedDevices;
		GameEvents.LoadConnectedDevicesEvent -= LoadDevices;
		GameEvents.AddPlayerInputDeviceEvent -= AddPlayerInputDevice;
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		// Runs the Game Start Logic 

		if (m_GameStartRoutine != null)
		{
			StopCoroutine(m_GameStartRoutine);
		}

		m_GameStartRoutine = StartCoroutine(HandleGameStartLogic());
	}

	private IEnumerator HandleGameStartLogic()
	{
		// Loads the currently connected devices 
		Debug.Log("[GameManager.HandleGameStartLogic]: " + "Loading connected devices...");
		GameEvents.LoadConnectedDevicesEvent?.Invoke();

		Debug.Log("[GameManager.HandleGameStartLogic]: " + "Waiting a second before continuing..");
		yield return new WaitForSeconds(1f);



		yield return null;
	}

	private void Update()
	{
		
		m_AllowMultipleDeviceInput = GameUIManager.Instance.AllowMultipleDeviceInput() == true;
		

		if (m_AllowMultipleDeviceInput)
		{ 
			if (Gamepad.current.startButton.wasPressedThisFrame)
			{
				Debug.Log("[GameManager.Update]: " + "Gamepad Input Device ID: " + Gamepad.current.deviceId + " pressed " + Gamepad.current.startButton);

				GameEvents.AddPlayerInputDeviceEvent?.Invoke(Gamepad.current.deviceId);
			}
		}
	
	}

	#endregion

	#region Public Methods

	/// <summary>
	///		Gets and sets the currently connected devices 
	/// </summary>
	public int GetConnectedDeviceIndex
	{
		get
		{
			return m_TotalConnectedDevices;
		}
		set
		{
			m_TotalConnectedDevices = value;

			GameEvents.HandleUpdateDevicesEvent?.Invoke();
		}
	}

	/// <summary>
	///		Gets and sets the currently connected players index 
	/// </summary>
	public int GetConnectedPlayersIndex
	{
		get
		{
			return m_TotalConnectedPlayers;
		}
		set
		{
			m_TotalConnectedPlayers = value;

			GameEvents.HandleUpdateDevicesEvent?.Invoke();
		}
	}

	/// <summary>
	///		Returns the type of input controller by the InputSystems.InputDevice name property 
	/// </summary>
	/// <param name="p_DeviceDescription">The name of the input device</param>
	/// <returns>The Input Controller Type</returns>
	public InputController GetInputControllerDevice(InputDevice p_ControllerInputDevice)
	{
		// If the input device display name contains the mouse device key 
		if (p_ControllerInputDevice.device.displayName.Contains(GameText.MouseDevice))
		{
			// return the input controller as a mouse
			return InputController.Mouse;
		}
		// Otherwise if the input device display name contains the keyboard device key 
		else if (p_ControllerInputDevice.device.displayName.Contains(GameText.KeyboardDevice))
		{
			// return the input controller as a keyboard 
			return InputController.Keyboard;
		}
		else
		{
			// Otherwise, look through the input device's manufacturer 
			switch (p_ControllerInputDevice.description.manufacturer)
			{
				// If the manufacturer matches the ps4 controller type, return the device as a ps4 controller 
				case GameText.PS4ControllerDevice:
					return InputController.Playstation;
					// otherwise, if the manufactrer matches an empty string, just assume it's an xbox controller 
				case GameText.XboxControllerDevice:
					return InputController.Xbox;
				default:
					// Otherwise return input controller as unknown 
					return InputController.Unknown;
					break;
			}
		}
	}

	#endregion

	#region Private Methods 

	/// <summary>
	///		Handles when a controller has either been added, removed, disconected or reconnected
	/// </summary>
	/// <param name="p_InputDevice">The input device that has been added, removed, disconnected or reconnected</param>
	/// <param name="p_InputDeviceState">The state of the device whenever the state changes</param>
	private void HandleOnControllerInputDeviceChanged(InputDevice p_InputDevice, InputDeviceChange p_InputDeviceState)
	{
		switch (p_InputDeviceState)
		{
			case InputDeviceChange.Added: // when an input device is added 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Trying to add Input Device " + p_InputDevice.displayName + " State " + p_InputDeviceState);
			
				GameInputDevice s_newDevice = new GameInputDevice
				{
					name = p_InputDevice.name,
					controller = GetInputControllerDevice(p_InputDevice),
					identity = p_InputDevice.deviceId
				};


				// If the new device has already been connected we want to return 
				if (m_ConnectedDevices.Contains(s_newDevice))
				{
					return;
				}
				else
				{
					Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Successfully added input device " + s_newDevice.name + " type: " + s_newDevice.controller.ToString());
					// Otherwise we can add the device in 
					m_ConnectedDevices.Add(s_newDevice);
				}
				
				break;
				}
			case InputDeviceChange.Removed: // When an input device is removed 
				{

					Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Attempting to remove Input Device " + p_InputDevice.displayName + " State " + p_InputDeviceState);

					// Loop through each device in the connected devices list 
					for (int i = 0; i < m_ConnectedDevices.Count; i++)
				{
					// If the device identity is the same as the input device's ID 
					if (m_ConnectedDevices[i].identity == p_InputDevice.deviceId)
					{
						GameInputDevice device = m_ConnectedDevices[i];
						Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Successfully added input device " + device.name + " type: " + device.controller.ToString());
						
							// Remove the device from the list of connected devices 
						m_ConnectedDevices.Remove(device);
					}
				}

				break;
				}
			case InputDeviceChange.Disconnected: // When a controller disconnects 
				{
					Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Attempting to Disconnect Input Controller" + p_InputDevice.displayName + " State " + p_InputDeviceState);


					// This is where we would store data locally

					GameEvents.onSetCurrentConnectedDevices?.Invoke(-1);

				break;
				}
			case InputDeviceChange.Reconnected: // When a controller reconnects 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Attempting to reconnect controller input device" + p_InputDevice.displayName + " State " + p_InputDeviceState);

					// Then once the controller is reconnected we can apply the data we stored in a private list here.. 

				GameEvents.onSetCurrentConnectedDevices?.Invoke(1);

				break;
				}
		}
	}

	/// <summary>
	///		Updates the active players using the SetCurrentActivePlayersEvent Called from HandleOnControllerInputDeviceChanged 
	/// </summary>
	/// <param name="UpdatedDeviceIndex"></param>
	private void UpdateCurrentConnectedDevices(int UpdatedDeviceIndex) => GetConnectedDeviceIndex += UpdatedDeviceIndex; 

	/// <summary>
	///		Loads the devices that are currently connected
	/// </summary>
	private void LoadDevices()
	{
		// Clear the list of connected devices 
		m_ConnectedDevices.Clear();
		
		// Clear the list of connected players 
		m_ConnectedPlayers.Clear();

		// Loop through the input system connected devices 
		for (int i = 0; i < InputSystem.devices.Count; i++)
		{
			// Get the current input device 
			InputDevice device = InputSystem.GetDeviceById(InputSystem.devices[i].deviceId);

			// Return the input controller type using the device as a parameter 
			InputController s_Controller = GetInputControllerDevice(device);
			
			// Set the player identity to none 
			PlayerIdentity s_PlayerIdentity = PlayerIdentity.None;

			// If the controller is of a mouse or keyboard input type 
			if (s_Controller == InputController.Mouse || s_Controller == InputController.Keyboard)
			{
				// Set the player identity to player 1 
				s_PlayerIdentity = PlayerIdentity.P1;
			}

			// Add the new GameInputDevice to the connected devices list
			m_ConnectedDevices.Add(
					new GameInputDevice
					{
						name = device.name,
						controller = s_Controller,
						identity = device.deviceId,
						player = s_PlayerIdentity
					}
				);
		}

		// Check if there is a keyboard and mouse attached 
		bool s_HasKeyboardAndMouse = m_ConnectedDevices.Any(input => input.controller == InputController.Mouse) && m_ConnectedDevices.Any(input => input.controller == InputController.Keyboard);

		// If there is a keyboard and mouse game input device 
		if (s_HasKeyboardAndMouse)
		{
			// Create a temporary list and add both devices to it 
			List<GameInputDevice> temporary = new List<GameInputDevice>
			{
				m_ConnectedDevices.Find(input => input.controller == InputController.Mouse),
				m_ConnectedDevices.Find(input => input.controller == InputController.Keyboard),
			};


			// Create a new player input device in the connected players list 
			m_ConnectedPlayers.Add(
					new PlayerInputDevice
					{
						name = "Player 1",
						devices = temporary,
						identity = PlayerIdentity.P1,
					}
				);

			GetConnectedPlayersIndex = m_ConnectedPlayers.Count;
		}

		// Set the current connected device index
		GetConnectedDeviceIndex = m_ConnectedDevices.Count;
	}

	/// <summary>
	///		Returns a player identity index based on the value given 
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	private PlayerIdentity ReturnPlayerIdentity(int value)
	{
		switch (value)
		{
			case 0:
				return PlayerIdentity.None;
			case 1:
				return PlayerIdentity.P1;
			case 2:
				return PlayerIdentity.P2;
			case 3:
				return PlayerIdentity.P3;
			case 4:
				return PlayerIdentity.P4;
			default:
				return PlayerIdentity.None;
		}
	}

	/// <summary>
	///		Adds a player input device by the input device ID 
	/// </summary>
	/// <param name="DeviceID"></param>
	private void AddPlayerInputDevice(int DeviceID)
	{

		for (int i = 0; i < m_ConnectedDevices.Count; i++)
		{

			// Check if the identity id already exists 
			if (m_ConnectedDevices[i].identity == DeviceID)
			{
				Debug.Log("[GameManager.AddPlayerInputDevice]: " + "Attemping to add player to the connected players");
				// Get the current input device 
				GameInputDevice currentDevice = m_ConnectedDevices[i];


				if (m_ConnectedPlayers.Any(player => player.devices.Any(device => device.identity == currentDevice.identity)))
				{
					Debug.LogWarning("[GameManager.AddPlayerInputDevice]: " + "Device already exists... returning.");
					return;
				}

				// Get the next player index in the connected players list 
				int nextPlayerIndex = m_ConnectedPlayers.Count + 1;

				Debug.Log("[GameManager.AddPlayerInputDevice]: " + "The next connected player list index: " + nextPlayerIndex);

				// Set the device's player identity 
				currentDevice.player = ReturnPlayerIdentity(nextPlayerIndex);

				PlayerInputDevice s_PlayerDevice = new PlayerInputDevice
				{
					name = "Player " + nextPlayerIndex.ToString(),
					devices = new List<GameInputDevice>
					{
						currentDevice,
					},
					identity = ReturnPlayerIdentity(nextPlayerIndex),
				};


				// Before adding the input device, we should check if the player device is already in the connected players list.
				if (m_ConnectedPlayers.Contains(s_PlayerDevice))
				{
					// If it is, we want to back the fuck right out of here 
					return;
				}
				// Otherwise, add the device to the connected players list 
				else
				{ 
					m_ConnectedPlayers.Add(s_PlayerDevice);
				}
			}


		}

		// Set the connected playesr index to the connected players count 
		GetConnectedPlayersIndex = m_ConnectedPlayers.Count;
	}

	#endregion

}



/// <summary>
///		Device Input Controller Types
/// </summary>
public enum InputController { Unknown = 0, Keyboard = 1, Mouse = 2, Xbox = 3, Playstation = 4 };

/// <summary>
///		The Player's Input Controller Identity Number (Player 1, Player 2, Player 3, Player 4) 
/// </summary>
public enum PlayerIdentity { None = 0, P1 = 1, P2 = 2, P3 = 3, P4 = 4 };

/// <summary>
///		Data class for storing details for a device that's connected 
/// </summary>
[System.Serializable]
public class GameInputDevice
{

	#region Public Variables 

	/// <summary>
	///		The name of the input device 
	/// </summary>
	public string name;

	/// <summary>
	///		The type of input device the controller is 
	/// </summary>
	public InputController controller;

	/// <summary>
	///  Which identity is the device attached to 
	/// </summary>
	public PlayerIdentity player;

	/// <summary>
	///		The unique id of the device 
	/// </summary>
	public int identity;

	#endregion

}

[System.Serializable]
public class PlayerInputDevice
{

	#region Public Variables 

	/// <summary>
	///		The Connected Player Identity name
	/// </summary>
	public string name;

	/// <summary>
	///		List of player input devices 
	/// </summary>
	public List<GameInputDevice> devices;

	/// <summary>
	///		The player's identity 
	/// </summary>
	public PlayerIdentity identity;

	#endregion

}

