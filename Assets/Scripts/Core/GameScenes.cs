﻿#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


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



	#region Scene ID's

	public const string EndlessMagic_StartingMenu = "EndlessMagic_StartingMenu";
	public const string EndlessMagic_CharacterCreation = "EndlessMagic_CharacterCreation";
	public const string EndlessMagic_GameLevel_01 = "EndlessMagic_GameLevel_01";

	#endregion


	/// <summary>
	///		Finds the string associated with the Scenes enum 
	/// </summary>
	/// <param name="p_SceneType">The type of scene</param>
	/// <returns></returns>
	public static string SelectGameSceneBySceneType(Scenes p_SceneType)
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

}
