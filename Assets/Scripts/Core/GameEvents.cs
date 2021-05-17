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

	public delegate void GameDeviceListDelegate(List<GameInputDevice> p_GameDeviceList);

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
	///		Called after onSetCurrentConnectedDevices, which changes the amount of connected devices, this updates the UI 
	/// </summary>
	public static VoidDelegate HandleUpdateDevicesEvent;

	/// <summary>
	///		Called to allow multiple device input 
	/// </summary>
	public static BoolParameterDelegate HandleAllowMultipleDeviceInputEvent;

	/// <summary>
	///		Called on start to load the currently connected devices
	/// </summary>
	public static VoidDelegate LoadConnectedDevicesEvent;

	/// <summary>
	///		Called to set the current active players after the input event has been received 
	/// </summary>
	public static IntParameterDelegate onSetCurrentConnectedDevices;

	/// <summary>
	///		Called using the Device ID to add the player's device as an input 
	/// </summary>
	public static IntParameterDelegate AddPlayerInputDeviceEvent;

	/// <summary>
	///		Sets the connected players 
	/// </summary>
	public static GameDeviceListDelegate SetConnectedPlayersEvent;

	#endregion

}