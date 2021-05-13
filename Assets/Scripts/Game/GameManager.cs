#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///		Game Manager
///		- Handles the core gameplay loop
///		- Spawns the players into the game 
///		- Creates the racetrack procedurally 
/// </summary>
public class GameManager : MonoBehaviour
{

	#region Static

	/// <summary>
	///		Reference to the Game Manager Instance 
	/// </summary>
	public static GameManager Instance;

	#endregion

	#region Public Variables


	#endregion


	#region Private Variables


	#endregion


	#region Unity References


	private void OnEnable()
	{
		
	}


	private void OnDisable()
	{
		
	}


	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	#endregion


	#region Public Methods


	#endregion


	#region Private Methods


	#endregion

}