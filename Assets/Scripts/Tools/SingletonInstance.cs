#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///		Creates a singleton instance of a game object 
/// </summary>
public class SingletonInstance : MonoBehaviour
{

	private GameObject Instance;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = gameObject;
			DontDestroyOnLoad(gameObject);
		}	
	}
}