#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
#endregion



public class WizardInputControls : MonoBehaviour 
{

	#region Public Variables

	public Rigidbody rb { get; private set; }

	public PlayerInput input { get; private set; }

	#endregion


	#region Private Variables

	private Vector2 m_Movement;

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
	
		if (GetComponent<Rigidbody>())
		{
			rb = GetComponent<Rigidbody>();
		}

		if (GetComponent<PlayerInput>())
		{
			input = GetComponent<PlayerInput>();
		}

	}

	private void Update()
	{
		if (input.enabled)
		{
			rb.AddForce(m_Movement);
		}
	}

	#endregion

	public void SetAllowPlayerInput(bool activate, PlayerInput p_PlayerInput)
	{
		if (input == null)
		{
			input = p_PlayerInput;
		}

		input.enabled = activate;
	}

	public void OnMove(InputAction.CallbackContext context) => m_Movement = context.ReadValue<Vector2>();
}
