#region Namespaces
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
#endregion



public class PlayerInputHandler : MonoBehaviour
{
	/// <summary>
	///		Reference to the player input component 
	/// </summary>
	private PlayerInput m_PlayerInput;


	private void Awake()
	{
		m_PlayerInput = GetComponent<PlayerInput>();

		var currentPlayerIndex = m_PlayerInput.playerIndex;

		string s_CameraString = "P" + (currentPlayerIndex + 1) + "_Camera";

		if (GameObject.Find(s_CameraString))
		{
			m_PlayerInput.camera = GameObject.Find(s_CameraString).GetComponent<Camera>();

		}
		
	}
}