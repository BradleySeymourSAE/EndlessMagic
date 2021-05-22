#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
#endregion


/// <summary>
///		Handles selection of characters 
/// </summary>
public class CursorSelectionBehaviour : MonoBehaviour
{

	public bool objectSelected = false;
	public bool isReady = false;

	public GameObject wizardSelection;

	public GameObject[] wizardSelectionChoices;

	public static EventHandler HandleOnCharacterSelectedEvent;

	[SerializeField] private bool allowPlayerJoinBehaviour = false;

	[SerializeField] private bool allowCharacterSelecting = false;

	[SerializeField] private int m_SelectedWizardIndex = 0;

	[SerializeField] private int m_CurrentUserID;

	private TMP_Text m_CharacterName;

	private bool disableNavigation = false;
	
	private void OnEnable()
	{
		GameEvents.SetSelectableWizards += SetWizardSelectionChoices;
	}

	private void OnDisable()
	{
		GameEvents.SetSelectableWizards -= SetWizardSelectionChoices;	
	}


	private void Update()
	{
		allowPlayerJoinBehaviour = GameManager.Instance.AllowPlayerJoining;


		if (wizardSelection != null)
		{
			allowCharacterSelecting = true;
		}

		if (objectSelected)
		{
			wizardSelection = ReturnSelectedWizardCharacter();
			disableNavigation = true;
		}
		else
		{
			disableNavigation = false;
		}
	}

	public void OnNextButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnNextButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
			return;
		}

		if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{

			if (disableNavigation)
			{
				return;
			}

			if (context.phase == InputActionPhase.Started)
			{
				Debug.Log("[CursorSelectionManager.OnNextButton]: " + "Selecing next item " + context.action.name);
				
				// Disable the current selected wizard index 
				wizardSelectionChoices[m_SelectedWizardIndex].SetActive(false);

				// Set the selected wizard index to increase by 1, using a percetage of the total wizardSelectionChoices
				m_SelectedWizardIndex = (m_SelectedWizardIndex + 1) % wizardSelectionChoices.Length;

				// Set the current wizard selection choice as active 
				wizardSelectionChoices[m_SelectedWizardIndex].SetActive(true);

				m_CharacterName.text = GetWizard();
			}
		}
	}

	public void OnPreviousButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
			return;
		}
		
		if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{
			if (disableNavigation)
			{
				return;
			}

			if (context.phase == InputActionPhase.Started)
			{
				Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Selecing previous item " + context.action.name);
				
				// Set the current wizard selection as inactive 
				wizardSelectionChoices[m_SelectedWizardIndex].SetActive(false);
				
				// Deduct the index by 1 
				m_SelectedWizardIndex--;

				// If the selected index is less than 0 
				if (m_SelectedWizardIndex < 0)
				{
					// Set the index to the last wizard selection choice in the array 
					m_SelectedWizardIndex += wizardSelectionChoices.Length;
				}

				// Set the index as active 
				wizardSelectionChoices[m_SelectedWizardIndex].SetActive(true);

				m_CharacterName.text = GetWizard();
			}
		}
	}	

	public void OnSelect(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("Both player join & character selecting is false. Join Behaviour: " + allowPlayerJoinBehaviour + " Character Selecting: " + allowCharacterSelecting);
			return;
		}

		if (allowPlayerJoinBehaviour && !allowCharacterSelecting)
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
				}
			}
		}
		else if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{
			if (context.phase == InputActionPhase.Started)
			{

				wizardSelection = ReturnSelectedWizardCharacter();

				Button s_ReadyUpButton = GameEntity.FindSceneAsset(m_CurrentUserID, SceneAsset.SelectionUI_ReadyUp).GetComponent<Button>();


				if (!objectSelected)
				{
					objectSelected = true;
					wizardSelection = ReturnSelectedWizardCharacter();
					
				
					SoundEffect s_SoundToPlay = ReturnWizardSoundEffect(m_SelectedWizardIndex); // Find the sound effect to play 

					AudioManager.PlaySound(s_SoundToPlay); // play the sound effect 

					s_ReadyUpButton.interactable = false;
					s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ready!";

					Debug.Log("Selected wizard " + GetWizard());
				}
				else if (objectSelected)
				{
					objectSelected = false;
					Debug.Log("Unselected wizard " + GetWizard());

					s_ReadyUpButton.interactable = true;
					s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ready up";

				}
			}
		}
	}

	public void OnStartButton(InputAction.CallbackContext context)
	{
		
		if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnStartButton]: " + "Allow character selecting! " + context.action.name);
		}

	}

	public void SetCursorIdentity(int Identity = 0) => m_CurrentUserID = Identity; 

	public GameObject ReturnSelectedWizardCharacter() => wizardSelectionChoices[m_SelectedWizardIndex];

	#region Private Methods

	private void SetWizardSelectionChoices(int p_CurrentCursorIdentity, List<Transform> p_WizardSelectionsSpawned)
	{

		if (p_CurrentCursorIdentity != m_CurrentUserID)
		{
			return;
		}

		wizardSelectionChoices = new GameObject[p_WizardSelectionsSpawned.Count];
	
			for (int i = 0; i < p_WizardSelectionsSpawned.Count; i++)
			{
				
				p_WizardSelectionsSpawned[i].gameObject.SetActive(false);

				wizardSelectionChoices[i] = p_WizardSelectionsSpawned[i].gameObject;
				
			}

		// Set the first indexed wizard selection choice as the active wizard selection 
		wizardSelectionChoices[m_SelectedWizardIndex].SetActive(true);

		// Set the player selection to the selected index (Enables `allowCharacterSelection` to true (Receiving input))
		wizardSelection = ReturnSelectedWizardCharacter();

		// Set the character title text to the current selected prefab's name 
		GameObject title = GameEntity.FindByTag(GameEntity.GetWizardCharacterTitleTag(m_CurrentUserID));

		m_CharacterName = title.GetComponentInChildren<TMP_Text>();
		m_CharacterName.text = GetWizard();
	}

	public string GetWizard()
	{
		switch (m_SelectedWizardIndex)
		{
			case 0:
				return "Draco Malfoy";
			case 1:
				return "Hermione Granger";
			case 2:
				return "Sirius Black";
			case 3:
				return "Severus Snape";
			case 4:
				return "Yennefer";
			case 5:
				return "Voldemort";
			default:
				return null;
		}
	}
	
	/// <summary>
	///		Returns the wizard sound effect to play - should probably move this to a different folder but this 
	///		is just due to my poor organisation of code. Will fix it up eventually 
	/// </summary>
	/// <param name="p_WizardIndex"></param>
	/// <returns></returns>
	private SoundEffect ReturnWizardSoundEffect(int p_WizardIndex)
	{
		switch (p_WizardIndex)
		{
			case 0:
				return SoundEffect.Draco_Selected;
			case 1:
				return SoundEffect.Hermione_Selected;
			case 2:
				return SoundEffect.SiriusBlack_Selected;
			case 3:
				return SoundEffect.Snape_Selected;
			case 4:
				return SoundEffect.Yennefer_Selected;
			case 5: 
				return SoundEffect.Voldemort_Selected;
			default:
				return (SoundEffect)p_WizardIndex;
		}
	}
	#endregion
}