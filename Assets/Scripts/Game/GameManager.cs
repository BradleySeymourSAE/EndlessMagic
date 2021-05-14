#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
#endregion



#region @TODO - Move these to their own seperate handler, maybe inside the Game Assets Script 

/// <summary>
///		Device Platform ID's - These are what I know anyways, Playstation I have not tested yet but I am guessing 
/// </summary>
public enum InputController { Unknown = 0, Keyboard = 1, Mouse = 2, Xbox = 3, Playstation = 4 };

/// <summary>
///		The players controller ID - Used to select which player is player 1, 2, 3, 4 and so on 
/// </summary>
public enum PlayerIdentity { None = 0, P1 = 1, P2 = 2, P3 = 3, P4 = 4 };



/// <summary>
///		Data class for storing details for a device that's connected 
/// </summary>
[System.Serializable]
public class GameDeviceData
{
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

	[SerializeField]
	/// <summary>
	///		Reference to the Data associated with the currently connected input device 
	/// </summary>
	public InputDevice device;

}

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
	///		Current total connected devices (Minus 1 because Mouse & Keyboard should be set as a single player? ) 
	/// </summary>
	[SerializeField] private int m_TotalConnectedDevices;

	/// <summary>
	///		List of currently connected devices 
	/// </summary>
	[SerializeField] private List<GameDeviceData> m_ConnectedDevices = new List<GameDeviceData>();

	/// <summary>
	///		Whether we are currently debugging 
	/// </summary>
	[SerializeField] private bool m_Debugging = true;


	#region Connected Device Types  @TODO - I will clean this up when I get the chance - Just testing the code atm 

	private const string Xbox_ControllerDevice = "XInputControllerWindows";

	private const string PS4_ControllerDevice = "DualShock4GamepadHID";

	private const string PC_MouseDevice = "Mouse";

	private const string PC_KeyboardDevice = "Keyboard";

	#endregion

	#endregion

	public int ActiveDevices
	{
		get
		{
			return m_TotalConnectedDevices;
		}
		set
		{
			m_TotalConnectedDevices = value;

			m_TotalConnectedDevices = Mathf.Clamp(m_TotalConnectedDevices, 0, 4);

		}
	}

	#region Unity References


	private void OnEnable()
	{
		InputSystem.onDeviceChange += HandleOnControllerInputDeviceChanged;
		GameEvents.SetCurrentActivePlayers += UpdateActivePlayers;
	}


	private void OnDisable()
	{
		InputSystem.onDeviceChange -= HandleOnControllerInputDeviceChanged;
		GameEvents.SetCurrentActivePlayers -= UpdateActivePlayers;
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
		
		// Clear the list of connected devices 
		m_ConnectedDevices.Clear();

		int deviceIndex = 0;

		// Loop through the input system connected devices 
		foreach (InputDevice Device in InputSystem.devices)
		{
			deviceIndex++;

			// Get the current input device 
			InputDevice s_inputDevice = InputSystem.GetDeviceById(Device.deviceId);


			// Set the input device's data 
			GameDeviceData newDevice = new GameDeviceData
			{
				name = s_inputDevice.name,
				controller = GetInputControllerDevice(s_inputDevice.name),
				identity = s_inputDevice.deviceId,
				device = Device,
			};

			// If the device's controller is a keyboard or a mouse device
			if (newDevice.controller == InputController.Keyboard || newDevice.controller == InputController.Mouse)
			{
				// Then we want to assign player 1 identity to that player by default 
				newDevice.player = PlayerIdentity.P1;
			}
			else
			{
				// Othwise we can set the player identity to the player's connected index 
				newDevice.player = PlayerIdentity.None;
			}


			// Add the 
			m_ConnectedDevices.Add(newDevice);
		}

		ActiveDevices = m_ConnectedDevices.Count;
	}

	#endregion

	#region Public Methods

	/// <summary>
	///		Returns the type of input controller by the InputSystems.InputDevice name property 
	/// </summary>
	/// <param name="p_DeviceName">The name of the input device</param>
	/// <returns>The Input Controller Type</returns>
	public InputController GetInputControllerDevice(string p_DeviceName)
	{
		var device = InputController.Unknown;


		switch (p_DeviceName)
		{
			case PC_MouseDevice:
				{
					device = InputController.Mouse;
				}
				break;
			case PC_KeyboardDevice:
				{ 
					device = InputController.Keyboard;
				}
				break;
			case Xbox_ControllerDevice:
				{
					device = InputController.Xbox;
				}
				break;
			case PS4_ControllerDevice:
				{
					device = InputController.Playstation;
				}
				break;
			default:
				{
					device = InputController.Unknown;
					Debug.LogWarning("[GameManager.GetDevicePlatform]: " + "Could not find that device, it is unknown!");
				}
				break;
		}

		return device;
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
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Added Input Device... " + p_InputDeviceState);

				GameDeviceData s_newDevice = new GameDeviceData
				{
					name = p_InputDevice.name,
					controller = GetInputControllerDevice(p_InputDevice.name),
					identity = p_InputDevice.deviceId,
					device = p_InputDevice.device,
				};


				// If the new device has already been connected we want to return 
				if (m_ConnectedDevices.Contains(s_newDevice))
				{
					return;
				}
				else
				{
					// Otherwise we can add the device in 
					m_ConnectedDevices.Add(s_newDevice);
				}
				
				break;
				}
			case InputDeviceChange.Removed: // When an input device is removed 
				{ 

				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Removed Input Device... " + p_InputDeviceState);

				// Loop through each device in the connected devices list 
				for (int i = 0; i < m_ConnectedDevices.Count; i++)
				{
					// If the device identity is the same as the input device's ID 
					if (m_ConnectedDevices[i].identity == p_InputDevice.deviceId)
					{
						GameDeviceData device = m_ConnectedDevices[i];
						// Remove the device from the list of connected devices 
						m_ConnectedDevices.Remove(device);
					}
				}

				break;
				}
			case InputDeviceChange.Disconnected: // When a controller disconnects 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Disconnected Input Device... " + p_InputDeviceState);

				// This is where we would store data locally

				GameEvents.SetCurrentActivePlayers?.Invoke(-1);

				break;
				}
			case InputDeviceChange.Reconnected: // When a controller reconnects 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Reconnected Input Device... " + p_InputDeviceState);

					// Then once the controller is reconnected we can apply the data we stored in a private list here.. 

				GameEvents.SetCurrentActivePlayers?.Invoke(1);

				break;
				}
		}
	}


	/// <summary>
	///		Updates the active players using the SetCurrentActivePlayersEvent Called from HandleOnControllerInputDeviceChanged 
	/// </summary>
	/// <param name="UpdatedPlayerCount"></param>
	private void UpdateActivePlayers(int UpdatedPlayerCount) => ActiveDevices += UpdatedPlayerCount; 

	#endregion

}