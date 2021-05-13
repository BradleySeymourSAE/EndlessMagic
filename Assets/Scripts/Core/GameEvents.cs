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

	public delegate void DoubleIntegerParameterDelegate(int p_Int, int p_Integer);

	public delegate void FloatParameterDelegate(float p_Float);

	public delegate void DoubleFloatParameterDelegate(float p_Float, float p_SecondFloat);

	public delegate void TransformParameterDelegate(Transform p_Transform);

	public delegate void TransformIntegerParameterDelegate(Transform p_Transform, int p_Integer);

	public delegate void GameObjectParameterDelegate(GameObject p_GameObject);

	public delegate void TransformListParameterDelegate(List<Transform> p_TransformList);

	public delegate void GameObjectListParameterDelegate(List<GameObject> p_GameObjectList);

	public delegate void SoundCategoryParameterDelegate(SoundCategory p_SoundCategory);

	public delegate void AsyncOperationFloatParameterDelegate(AsyncOperation p_AsyncOperation, float p_FloatParameter);



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
	///		Called using the input amount of players, sets up the camera references (Split screen)
	/// </summary>
	public static IntParameterDelegate CoopCharacterCreationStartEvent;


	#endregion

}