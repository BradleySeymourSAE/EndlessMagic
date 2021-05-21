#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion


/// <summary>
///		Handles selection of characters 
/// </summary>
public class CursorSelectionBehaviour : MonoBehaviour
{

	public bool objectSelected = false;
	public bool isReady = false;

	public GameObject playerSelection;

	public GameObject[] wizardSelectionChoices;

	public static EventHandler HandleOnCharacterSelectedEvent;

	private bool allowPlayerJoinBehaviour = false;

	private bool allowCharacterSelecting = false;

	private int m_SelectedWizardIndex = 0;

	private void Update()
	{
		allowPlayerJoinBehaviour = GameManager.Instance.AllowPlayerJoining;

		if (GameManager.Instance)
		{
			allowPlayerJoinBehaviour = GameManager.Instance.AllowPlayerJoining;
			allowCharacterSelecting = GameManager.Instance.AllowCharacterSelecting;
		}

	}

	public void OnNextButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour)
		{
			Debug.Log("On Next Button Called - Allow Player join behaviour is false!");
			return;
		}
		else
		{ 
			if (context.phase == InputActionPhase.Started)
			{
				Debug.Log("[CursorSelectionManager.OnNextButton]: " + context.action.name + " event called!");
			}
		}
	}

	public void OnPreviousButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour)
		{
			Debug.Log("On Previous Button Called - Allow Player join behaviour is false!");
			return;
		}
		else
		{ 
			if (context.phase == InputActionPhase.Started)
			{
				Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + context.action.name + " event called!");
			}
		}
	}	

	public void OnSelect(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour)
		{
			Debug.Log("On Next Button Called - Allow Player join behaviour is false!");
			return;
		}
		else
		{ 
			if (context.phase == InputActionPhase.Started)
			{
				if (!isReady)
				{
					Debug.Log("[CursorSelectionManager.OnSelect]: " + "Ready to join the game -" + context.action.name);
					GameEvents.SetPlayerReadyEvent?.Invoke(gameObject, 1);
					
					isReady = true;
				}
				else
				{
					Debug.Log("[CursorSelectionManager.OnSelect]: " + "Cancelled ready up - " + context.action.name);
					GameEvents.SetPlayerReadyEvent?.Invoke(gameObject, -1);
					isReady = false;
					// DO nothing
				}
			}
		}
	}

	public void OnStartButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour)
		{
			Debug.Log("On Start Button - Allow Player join behaviour is false!");
			return;
		}
		else
		{ 
			if (context.phase == InputActionPhase.Started)
				{
					Debug.Log("Start button has been pressed: " + context.action.name);
				}
			}
	}



	public GameObject ReturnSelectedWizardCharacter() => wizardSelectionChoices[m_SelectedWizardIndex];


}