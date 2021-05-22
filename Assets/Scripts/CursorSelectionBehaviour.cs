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

	#region Public Variables 

	/// <summary>
	///		Has a wizard character been selected? 
	/// </summary>
	public bool hasSelectedWizardCharacter = false;

	/// <summary>
	///		In the co-op screen - has the player ready'd up 
	/// </summary>
	public bool isReady = false;

	/// <summary>
	///		The selected wizard - or null 
	/// </summary>
	public GameObject wizardSelection;

	/// <summary>
	///		An array of selectable wizards 
	/// </summary>
	public GameObject[] wizardSelectionChoices;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Is the player allowed to join the game? 
	/// </summary>
	[SerializeField] private bool allowPlayerJoinBehaviour = false;

	/// <summary>
	///		Allow character selection (When on the character selection screen) 
	/// </summary>
	[SerializeField] private bool allowCharacterSelecting = false;

	/// <summary>
	///		The current selected wizard index 
	/// </summary>
	[SerializeField] private int m_SelectedWizardIndex = 0;

	/// <summary>
	///		The current controller ID (For each cursor / user)
	/// </summary>
	[SerializeField] private int m_CurrentUserID;

	/// <summary>
	///		Reference to the character name text UI 
	/// </summary>
	private TMP_Text m_CharacterName;

	/// <summary>
	///		Is navigation disabled, if a wizard is selected this is set to true, otherwise false
	/// </summary>
	private bool navigationDisabled = false;

	#endregion

	#region Unity References 

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

		if (hasSelectedWizardCharacter)
		{
			wizardSelection = ReturnSelectedWizardCharacter();
			navigationDisabled = true;
		}
		else
		{
			navigationDisabled = false;
		}
	}

	#endregion

	#region Input Action Events 

	/// <summary>
	///		Cycles through the next character in the wizard selection index 
	/// </summary>
	/// <param name="context"></param>
	public void OnNextButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnNextButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
			return;
		}

		if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{

			if (navigationDisabled)
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

	/// <summary>
	///		Cycles through the previous character in the wizard selection index 
	/// </summary>
	/// <param name="context"></param>
	public void OnPreviousButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
			return;
		}
		
		if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{
			if (navigationDisabled)
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

	/// <summary>
	///		OnSelect 
	///		- If allow player join behaviour is set to true, handles joining the game and readying up. 
	///		- If Allow Character Selection is set to true, handles setting an unsetting the selected wizard character 
	/// </summary>
	/// <param name="context"></param>
	public void OnSelect(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("Both player join & character selecting is false. Join Behaviour: " + allowPlayerJoinBehaviour + " Character Selecting: " + allowCharacterSelecting);
			return;
		}

		// Cooperation Screen - Allowing the controller to 'Ready up' 
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
		// Character Selection Screen - Allow characters to be selected 
		else if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{
			if (context.phase == InputActionPhase.Started)
			{
				// Returns teh currently selected wizard character 
				wizardSelection = ReturnSelectedWizardCharacter();

				// Reference to the ready up button for the player's cursor  
				Button s_ReadyUpButton = GameEntity.FindSceneAsset(m_CurrentUserID, SceneAsset.SelectionUI_ReadyUp).GetComponent<Button>();

				// Is there a wizard character selected? 
				if (!hasSelectedWizardCharacter)
				{
					hasSelectedWizardCharacter = true;
					wizardSelection = ReturnSelectedWizardCharacter();
					
				
					SoundEffect s_SoundToPlay = ReturnWizardSoundEffect(m_SelectedWizardIndex); // Find the sound effect to play 

					AudioManager.PlaySound(s_SoundToPlay); // play the sound effect 

					s_ReadyUpButton.interactable = false; // sets the button as not interactable (Fades) 
					s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ready!";

					// Could also do some lighting particle effects here

					// This is where we will add the currently selected wizard to the GameManager.Instance.SelectedCharacters list (@TODO) 



					Debug.Log("Selected wizard " + GetWizard());
				}
				else if (hasSelectedWizardCharacter)
				{
					hasSelectedWizardCharacter = false;
					Debug.Log("Unselected wizard " + GetWizard());

					// This is where you would remove the currently selected wizard from the GameManager.SelectedCharacters list (@TODO)

					s_ReadyUpButton.interactable = true;
					s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ready up";

				}
			}
		}
	}

	/// <summary>
	///		OnStart - If all players have selected their character in the list of (@TODO) GameManager.Instance.SelectedCharacters
	///		This will be used to invoke the StartGame event (Which will be handled from the GameManager instance. 
	/// </summary>
	/// <param name="context"></param>
	public void OnStartButton(InputAction.CallbackContext context)
	{

		if (!hasSelectedWizardCharacter && !navigationDisabled)
		{

			Debug.Log("[CursorSelectionManager.OnStartButton]: " + "Please select a character first, navigation needs to be disabled." + context.action.name);
			return;
		}

		// If we have an object selected and navigation is disabled
		// We can invoke the done selecting event - Ofcourse this will not fire unless all players have 
		// selected their characters 
		if (hasSelectedWizardCharacter && navigationDisabled)
		{
			if (context.phase == InputActionPhase.Started)
			{ 
				Debug.Log("[CursorSelectionManager.OnStartButton]: " + "Player pressed start button... - User: " + m_CurrentUserID + " Character: " + GetWizard());
			}
		}
	}

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets the current players identity - This could also be improved but it works 
	/// </summary>
	/// <param name="Identity"></param>
	public void SetCursorIdentity(int Identity = 0) => m_CurrentUserID = Identity;

	/// <summary>
	///		Really dodgy way of returning the wizard's name 
	/// </summary>
	/// <returns></returns>
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
	///		Returns the currently selected wizard character 
	/// </summary>
	/// <returns></returns>
	public GameObject ReturnSelectedWizardCharacter() => wizardSelectionChoices[m_SelectedWizardIndex];

	#endregion

	#region Private Methods

	/// <summary>
	///		Sets the wizard selection options for this particular controller 
	/// </summary>
	/// <param name="p_CurrentCursorIdentity"></param>
	/// <param name="p_WizardSelectionsSpawned"></param>
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