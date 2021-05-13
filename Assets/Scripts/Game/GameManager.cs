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
public enum DevicePlatform { Unknown = -1, PC = 1, Xbox = 13, Playstation = 14 };

/// <summary>
///		Data class for storing details for a device that's connected 
/// </summary>
[System.Serializable]
public class Device
{
	public string name;
	public DevicePlatform platform;
	public int identity;
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
	///		Current Active Players Index 
	/// </summary>
	[SerializeField] private int m_ActivePlayersIndex;

	/// <summary>
	///		List of currently connected devices 
	/// </summary>
	[SerializeField] private List<Device> m_ConnectedDevices = new List<Device>();

	/// <summary>
	///		Whether we are currently debugging 
	/// </summary>
	[SerializeField] private bool m_Debugging = true;


	#region Connected Device Types  @TODO - I will clean this up when I get the chance - Just testing the code atm 

	private const string Xbox_ControllerDevice = "Xbox Controller";

	private const string PS4_ControllerDevice = "PS4 Controller";

	private const string PC_MouseDevice = "Mouse";

	private const string PC_KeyboardDevice = "Keyboard";

	#endregion

	#endregion

	public int ActivePlayers
	{
		get
		{
			return m_ActivePlayersIndex;
		}
		set
		{
			m_ActivePlayersIndex = value;

			m_ActivePlayersIndex = Mathf.Clamp(m_ActivePlayersIndex, 0, 4);

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

		// Loop through the input system connected devices 
		for (int i = 0; i < InputSystem.devices.Count; i++)
		{
			// Get the input system devices index device id property 
			int s_InputDeviceId = InputSystem.devices[i].deviceId;

			// Then find the device by using the local variable input device id 
			InputDevice s_Device = InputSystem.GetDeviceById(s_InputDeviceId);

			Debug.Log("[GameManager.Start]: " + "Found Input Device: " + s_Device.displayName + " Device ID: " + s_Device.deviceId);


			DevicePlatform s_CurrentDevicePlatform = GetDevicePlatform(s_Device.displayName);

			Device device = new Device { 
				platform = s_CurrentDevicePlatform,
				identity = s_Device.deviceId, 
				name = s_Device.displayName
			};

		

			m_ConnectedDevices.Add(device);
		}


		ActivePlayers = m_ConnectedDevices.Count - 1;
	}

	#endregion

	#region Public Methods

	/// <summary>
	///		@TODO - Move this to another location, Probably the game assets script 
	///		Gets the devices platform
	/// </summary>
	/// <param name="Device"></param>
	/// <returns></returns>
	public DevicePlatform GetDevicePlatform(string Device)
	{
		var device = DevicePlatform.Unknown;


		switch (Device)
		{
			case PC_KeyboardDevice:
			case PC_MouseDevice:
				{ 
					device = DevicePlatform.PC;
				}
				break;
			case Xbox_ControllerDevice:
				{
					device = DevicePlatform.Xbox;
				}
				break;
			case PS4_ControllerDevice:
				{
					device = DevicePlatform.Playstation;
				}
				break;
		}


		if (device == DevicePlatform.Unknown)
		{
			Debug.LogWarning("[GameManager.GetDevicePlatform]: " + "Could not find that device, it is unknown!");
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

				Device s_newDevice = new Device
				{
					identity = p_InputDevice.deviceId,
					name = p_InputDevice.displayName,
					platform = GetDevicePlatform(p_InputDevice.displayName),
				};

				m_ConnectedDevices.Add(s_newDevice);
			
				// GameEvents.SetCurrentActivePlayers?.Invoke(1);
				
				break;
				}
			case InputDeviceChange.Removed: // When an input device is removed 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Removed Input Device... " + p_InputDeviceState);

				// GameEvents.SetCurrentActivePlayers?.Invoke(-1);

				for (int i = 0; i < m_ConnectedDevices.Count; i++)
					{
						if (m_ConnectedDevices[i].identity == p_InputDevice.deviceId)
						{
							Device device = m_ConnectedDevices[i];

							m_ConnectedDevices.Remove(device);
						}
					}

				break;
				}
			case InputDeviceChange.Disconnected: // When a controller disconnects 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Disconnected Input Device... " + p_InputDeviceState);

				GameEvents.SetCurrentActivePlayers?.Invoke(-1);

				break;
				}
			case InputDeviceChange.Reconnected: // When a controller reconnects 
				{ 
				Debug.Log("[GameManager.HandleOnControllerInputDeviceChanged]: " + "Reconnected Input Device... " + p_InputDeviceState);

				GameEvents.SetCurrentActivePlayers?.Invoke(1);

				break;
				}
		}
	}


	/// <summary>
	///		Updates the active players using the SetCurrentActivePlayersEvent Called from HandleOnControllerInputDeviceChanged 
	/// </summary>
	/// <param name="UpdatedPlayerCount"></param>
	private void UpdateActivePlayers(int UpdatedPlayerCount) => ActivePlayers += UpdatedPlayerCount; 

	#endregion

}