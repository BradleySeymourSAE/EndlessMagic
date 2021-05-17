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
	public int ActivePlayers {  get; private set; } = 0;

	#endregion


	#region Private Variables

	/// <summary>
	///		Reference to the current selection canvas 
	/// </summary>
	[SerializeField] private Canvas m_SelectionCanvas;

	/// <summary>
	///		The selection panel of the canvas 
	/// </summary>
	[SerializeField] private GameObject m_SelectionPanel;

	#endregion


	private void Start()
	{
		var s_JoinPlayerAction = new InputAction(binding: "/*/<button>");

		s_JoinPlayerAction.performed += action =>
		{
			AddGamepad(action.control.device);
		};


		s_JoinPlayerAction.Enable();
	}


	#region Private Methods

	private void AddGamepad(InputDevice device)
	{
		foreach (var s_CurrentPlayer in PlayerInput.all)
		{

			foreach (var s_PlayerDevice in s_CurrentPlayer.devices)
			{
				if (device == s_PlayerDevice)
				{
					return;
				}
			}
		}


		if (!device.displayName.Contains("Controller") &&
			!device.displayName.Contains("Joystick") && 
			!device.displayName.Contains("Gamepad")
		) {
			return;
		}

		var s_CurrentPlayerIndex = PlayerInput.all.Count + 1;

		string s_ActionControlScheme = "KeyboardMouse";

		if (device.displayName.Contains("Controller") || device.displayName.Contains("Gamepad"))
		{
			s_ActionControlScheme = "Gamepad";
		}
		else if (device.displayName.Contains("Joystick"))
		{
			s_ActionControlScheme = "Joystick";
		}


		GameObject s_Player = Resources.Load<GameObject>($"CursorPrefabs/P{s_CurrentPlayerIndex}_Cursor");


		if (!s_Player.activeInHierarchy)
		{
			PlayerInput s_PlayerCursor = PlayerInput.Instantiate(s_Player, -1, s_ActionControlScheme, -1, device);
			s_PlayerCursor.transform.SetParent(m_SelectionPanel.transform);
			s_Player.transform.localScale = new Vector3(1, 1, 1);
		}
	}



	public void OnPlayerJoin(PlayerInput p_Input)
	{
		ActivePlayers = PlayerInput.all.Count;
		Debug.Log("[GamepadJoinGameBehaviour.HandleOnPlayerJoin]: " + "Joined players:  " + ActivePlayers);
	}

	public void OnPlayerLeft(PlayerInput p_Input)
	{
		ActivePlayers = PlayerInput.all.Count;
		Debug.Log("[GamepadJoinGameBehaviour.HandleOnPlayerLeft]: " + "Joined players:  " + ActivePlayers);
	}
	#endregion

}
