#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion



public class CursorSelectionBehaviour : MonoBehaviour
{

	private float m_ScreenViewEdgeThreshold = 0.02f;

	public bool objectSelected = false;
	public GameObject playerSelection;

	public static EventHandler HandleOnCharacterSelectedEvent;

	private void Update()
	{
		
	}

	public void OnNextButton(InputAction.CallbackContext context)
	{
		Debug.Log("[CursorSelectionManager.OnNextButton]: " + context);
	}

	public void OnPreviousButton(InputAction.CallbackContext context)
	{
		Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + context);
	}	

	public void OnSelect(InputAction.CallbackContext context)
	{
		Debug.Log("[CursorSelectionManager.OnSelect]: " + context);
	}

	public void OnStartButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{
			HandleOnCharacterSelectedEvent?.Invoke(this, EventArgs.Empty);
		}
	}
}