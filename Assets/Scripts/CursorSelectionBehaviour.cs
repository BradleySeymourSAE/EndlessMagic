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
	///		Is the player ready to join? 
	/// </summary>
	public bool isReady = false;

	/// <summary>
	///		Is there a wizard selected? 
	/// </summary>
	public bool wizardSelected = false;

	/// <summary>
	///		Is there a mountable 'broom' selected? 
	/// </summary>
	public bool mountableSelected = false;

	/// <summary>
	///		The currently selected wizard prefab 
	/// </summary>
	public GameObject wizardSelection;

	/// <summary>
	///		The Selectable Wizard Game Objects  
	/// </summary>
	public GameObject[] wizardSelectionChoices;

	/// <summary>
	///		The currently selected mountable 'broom / vehicle' 
	/// </summary>
	public GameObject mountableSelection = null;

	/// <summary>
	///		The Selectable Mountable Vehicle Game Objects 
	/// </summary>
	public GameObject[] mountableSelectionChoices;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Allow Player Join Behaviour 
	/// </summary>
	[SerializeField] private bool allowPlayerJoinBehaviour = false;

	/// <summary>
	///		Allow Character Selection Behaviour 
	/// </summary>
	[SerializeField] private bool allowCharacterSelecting = false;

	/// <summary>
	///		Allow mountable selection behaviour 
	/// </summary>
	[SerializeField] private bool allowMountableSelecting = false;

	/// <summary>
	///		The currently selected wizard index 
	/// </summary>
	[SerializeField] private int m_SelectedWizardIndex = 0;

	/// <summary>
	///		The currently selected mountable index 
	/// </summary>
	[SerializeField] private int m_SelectedMountableIndex = 0;

	/// <summary>
	///		Reference to the current user ID 
	/// </summary>
	[SerializeField] private int m_CurrentUserID;

	/// <summary>
	///		Reference to the current Game Controller Type 
	/// </summary>
	[SerializeField] private GameControllerType m_GameControllerType;

	/// <summary>
	///		Game Controller UI Data Scriptable Object - For Displaying UI 
	/// </summary>
	[SerializeField] private GameControllerUIData m_GameControllerData;

	/// <summary>
	///		Reference to the Character Name Text Field 
	/// </summary>
	private TMP_Text m_CharacterName;

	/// <summary>
	///		Should navigation left and right be disabled? 
	/// </summary>
	private bool disableNavigation = false;

	/// <summary>
	///		Testing - Currently testing to see if i can temporarily add the selected prefabs, as its handled on a single screen 
	/// </summary>
	[SerializeField] private List<GameObject> m_SelectedPrefabs = new List<GameObject>();

	#endregion

	#region Unity References 

	private void OnEnable()
	{
		GameEvents.SetSelectableWizards += SetWizardSelectionChoices;
		GameEvents.SetSelectableMountables += SetMountableSelectionChoices;
	}

	private void OnDisable()
	{
		GameEvents.SetSelectableWizards -= SetWizardSelectionChoices;	
		GameEvents.SetSelectableMountables -= SetMountableSelectionChoices;
	}

	private void Update()
	{
		allowPlayerJoinBehaviour = GameManager.Instance.AllowPlayerJoining;

		// If a wizard has not been selected yet and we need to select a wizard 
		if (!wizardSelected && wizardSelection)
		{
			// Allow Character Selecting 
			allowCharacterSelecting = true;
			wizardSelection = ReturnSelectedWizardCharacter();
		}
		else if (wizardSelected && wizardSelection && !mountableSelected && mountableSelection)
		{ 
			allowCharacterSelecting = false;
			allowMountableSelecting = true;

			mountableSelection = ReturnSelectedMountableVehicle();
		}
		else if (wizardSelected && mountableSelected)
		{
			disableNavigation = true;
			Debug.Log("Both Wizard and Mountable Selected!");
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
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnNextButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
			return;
		}

		if (!allowPlayerJoinBehaviour && !allowMountableSelecting && allowCharacterSelecting)
		{

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


				UpdateCharacterUI(wizardSelectionChoices[m_SelectedWizardIndex]);
				// m_CharacterName.text = ReturnWizardName();
			}
		}
		else if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && allowMountableSelecting)
		{

			if (context.phase == InputActionPhase.Started)
			{
				Debug.Log("[CursorSelectionManager.OnNextButton]: " + "Selecting next mountable vehicle " + context.action.name);
				
				// Disable the current selected mountable index 
				mountableSelectionChoices[m_SelectedMountableIndex].SetActive(false);

				// Set the selected mountable vehicle index to increase by 1, using a percentage of the total mountableSelectionChoices 
				m_SelectedMountableIndex = (m_SelectedMountableIndex + 1) % mountableSelectionChoices.Length;

				// Play the UI Move Sound Effect 
				AudioManager.PlaySound(SoundEffect.GUI_Move);

				// Set the current mountable selection choice as active 
				mountableSelectionChoices[m_SelectedMountableIndex].SetActive(true);

				// Update the character text ui with the mountable name  
				// m_CharacterName.text = ReturnMountableName();
				UpdateMountableUI();
			}
		}
	}

	/// <summary>
	///		Devices Previous Button has been pressed ( DPAD Left / Left Arrow Key ) 
	/// </summary>
	/// <param name="context"></param>
	public void OnPreviousButton(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
		{
			Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
			return;
		}
		
		if (!allowPlayerJoinBehaviour && !allowMountableSelecting && allowCharacterSelecting)
		{

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

				// Play the UI Move Sound Effect 
				AudioManager.PlaySound(SoundEffect.GUI_Move);

				// Return the wizard's character name 
				// m_CharacterName.text = ReturnWizardName();

				UpdateCharacterUI(wizardSelectionChoices[m_SelectedWizardIndex]);
			}
		}
		// Otherwise, if not allowing player join and also not allowing character selecting BUT allowing mountable item selection  
		else if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && allowMountableSelecting)
		{
			// If the context phase is in the `first phase` of the action.. 
			if (context.phase == InputActionPhase.Started)
			{
				Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Selecing previous mountable vehicle " + context.action.name);

				// Set the current mountable selection as inactive 
				mountableSelectionChoices[m_SelectedMountableIndex].SetActive(false);

				// Deduct the index by 1 
				m_SelectedMountableIndex--;

				// If the selected mountable index is less than 0 
				if (m_SelectedMountableIndex < 0)
				{
					// Set the index to the last mountable selection choice in the array 
					m_SelectedMountableIndex += mountableSelectionChoices.Length;
				}

				// Set the index as active 
				mountableSelectionChoices[m_SelectedMountableIndex].SetActive(true);
				
				// Play the audio effect 
				AudioManager.PlaySound(SoundEffect.GUI_Move);

				// Reset the character name text to the mountable vehicles name 
				// m_CharacterName.text = ReturnMountableName();

				UpdateMountableUI();
			}
		}
	}	

	/// <summary>
	///		Devices Select Button has been pressed (A Button) / (Button South) 
	/// </summary>
	/// <param name="context"></param>
	public void OnSelect(InputAction.CallbackContext context)
	{
		if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
		{
			Debug.Log("Player join, Character Selection AND MountableSelecting is false -- Join Behaviour: " + allowPlayerJoinBehaviour + " Character Selecting: " + allowCharacterSelecting + " Allow Mountable Vehicle Selecting: " + allowMountableSelecting);
			return;
		}

		// If allowing player join behaviour and not allowing character selecting, or not allowing mountable selecting 
		if (allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
		{
			// If the first frame of the input action 
			if (context.phase == InputActionPhase.Started)
			{
				// If the player is not ready, then set the player as being ready 
				if (!isReady)
				{
					Debug.Log("[CursorSelectionManager.OnSelect]: " + "Ready to join the game -" + context.action.name);
					GameEvents.SetPlayerReadyEvent?.Invoke(gameObject, 1);

					// Play the Confirm Sound Effect 
					AudioManager.PlaySound(SoundEffect.GUI_Confirm);

					// Set ready to true 
					isReady = true;
				}
				// Otherwise, IF the player is already `Ready`, then set the player as NOT being ready anymore 
				else
				{
					
					Debug.Log("[CursorSelectionManager.OnSelect]: " + "Cancelled ready up - " + context.action.name);
					GameEvents.SetPlayerReadyEvent?.Invoke(gameObject, -1);

					// Play the confirm sound effect 
					AudioManager.PlaySound(SoundEffect.GUI_Confirm);
					
					// Set is ready to false 
					isReady = false;
				}
			}
		}
		// Otherwise, If NOT allowing player join behaviour and NOT allowing mountable selecting but we are allowing character selection 
		else if (!allowPlayerJoinBehaviour && !allowMountableSelecting && allowCharacterSelecting)
		{
			// If the action's phase is in the first frame 
			if (context.phase == InputActionPhase.Started)
			{
				// Return the ready up button asset 
				Button s_ReadyUpButton = GameEntity.FindSceneAsset(m_CurrentUserID, SceneAsset.SelectionUI_ReadyUp).GetComponent<Button>();


				// If a wizard has not been selected yet
				if (!wizardSelected)
				{
					// Then set the wizard as being selected 
					wizardSelected = true;

					Debug.Log("Wizard has been selected!! Wizard Selection: " + ReturnWizardName());

					// Return the currently selected wizard character 
					wizardSelection = ReturnSelectedWizardCharacter();

					// If the selected prefabs list contains the currently selected wizard 
					if (m_SelectedPrefabs.Contains(wizardSelection))
					{
						// Then we just want to return 
						Debug.LogWarning("That wizard has already been selected! - Returning!");
						return;
					}
					
					// Add the wizard Selection to the selected prefabs list 
					m_SelectedPrefabs.Add(wizardSelection);

					// Find the sound effect for the wizard character 
					SoundEffect s_SoundToPlay = ReturnWizardSoundEffect(m_SelectedWizardIndex);
					
					// Play the sound effect 
					AudioManager.PlaySound(s_SoundToPlay); // play the sound effect 

					// Set ready up button as not interactable 
					s_ReadyUpButton.interactable = false;
				
					// Hide the currently selected wizard selection 
					wizardSelection.SetActive(false);

					// Show the currently selected mountable vehicle selection
					mountableSelection.SetActive(true);

					// Set the ready up button to be interactable
					s_ReadyUpButton.interactable = true;
				}
				else 
				{

					Debug.Log("[CursorSelectionBehaviour.OnSelect]: " + "Unselecting wizard -- " + ReturnWizardName());

					// The wizard is unselected 
					wizardSelected = false;

					// If the selected prefabs list contains the wizard selection 
					if (m_SelectedPrefabs.Contains(wizardSelection))
					{
						// Remove the wizard selection game object from the list 
						m_SelectedPrefabs.Remove(wizardSelection);
					}
				}
			}
		}
		// Otherwise, If NOT allowing player join behaviour and NOT allowing character selection BUT we are allowing mountable vehicles selection 
		else if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && allowMountableSelecting)
		{
			// If the input action happens in the first phase 
			if (context.phase == InputActionPhase.Started)
			{
				
				Debug.Log("[CursorSelectionBehaviour.OnSelect]: " + "Mountable has been selected " + mountableSelected);

				// If the mountable vehicle selection is not selected 
				if (!mountableSelected)
				{ 
					Debug.Log("Mountable has now been selected!");

					// Select the mountable item as selected 
					mountableSelected = true;

					// Return the current mountable selection item 
					mountableSelection = ReturnSelectedMountableVehicle();

					// if the selected prefabs contains the mountable selection 
					if (m_SelectedPrefabs.Contains(mountableSelection))
					{
						// just return 
						Debug.Log("Selected Prefabs already contains Mountable Selection!");
						return;
					}
					
					// Add the mountable vehicle selection to the selected prefabs list 
					m_SelectedPrefabs.Add(mountableSelection);

					// Update the mountable character name text 
					m_CharacterName.text = ReturnMountableName();

					// Show the ready screen 
				}
				// Otherwise, if mountable vehicle has been selected 
				else if (mountableSelected)
				{
					Debug.Log("[CursorSelectionBehaviour.OnSelect]: " + "Removed Selected Mountable " + ReturnMountableName());
				
					// Stop selecting the mountable vehicle 
					mountableSelected = false;

					// If the selected prefabs list contains the mountable selection game object 
					if (m_SelectedPrefabs.Contains(mountableSelection))
					{
						// Remove the mountable selection from the game object list 
						m_SelectedPrefabs.Remove(mountableSelection);
					}
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
		if (context.phase == InputActionPhase.Started)
		{ 	
			// if (!allowPlayerJoinBehaviour && allowCharacterSelecting)
			// {
				Debug.Log("[CursorSelectionManager.OnStartButton]: " + "Allow character selecting! " + context.action.name);
			// }
		}
	}

	public void OnBackButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{ 
			if (allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
			{

				Debug.Log("[CursorSelectionManager.OnBackButton]: " + "Back button pressed - Returning to main menu! " + context.action.name);

				if (GameUIManager.Instance)
				{
					GameUIManager.Instance.PlayerJoinMenuUI.ReturnToMainMenu();
				}
			}
		}
	}

	#endregion

	#region Public Methods 

	/// <summary>
	///		Getter / Setter for the Game Controller Type 
	/// </summary>
	public GameControllerType Controller
	{
		get
		{
			return m_GameControllerType;
		}
		private set
		{
			m_GameControllerType = value;
		}
	}

	/// <summary>
	///		Getter / Setter for the Game Controller UI 
	/// </summary>
	public GameControllerUIData ControllerUI
	{
		get
		{
			return m_GameControllerData;
		}
		private set
		{
			m_GameControllerData = value;
		}
	}

	/// <summary>
	///		Sets the current cursor identity to the playerIndex 
	/// </summary>
	/// <param name="Identity"></param>
	public void SetCursorIdentity(int Identity = 0) => m_CurrentUserID = Identity; 

	/// <summary>
	///		Sets the current cursor game controller type (Mostly used for UI, REALLLLY helpful) 
	/// </summary>
	/// <param name="p_ControllerType"></param>
	public void SetGameControllerType(GameControllerType p_ControllerType) => Controller = p_ControllerType;

	public void SetGameControllerUIData(GameControllerUIData p_GameControllerData) => ControllerUI = p_GameControllerData;

	/// <summary>
	///		Returns the currently selected wizard character game object 
	/// </summary>
	/// <returns></returns>
	public GameObject ReturnSelectedWizardCharacter() => wizardSelectionChoices[m_SelectedWizardIndex];

	public GameObject ReturnSelectedMountableVehicle() => mountableSelectionChoices[m_SelectedMountableIndex];

	/// <summary>
	///		Gets the wizard name 
	/// </summary>
	/// <returns></returns>
	public string ReturnWizardName()
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

	public string ReturnMountableName()
	{
		switch (m_SelectedMountableIndex)
		{
			case 0:
				return "The Holy Bathtub";
			case 1:
				return "The Hanger of Coats";
			case 2:
				return "The Nimbruh";
			case 3:
				return "Hagrids Special";
			case 4:
				return "The Dombra-kin";
			case 5:
				return "The Master Broom";
			default:
				return null;
		}
	}

	public void UpdateCharacterUI(GameObject p_WizardSelection)
	{
		if (p_WizardSelection.GetComponent<CharacterStatsUI>() != null)
		{
			p_WizardSelection.GetComponent<CharacterStatsUI>().DisplayCharacterUI();
		}
	}

	public void UpdateMountableUI()
	{

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

		if (wizardSelection.GetComponent<CharacterStatsUI>() != null)
		{
			UpdateCharacterUI(wizardSelection);
		}

		// Set the character title text to the current selected prefab's name 
		GameObject title = GameEntity.FindByTag(GameEntity.GetCharacterSelectionTitle(m_CurrentUserID));

		m_CharacterName = title.GetComponentInChildren<TMP_Text>();
		m_CharacterName.text = ReturnWizardName();
	}

	private void SetMountableSelectionChoices(int p_CurrentCursorIdentity, List<Transform> p_MountableSelectionsSpawned)
	{
		if (p_CurrentCursorIdentity != m_CurrentUserID)
		{
			return;
		}

		mountableSelectionChoices = new GameObject[p_MountableSelectionsSpawned.Count];

		for (int i = 0; i < p_MountableSelectionsSpawned.Count; i++)
		{
			p_MountableSelectionsSpawned[i].gameObject.SetActive(false);

			mountableSelectionChoices[i] = p_MountableSelectionsSpawned[i].gameObject;
		}


		// Set the player selection to the selected index (Which enables `allowMountableSelection` to true (Receiving input))
		mountableSelection = ReturnSelectedMountableVehicle();

	

		mountableSelected = false;
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