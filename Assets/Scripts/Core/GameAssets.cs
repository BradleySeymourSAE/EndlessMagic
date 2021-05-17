﻿#region Namespaces
using UnityEngine;
using UnityEngine.Audio;
using System.Reflection;
#endregion



/// <summary>
///		Displays Game Assets in the Inspector 
/// </summary>
public class GameAssets : MonoBehaviour 
{

	#region Static  
	
	/// <summary>
	///		Reference to the static Game Assets Instance 
	/// </summary>
	public static GameAssets Assets;

	#endregion

	#region Public Variables 

	[Header("Audio")]
	/// <summary>
	///  Reference to the Audio Mixer Group 
	/// </summary>
	public AudioMixerGroup m_AudioMixer;

	/// <summary>
	///		An Array of Game Sound Effects 
	/// </summary>
	public SoundFX[] GameSoundEffects;

	public GameObject[] PlayerCursors;

	#endregion

	#region Unity References 

	private void Awake()
	{
		int maximumAllowedPlayers = GameManager.Instance.maximumAllowedPlayers;

		if (Assets != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Assets = this;
			DontDestroyOnLoad(gameObject);
		}

		if (GameSoundEffects.Length > 0)
		{ 
			AudioManager.SetupSounds(GameSoundEffects, m_AudioMixer);

		}
		else
		{
			Debug.LogWarning("Game Sound Effects Length is less than 0!");
		}

		int currentIndex = 0;

		PlayerCursors = new GameObject[maximumAllowedPlayers];

		for (int i = 0; i < maximumAllowedPlayers; ++i)
		{
			currentIndex++;

			string search = $"CursorPrefabs/P{currentIndex}_Cursor";

			GameObject cursorPrefab = Resources.Load<GameObject>(search);

			PlayerCursors[i] = cursorPrefab;
		}
	}

	#endregion

}
