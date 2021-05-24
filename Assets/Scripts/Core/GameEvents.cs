#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///		Static class that handles Game Events 
/// </summary>
public static class GameEvents
{

	#region Delegates

	public delegate void VoidDelegate();

	public delegate void IntParameterDelegate(int p_Integer);
	public delegate void FloatParameterDelegate(float p_Float);

	public delegate void DoubleIntegerParameterDelegate(int p_Int, int p_Integer);
	public delegate void DoubleFloatParameterDelegate(float p_Float, float p_SecondFloat);

	public delegate void TransformIntegerParameterDelegate(Transform p_Transform, int p_Integer);
	public delegate void GameObjectIntegerParameterDelegate(GameObject p_GameObject, int p_Integer);

	public delegate void GameObjectParameterDelegate(GameObject p_GameObject);
	public delegate void TransformParameterDelegate(Transform p_Transform);

	public delegate void GameObjectListParameterDelegate(List<GameObject> p_GameObjectList);
	public delegate void TransformListParameterDelegate(List<Transform> p_TransformList);

	public delegate void IntegerTransformListParameterDelegate(int p_Integer, List<Transform> p_TransformList);

	public delegate void SoundCategoryParameterDelegate(SoundEffect p_SoundCategory);
	public delegate void AsyncOperationFloatParameterDelegate(AsyncOperation p_AsyncOperation, float p_FloatParameter);

	public delegate void BoolParameterDelegate(bool p_BooleanParameter);

	#endregion

	#region Events

	/// <summary>
	///		Once called - Plays button Selected Sound Effects 
	/// </summary>
	public static VoidDelegate PlayGUISelectedEvent;

	/// <summary>
	///		Once called - Plays the Menu Transition Sound Effect 
	/// </summary>
	public static VoidDelegate PlayMenuTransitionEvent;

	/// <summary>
	///		Once called - sets the players currently assigned a spot on the co-op player join screen
	/// </summary>
	public static IntParameterDelegate SetPlayerJoinedEvent;

	/// <summary>
	///		Once called - Plays the Audio Sound Effect for the selected Wizard
	/// </summary>
	public static IntParameterDelegate PlayWizardSelectedEvent;

	/// <summary>
	///		Once called - Sets the current player's cursors selectable wizards to navigate through 
	/// </summary>
	public static IntegerTransformListParameterDelegate SetSelectableWizards;

	/// <summary>
	///		Once called - Sets the current player cursors selectable mountable vehicles to navigate through 
	/// </summary>
	public static IntegerTransformListParameterDelegate SetSelectableMountables;

	/// <summary>
	///		Once called - ready's the player up on the co-op player join screen, 
	///		Game Object - The parent game object for the player's cursor
	///		Integer - Whether the player is ready or not 
	/// </summary>
	public static GameObjectIntegerParameterDelegate SetPlayerReadyEvent;

	/// <summary>
	///		Once called - Sets the character creation cursor's to persist into the next scene 
	/// </summary>
	public static GameObjectListParameterDelegate SetCharacterCreationCursorEvent;

	/// <summary>
	///		Once called - Begins the player join ready up countdown timer
	/// </summary>
	public static BoolParameterDelegate UpdatePlayerJoinReadyTimer;
	
	#endregion

}