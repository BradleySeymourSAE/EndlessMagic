#region Namespaces
using UnityEngine;
using UnityEngine.Audio;
#endregion


/// <summary>
///		Data Class for storing Sound Effects for the Game 
/// </summary>
[System.Serializable]
public class SoundFX
{


	/// <summary>
	///		The name of the sound effect 
	/// </summary>
	public string name;

	/// <summary>
	///		The Sound Effect Category Name - This is used to find and play the sound effect 
	/// </summary>
	public SoundCategory Category;

	/// <summary>
	///		Set whether the audio clip is looping or not 
	/// </summary>
	public bool loop = false; // default to false  

	/// <summary>
	///		Whether the sound should play on awake or not 
	/// </summary>
	public bool awake = false; // default's to playing on awake 

	/// <summary>
	///		The volume for the sound effect 
	/// </summary>
	[Range(0f, 1f)]
	public float volume = 1f; // default to 1f 

	/// <summary>
	///		The Pitch for the sound fx 
	/// </summary>
	[Range(0.1f, 3f)]
	public float pitch = 1f; // default to 1f

	/// <summary>
	///		The length of the audio clip 
	/// </summary>
	public float length;

	/// <summary>
	///		The Maximum of the audio clip 
	/// </summary>
	public float MaximumLength; 

	/// <summary>
	///		The Sound Effect Clip 
	/// </summary>
	public AudioClip clip;

	/// <summary>
	///		The Audio Source for the Sound Effect 
	/// </summary>
	[HideInInspector]
	public AudioSource source;



}