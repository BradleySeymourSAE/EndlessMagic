#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion


/// <summary>
///		The Scenes in the Game 
/// </summary>
public enum Scenes 
{ 
	EndlessMagic_StartingMenu, 
	EndlessMagic_CharacterCreation, 
	EndlessMagic_GameLevel_01 
};


/// <summary>
///		Class for Handling Game Scene Key's 
/// </summary>
public static class GameScenes
{

	#region Scenes 

	public const string EndlessMagic_StartingMenu = "EndlessMagic_StartingMenu";
	public const string EndlessMagic_CharacterCreation = "EndlessMagic_CharacterCreation";
	public const string EndlessMagic_GameLevel_01 = "EndlessMagic_GameLevel_01";

	#endregion

	#region Public Methods 

	/// <summary>
	///		Loads a scene 
	/// </summary>
	/// <param name="p_SceneToLoad">The scene to load</param>
	/// <param name="UseAsyncLoading">Whether to load the scene asyncronously - defaults to false</param>
	public static void LoadScene(Scenes p_SceneToLoad, bool UseAsyncLoading = false)
	{
		string scene = SelectScene(p_SceneToLoad);

		if (UseAsyncLoading)
		{
			SceneManager.LoadSceneAsync(scene);
		}
		else
		{
			SceneManager.LoadScene(scene);
		}
	}

	/// <summary>
	///		Loads a new Scene 
	/// </summary>
	/// <param name="p_SceneToLoad">The scene you want to load</param>
	/// <param name="p_SceneMode">LoadSceneMode
	///		Single - Closes all currently loaded scenes and opens a new scene 
	///		Additive - Adds the scene to the current loaded scenes  
	/// </param>
	/// <param name="UseAsyncLoading">Whether to load the scene asyncronously or not - Defaults to false</param>
	public static void LoadScene(Scenes p_SceneToLoad, LoadSceneMode p_SceneMode = LoadSceneMode.Single, bool UseAsyncLoading = false)
	{
		string s_SelectedScene = SelectScene(p_SceneToLoad);

		if (s_SelectedScene == null)
		{ 
			Debug.LogWarning("[GameScenes.LoadScene]: " + "Could not find scene: " + p_SceneToLoad);
			return;
		}


		if (UseAsyncLoading)
		{
			SceneManager.LoadSceneAsync(s_SelectedScene, p_SceneMode);
		}
		else
		{
			SceneManager.LoadScene(s_SelectedScene, p_SceneMode);
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	///		Finds the string associated with the Scenes enum 
	/// </summary>
	/// <param name="p_SceneType">The type of scene</param>
	/// <returns></returns>
	private static string SelectScene(Scenes p_SceneType)
	{
		switch (p_SceneType)
		{
			case Scenes.EndlessMagic_StartingMenu:
				{
					return EndlessMagic_StartingMenu;
				}
			case Scenes.EndlessMagic_CharacterCreation:
				{
					return EndlessMagic_CharacterCreation;
				}
			case Scenes.EndlessMagic_GameLevel_01:
				{
					return EndlessMagic_GameLevel_01;
				}
		}

		return p_SceneType.ToString();
	}


	#endregion

}
