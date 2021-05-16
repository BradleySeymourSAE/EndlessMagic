#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion


/// <summary>
///		Selects the first UI Button  
/// </summary>
public class ButtonSelect : MonoBehaviour
{
	public Button InitialButton;


	private void Start()
	{
		InitialButton.Select();
	}
}