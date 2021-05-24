#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///     Creates Game Controller Specific Sprites 
/// </summary>
[CreateAssetMenu(fileName = "Add Controller UI", menuName = "Controllers/New Controller UI")]
public class GameControllerUIData : ScriptableObject
{

   /// <summary>
   ///		The Game Controller Type 
   /// </summary>
	public GameControllerType ControllerType;

	/// <summary>
	///		The south button on the game controller - Example (Xbox Controller - A)
	/// </summary>
	public Sprite ButtonSouth;

	/// <summary>
	///		The North button on the game controller - Example (Xbox Controller - Y)
	/// </summary>
	public Sprite ButtonNorth;

	/// <summary>
	///		The East button on the game controller - Example (Xbox Controller - B)
	/// </summary>
	public Sprite ButtonEast;

	/// <summary>
	///		The West button on the game controller - Example (Xbox Controller - X)
	/// </summary>
	public Sprite ButtonWest;

	/// <summary>
	///		The right trigger on the game controller 
	/// </summary>
	public Sprite RightTrigger;

	/// <summary>
	///		The left trigger on the game controller 
	/// </summary>
	public Sprite LeftTrigger;

	/// <summary>
	///		Left Bumper on the game controller 
	/// </summary>
	public Sprite LeftBumper;

	/// <summary>
	///		Right Bumper on the game controller 
	/// </summary>
	public Sprite RightBumper;

	/// <summary>
	///		Left Stick Pressed Sprite Image  
	/// </summary>
	public Sprite LeftStickDown;

	/// <summary>
	///		Right Stick Down Sprite Image 
	/// </summary>
	public Sprite RightStickDown;

	/// <summary>
	///		Game Controller is an Xbox Controller Type - The windows key located center left 
	/// </summary>
	public Sprite Windows;

	/// <summary>
	///		Game Controller is a PS4 Controller Type - The Share Button (ui sprite image) located on the center left 
	/// </summary>
	public Sprite Share;

	/// <summary>
	///		Game Controller is a PS4 Controller Type - The Touchpad (ui sprite image) 
	/// </summary>
	public Sprite Touchpad;

	/// <summary>
	///		Game Controller is a Nintendo Switch Type - Display the Switch Home Button 
	/// </summary>
	public Sprite SwitchHome;

	/// <summary>
	///		Game Controller is a Nintendo Switch Type - Displays the Minus Icon 
	/// </summary>
	public Sprite Minus;

	/// <summary>
	///		Game Controller is a Nintendo Switch Type - Display the Plus Icon 
	/// </summary>
	public Sprite Plus;

	/// <summary>
	///		Game Controller is a Nintendo Switch Type - Display the Square Icon 
	/// </summary>
	public Sprite Square;

}
