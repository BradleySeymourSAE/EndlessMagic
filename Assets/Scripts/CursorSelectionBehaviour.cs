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

	[SerializeField] private bool allowPlayerJoinBehaviour = false;

	[SerializeField] private bool allowCharacterSelecting = false;

	[SerializeField] private int m_SelectedWizardIndex = 0;

	[SerializeField] private int m_CurrentUserID;

	[SerializeField] private string m_CurrentControllerType;

	private TMP_Text m_CharacterName;

	private bool disableNavigation = false;

	public List<string> Controls
	{
		get
		{
			return m_DeviceControls;
		}
		set
		{
			m_DeviceControls = value;
		}
	}

	[SerializeField] private List<string> m_DeviceControls = new List<string>();

	#region Unity References 

	private void OnEnable()
	{
		GameEvents.SetSelectableWizards += SetWizardSelectionChoices;
	}

	private void OnDisable()
	{
		GameEvents.SetSelectableWizards -= SetWizardSelectionChoices;	
	}

	private void Awake()
	{
		m_DeviceControls.Clear();
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

	#endregion

	#region Input System Events 

	/// <summary>
	///		Devices Next Button has been pressed ( DPAG Right / Right Arrow Key ) 
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

				AudioManager.PlaySound(SoundEffect.GUI_Move);
				// Set the current wizard selection choice as active 
				wizardSelectionChoices[m_SelectedWizardIndex].SetActive(true);

			

				m_CharacterName.text = GetWizard();
			}
		}
	}

	/// <summary>
	///		Devices Previous Button has been pressed ( DPAD Left / Left Arrow Key ) 
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

				AudioManager.PlaySound(SoundEffect.GUI_Move);

				m_CharacterName.text = GetWizard();
			}
		}
	}	

	/// <summary>
	///		Devices Select Button has been pressed (A Button) / (Button South) 
	/// </summary>
	/// <param name="context"></param>
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

					AudioManager.PlaySound(SoundEffect.GUI_Confirm);

					isReady = true;
				}
				else
				{
					Debug.Log("[CursorSelectionManager.OnSelect]: " + "Cancelled ready up - " + context.action.name);
					GameEvents.SetPlayerReadyEvent?.Invoke(gameObject, -1);

					AudioManager.PlaySound(SoundEffect.GUI_Confirm);
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
					// s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ready!";


					// Technically now, we will just be setting the ready button 


					Debug.Log("Selected wizard " + GetWizard());
				}
				else if (objectSelected)
				{
					objectSelected = false;
					Debug.Log("Unselected wizard " + GetWizard());

					InputSystem.GetDeviceById(m_CurrentUserID).device.MakeCurrent();

				
					var buttonText = Gamepad.current.buttonSouth.displayName;

					buttonText[0].ToString().ToUpper();
		
					Debug.Log("Gamepad Select Button: " + buttonText);

					// For some reason its not getting this as the current gamepad -_- ughhhh



					if (buttonText != null)
					{
						s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ok";

						// If the device is a ps4 device 
						if (buttonText.Contains("Cross"))
						{
							s_ReadyUpButton.transform.GetChild(1).GetComponent<Image>().sprite = GameEntity.FindAsset(ResourceFolder.ControllerInputIcons, Asset.PS4, buttonText);
						}
						// Otherwise, if the device is an xbox controller 
						else if (buttonText.Contains("A"))
						{
							s_ReadyUpButton.transform.GetChild(1).GetComponent<Image>().sprite = GameEntity.FindAsset(ResourceFolder.ControllerInputIcons, Asset.Xbox, buttonText);
						}
					}



					s_ReadyUpButton.interactable = true;
					// s_ReadyUpButton.GetComponentInChildren<Text>().text = "Ready up";

				}
			}
		}
	}

	/// <summary>
	///		Devices Start Button has been pressed 
	/// </summary>
	/// <param name="context"></param>
	public void OnStartButton(InputAction.CallbackContext context)
	{
		
		if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnStartButton]: " + "Allow character selecting! " + context.action.name);
		}

	}

	public void OnBackButton(InputAction.CallbackContext context)
	{
		if (allowPlayerJoinBehaviour && !allowCharacterSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnBackButton]: " + "Back button pressed - Returning to main menu! " + context.action.name);

			if (GameUIManager.Instance)
			{
				GameUIManager.Instance.PlayerJoinMenuUI.ReturnToMainMenu();
			}
		}
	}

	#endregion


	#region Public Methods 

	/// <summary>
	///		Sets the current cursor identity to the playerIndex 
	/// </summary>
	/// <param name="Identity"></param>
	public void SetCursorIdentity(int Identity = 0) => m_CurrentUserID = Identity; 

	/// <summary>
	///		Sets the controllers type 
	/// </summary>
	/// <param name="type"></param>
	public void SetControllerType(string type = "") => m_CurrentControllerType = type;
	/// <summary>
	///		Returns the currently selected wizard character game object 
	/// </summary>
	/// <returns></returns>
	public GameObject ReturnSelectedWizardCharacter() => wizardSelectionChoices[m_SelectedWizardIndex];

	/// <summary>
	///		Gets the wizard name 
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

	#endregion

	#region Private Methods

	/// <summary>
	///		Sets the wizard selection choices - Called from SetSelectableWizards event 
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