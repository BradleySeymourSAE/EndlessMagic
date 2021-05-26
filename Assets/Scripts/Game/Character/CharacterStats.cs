#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


public enum WizardType
{ 
	Draco = 1,
	Hermione = 2,
	SiriusBlack = 3,
	Snape = 4,
	Yennefer = 5,
	Voldemort = 6
};


/// <summary>
///		Object for storing Stats for a Character 
/// </summary>
[CreateAssetMenu(fileName = "New Character Stats", menuName = "Characters/Stats/New character stats")]
public class CharacterStats : ScriptableObject
{

	/// <summary>
	///		The characters name 
	/// </summary>
	public string characterName = "";

	/// <summary>
	///		The characters wizard type 
	/// </summary>
	public WizardType wizard = WizardType.Draco;

	/// <summary>
	///		Character's descriptive flavour text 
	/// </summary>
	public string characterFlavourText = "";


	[Header("Character Mobility")]
	/// <summary>
	///		The characters mobility / agility rating (Controlled by weight) 
	/// </summary>
	[Range(0f, 300f)] public float weight = 95f;

	[Header("Character Survivability")]
	/// <summary>
	///		The characters health rating 
	/// </summary>
	[Range(0f, 300f)] public float healthRating = 120;

	/// <summary>
	///		The characters defence rating 
	/// </summary>
	[Range(0f, 300f)] public float defenceRating = 160;

	/// <summary>
	///		The character's attach rating 
	/// </summary>
	[Header("Character Offensive Ability")]
	[Range(0f, 300f)] public float attackRating = 180;

	/// <summary>
	///		The amount of time it takes for an ability cooldown 
	/// </summary>
	[Range(0f, 300f)] public float abilityCooldown = 90;

}