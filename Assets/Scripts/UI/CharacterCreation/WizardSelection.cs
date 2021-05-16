#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///		Wizard Selection - Used for selecting a wizard in the character creation class 
/// </summary>
public class WizardSelection : MonoBehaviour
{

	#region Public Variables 

	/// <summary>
	///		Array of Wizard Game Object's  
	/// </summary>
	public GameObject[] wizards;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Selected Wizard
	/// </summary>
	private int m_SelectedWizardIndex = 0;

	#endregion

	#region Public Methods 


	public void Setup(int p_SpawnedWizards)
	{
		wizards = new GameObject[p_SpawnedWizards];

		for (int i = 0; i < transform.childCount; i++)
		{
			wizards[i] = transform.GetChild(i).gameObject;
			wizards[i].SetActive(false);
		}


		wizards[0].SetActive(true);
	}

	/// <summary>
	///		Selects the next wizard in the array of wizard prefabs 
	/// </summary>
	public void Next()
	{
		wizards[m_SelectedWizardIndex].SetActive(false);
		m_SelectedWizardIndex = (m_SelectedWizardIndex + 1) % wizards.Length;
		wizards[m_SelectedWizardIndex].SetActive(true);
	}

	/// <summary>
	///		Selects the previous wizard in the array of wizard prefabs  
	/// </summary>
	public void Previous()
	{
		wizards[m_SelectedWizardIndex].SetActive(false);
		
		m_SelectedWizardIndex--;

		if (m_SelectedWizardIndex < 0)
		{
			m_SelectedWizardIndex += wizards.Length;
		}
		
		wizards[m_SelectedWizardIndex].SetActive(true);
	}

	/// <summary>
	///		Returns the currently selected wizard gameobject 
	/// </summary>
	/// <returns></returns>
	public GameObject ReturnSelectedWizard() => wizards[m_SelectedWizardIndex];

	#endregion

}