#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
#endregion



public class Wizard : MonoBehaviour 
{

	#region Public Variables

	private PlayerInputHandler m_PlayerInputHandler;
	
	#endregion


	#region Private Variables

	/// <summary>
	///		Whether the player should be allowed to move or not 
	/// </summary>
	private bool m_EnablePlayerInput;

	#endregion


	#region Unity References

	/// <summary>
	///		Subscribe to events 
	/// </summary>
	private void OnEnable()
	{
		
	}

	/// <summary>
	///		Unsubscribing to events 
	/// </summary>
	private void OnDisable()
	{
		
	}


	private void Awake()
	{
		m_EnablePlayerInput = false;


	}


	private void FixedUpdate()
	{
		if (!m_EnablePlayerInput)
		{
			return;
		}

		Debug.Log("Player input allowed: " + m_EnablePlayerInput);
	}

	#endregion


	#region Public Methods

	public void EnablePlayerInput(bool ShouldEnablePlayerInput) => m_EnablePlayerInput = ShouldEnablePlayerInput;

	#endregion

	#region Private Methods

	private void AllowPlayerControllerInput()
	{
		EnablePlayerInput(true);
	}

	#endregion

}
