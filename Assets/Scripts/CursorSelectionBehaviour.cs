#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion



public class CursorSelectionBehaviour : MonoBehaviour
{

	public bool objectSelected = false;
	public GameObject playerSelection;

	public static EventHandler HandleOnCharacterSelectedEvent;



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
		if (context.phase == InputActionPhase.Started)
		{
			Debug.Log("[CursorSelectionManager.OnSelect]: " + "Ready to join the game -" + context.action.name);
		}
	}

	public void OnStartButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{
			Debug.Log("Start button has been pressed: " + context.action.name);
		}
	}
}