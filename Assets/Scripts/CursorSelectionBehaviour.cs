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
			Debug.Log("[CursorSelectionBehaviour.Update]: " + "Both Wizard and mountable have been selected!");
		}

	}

	#endregion

	#region Input System Events 

	/// <summary>
	///		Devices Next Button has been pressed ( DPAD Right / Right Arrow Key ) 
	/// </summary>
	/// <param name="context"></param>
	public void OnNextButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{ 
			if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
			{
				Debug.LogWarning("[CursorSelectionManager.OnNextButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
				return;
			}

			if (!allowPlayerJoinBehaviour && !allowMountableSelecting && allowCharacterSelecting)
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
			}
			else if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && allowMountableSelecting)
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

					// Updates the mountable vehicle UI 
					UpdateMountableUI(mountableSelectionChoices[m_SelectedMountableIndex]);
			}
		}
	}

	/// <summary>
	///		Devices Previous Button has been pressed ( DPAD Left / Left Arrow Key ) 
	/// </summary>
	/// <param name="context"></param>
	public void OnPreviousButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{ 

			if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
			{
				Debug.LogWarning("[CursorSelectionManager.OnPreviousButton]: " + "Allow play join behaviour " + allowPlayerJoinBehaviour + " Allow Character Selecting: " + allowCharacterSelecting);
				return;
			}
		
		if (!allowPlayerJoinBehaviour && !allowMountableSelecting && allowCharacterSelecting)
		{
				Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Selecting previous wizard " + context.action.name);
				
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

				// Update the character UI 
				UpdateCharacterUI(wizardSelectionChoices[m_SelectedWizardIndex]);
		}
		// Otherwise, if not allowing player join and also not allowing character selecting BUT allowing mountable item selection  
		else if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && allowMountableSelecting)
		{
				Debug.Log("[CursorSelectionManager.OnPreviousButton]: " + "Selecing previous mountable " + context.action.name);

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

				UpdateMountableUI(mountableSelectionChoices[m_SelectedMountableIndex]);
		}
	}
}	

	/// <summary>
	///		Devices Select Button has been pressed (A Button) / (Button South) 
	/// </summary>
	/// <param name="context"></param>
	public void OnSelect(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{ 
			if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
			{
				Debug.LogWarning(
					"[CursorSelectionBehaviour.OnSelect]: " + "Allow Player Join Behaviour: " + allowPlayerJoinBehaviour + 
					" Allow Character Selecting: " + allowCharacterSelecting + 
					" Allow Mountable Vehicle Selecting: " + allowMountableSelecting
					);
				return;
			}

		// If allowing player join behaviour and not allowing character selecting, or not allowing mountable selecting 
		if (allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
		{
				if (!isReady)
				{
					Debug.Log("[CursorSelectionBehaviour.OnSelect]: " + "Setting ready up status.");
					HandleReadyUp(gameObject, 1, true);
				}
				else
				{
					Debug.Log("[CursorSelectionBehaviour.OnSelect]: " + "Cancelling ready up status.");
					HandleReadyUp(gameObject, -1, false);
				}
		}
		// Otherwise, If NOT allowing player join behaviour and NOT allowing mountable selecting but we are allowing character selection 
		else if (!allowPlayerJoinBehaviour && !allowMountableSelecting && allowCharacterSelecting)
		{
			HandleSelectCharacter();
		}
		// Otherwise, If NOT allowing player join behaviour and NOT allowing character selection BUT we are allowing mountable vehicles selection 
		else if (!allowPlayerJoinBehaviour && !allowCharacterSelecting && allowMountableSelecting)
		{
			HandleSelectMountable();
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
			Debug.Log("[CursorSelectionManager.OnStartButton]: " + "Start button has been pressed: " + context.action.name);
		}
	}

	/// <summary>
	///		Input System Event - Back
	///		- If allow player join behaviour, returns everyone to the main menu 
	/// </summary>
	/// <param name="context"></param>
	public void OnBackButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{ 
			if (allowPlayerJoinBehaviour && !allowCharacterSelecting && !allowMountableSelecting)
			{
				// Returns to main menu 
				Debug.Log("[CursorSelectionManager.OnBackButton]: " + "Back button has been pressed - Returning to main menu " + context.action.name);
				HandleOnBackToMainMenu();
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
	///		Sets a reference to the current cursor playerIndex identity (Controller) 
	/// </summary>
	/// <param name="Identity"></param>
	public void SetCursorIdentity(int Identity) => m_CurrentUserID = Identity; 

	/// <summary>
	///		Sets the current cursor game controller type (Mostly used for UI, REALLLLY helpful) 
	///		@TODO - This is probably unnessesary now, Will look into cleaning this up 
	/// </summary>
	/// <param name="p_ControllerType"></param>
	public void SetGameControllerType(GameControllerType p_ControllerType) => Controller = p_ControllerType;

	/// <summary>
	///		Sets a local reference to the current game controller's UI 
	/// </summary>
	/// <param name="p_GameControllerData"></param>
	public void SetGameControllerUIData(GameControllerUIData p_GameControllerData) => ControllerUI = p_GameControllerData;

	/// <summary>
	///		Returns the currently visible wizard character game object selection choice
	/// </summary>
	/// <returns></returns>
	public GameObject ReturnSelectedWizardCharacter() => wizardSelectionChoices[m_SelectedWizardIndex];

	/// <summary>
	///		Returns the currently visible mountable game object selection choice
	/// </summary>
	/// <returns></returns>
	public GameObject ReturnSelectedMountableVehicle() => mountableSelectionChoices[m_SelectedMountableIndex];


	/// <summary>
	///		Returns the wizard name based on the selected wizard index 
	///		@TODO - I will get around to cleaning up this function and the ReturnMountableName() 
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

	/// <summary>
	///		Returns the Mountable Vehicle's Name
	///		@TODO - I will get around to cleaning up this function and the ReturnWizardName() 
	/// </summary>
	/// <returns></returns>
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

	/// <summary>
	///		Updates the Character UI 
	/// </summary>
	/// <param name="p_WizardSelection"></param>
	public void UpdateCharacterUI(GameObject p_WizardSelection)
	{
		Debug.Log("[CursorSelectionBehaviour.UpdateCharacterUI]: " + "Updating Character Stats UI for " + ReturnWizardName());

		if (p_WizardSelection.GetComponent<CharacterStatsUI>() != null)
		{
			p_WizardSelection.GetComponent<CharacterStatsUI>().DisplayCharacterUI();
		}
	}

	/// <summary>
	///		Updates the mountable UI - Unsure whether we are doing this just yet, so this is really doing nothing right now 
	/// </summary>
	/// <param name="p_MountableSelection"></param>
	public void UpdateMountableUI(GameObject p_MountableSelection)
	{
		Debug.Log("[CursorSelectionBehaviour.UpdateMountableUI]: " + "@TODO - Updating Mountable Vehicle Stats UI for " + ReturnMountableName());

		if (p_MountableSelection.GetComponent<CharacterStatsUI>() != null)
		{
			// p_MountableSelection.GetComponent<CharacterStatsUI>().DisplayMountableUI();
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	///		Handles readying up the current player (cursor) 
	/// </summary>
	/// <param name="p_ReadyPlayer">The player's (cursor) to ready up</param>
	/// <param name="p_SetReady">Adding or removing the player from the currently ready players</param>
	/// <param name="p_IsPlayerReady">Is the player ready?</param>
	private void HandleReadyUp(GameObject p_ReadyPlayer, int p_SetReady, bool p_IsPlayerReady)
	{
		// Play the GUI Confirmation Sound Effect 
		AudioManager.PlaySound(SoundEffect.GUI_Confirm);

		Debug.Log("[CursorSelectionBehaviour.HandleReadyUp]: " + (p_SetReady == 1 ? "Adding" : "Removing") + " player " + p_ReadyPlayer.name.ToString() + ". Player is ready: " + p_IsPlayerReady);

		//	Invokes the ready up event 
		GameEvents.SetPlayerReadyEvent?.Invoke(p_ReadyPlayer, p_SetReady);

		// Set's whether the current player has ready'd up or not. 
		isReady = p_IsPlayerReady;
	}

	/// <summary>
	///		Handles selection of a mountable vehicle 
	/// </summary>
	private void HandleSelectMountable()
	{
		// If the mountable vehicle selection is not selected 
		if (!mountableSelected)
		{
			Debug.Log("[CursorSelectionBehaviour.HandleSelectMountable]: " + "Mountable " + ReturnMountableName() + " has been selected!");

			// Select the mountable item as selected 
			mountableSelected = true;

			// Return the current mountable selection item 
			mountableSelection = ReturnSelectedMountableVehicle();

			// if the selected prefabs contains the mountable selection 
			if (m_SelectedPrefabs.Contains(mountableSelection))
			{
				// just return 
				Debug.LogWarning("[CursorSelectionBehaviour.HandleSelectMountable]: " + "The selected mountable prefabs already contains this game object -- Returning.");
				return;
			}

			// Add the mountable vehicle selection to the selected prefabs list 
			m_SelectedPrefabs.Add(mountableSelection);

			// Update the mountable character name text 
			m_CharacterName.text = ReturnMountableName();

			// Show the ready screen
			Debug.Log("[CursorSelectionBehaviour.HandleSelectMountable]: " + "@TODO - Display `Ready` screen...");

		}
		// Otherwise, if mountable vehicle has been selected 
		else if (mountableSelected)
		{
			Debug.Log("[CursorSelectionBehaviour.HandleSelectMountable]: " + "Removed Selected Mountable " + ReturnMountableName());

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

	/// <summary>
	///		Handles the logic for selecting a character 
	/// </summary>
	private void HandleSelectCharacter()
	{
		// Return the ready up button asset 
		Button s_ReadyUpButton = GameEntity.FindSceneAsset(m_CurrentUserID, SceneAsset.SelectionUI_ReadyUp).GetComponent<Button>();


		// If a wizard has not been selected yet
		if (!wizardSelected)
		{
			// Then set the wizard as being selected 
			wizardSelected = true;

			Debug.Log("[CursorSelectionBehaviour.HandleSelectCharacter]: " + "Wizard " + ReturnWizardName() + " has been selected");

			// Return the currently selected wizard character 
			wizardSelection = ReturnSelectedWizardCharacter();

			// If the selected prefabs list contains the currently selected wizard 
			if (m_SelectedPrefabs.Contains(wizardSelection))
			{
				// Then we just want to return 
				Debug.LogWarning("[CursorSelectionBehaviour.HandleSelectCharacter]: " + "That wizard has already been selected! - Returning!");
				return;
			}

			// Add the wizard Selection to the selected prefabs list 
			m_SelectedPrefabs.Add(wizardSelection);

			// Play the sound effect 
			AudioManager.PlaySound(ReturnWizardSoundEffect(m_SelectedWizardIndex)); // play the sound effect 

			// Set ready up button as not interactable 
			s_ReadyUpButton.interactable = false;

			// Hide the currently selected wizard selection 
			wizardSelection.SetActive(false);

			// Show the currently selected mountable vehicle selection
			mountableSelection.SetActive(true);

			// Hide the Wizard Character Stats UI 
			wizardSelection.GetComponent<CharacterStatsUI>().Show(false);

			// Update the Character name text field with the mountable vehicles name 
			m_CharacterName.text = ReturnMountableName();

			// Set the ready up button to be interactable
			s_ReadyUpButton.interactable = true;
		}
		else
		{

			Debug.Log("[CursorSelectionBehaviour.HandleSelectCharacter]: " + "Wizard " + ReturnWizardName() + " has been unselected!");

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

	/// <summary>
	///		Returns to the main menu 
	/// </summary>
	private void HandleOnBackToMainMenu()
	{
		Debug.Log("[CursorSelectionManager.HandleOnBackToMainMenu]: " + "Returning to main menu...");

		if (GameUIManager.Instance)
		{
			GameUIManager.Instance.PlayerJoinMenuUI.ReturnToMainMenu();
		}
	}

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

	/// <summary>
	///		Sets the mountable selection choices for each player (cursor)
	/// </summary>
	/// <param name="p_CurrentCursorIdentity">The current player's cursor id</param>
	/// <param name="p_MountableSelectionsSpawned">List of all mountable selection's spawned in</param>
	private void SetMountableSelectionChoices(int p_CurrentCursorIdentity, List<Transform> p_MountableSelectionsSpawned)
	{
		if (p_CurrentCursorIdentity != m_CurrentUserID)
		{
			return;
		}

		mountableSelectionChoices = new GameObject[p_MountableSelectionsSpawned.Count];

		for (int i = 0; i < p_MountableSelectionsSpawned.Count; i++)
		{
			// Set all mountable selections to be inactive 
			p_MountableSelectionsSpawned[i].gameObject.SetActive(false);

			mountableSelectionChoices[i] = p_MountableSelectionsSpawned[i].gameObject;
		}


		// Set the player selection to the selected index (Which enables `allowMountableSelection` to true (Receiving input))
		mountableSelection = ReturnSelectedMountableVehicle();
	
		// Initially set mountable selected to false 
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