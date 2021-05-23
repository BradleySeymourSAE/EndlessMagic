#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#endregion


/// <summary>
///		The Sound Effect
///		The key used to play an audio effect from the audio manager instance 
/// </summary>
public enum SoundEffect
{
	UI_StartMenuBackgroundMusic,
	UI_CharacterCreationBackgroundMusic,
	GUI_Selected,
	GUI_MenuSwitched,
	GUI_Move,
	GUI_Confirm,
	Game_Level01_Background,
	Draco_Selected,
	Hermione_Selected,
	SiriusBlack_Selected,
	Snape_Selected,
	Voldemort_Selected,
	Yennefer_Selected,
}

/// <summary>
///		A Static Class for Handling Audio Effects 
/// </summary>
public static class AudioManager
{ 

	private static AudioMixerGroup m_AudioMixer;

	private static GameAssets m_GameAssetsReference;

	public static void SetupSounds(SoundFX[] Sounds, AudioMixerGroup Mixer)
	{

		m_AudioMixer = Mixer;
		m_GameAssetsReference = Object.FindObjectOfType<GameAssets>();

		if (m_GameAssetsReference != null)
		{ 
		
			foreach (SoundFX sound in Sounds)
			{

				// Add the audio source component to the game assets game object 
				sound.source = m_GameAssetsReference.gameObject.AddComponent<AudioSource>();
			
				// Set the audio source clip to the audio clip added  
				sound.source.clip = sound.clip; 

				// Set whether the audio source should loop 
				sound.source.loop = sound.loop;

				// Set whether the audio source should play on awake 
				sound.source.playOnAwake = sound.awake;

				// Set the pitch of the audio source 
				sound.source.pitch = sound.pitch;
				
				// Set the volume of the audio source 
				sound.source.volume = sound.volume;

				// Set the Sound Effects Length based on the clips length 
				sound.length = sound.source.clip.length;



				sound.source.outputAudioMixerGroup = m_AudioMixer;
			}
		}
		else
		{
			Debug.LogWarning("[AudioManager.SetupSounds]: " + "Could not find Game Assets Reference!");
		}
	}


	/// <summary>
	///		Plays a Sound Effect using the Sound Category Type 
	/// </summary>
	/// <param name="p_SoundCategory"></param>
	public static void PlaySound(SoundEffect p_SoundCategory)
	{
		// Gets the sound effect using the sound category name 
		SoundFX sound = GetSoundEffect(p_SoundCategory);

		// Check if the sound is null 
		if (sound == null)
		{
			// Log a warning if we could not find the sound effect 
			Debug.LogWarning("[AudioManager.PlaySound]: " + "Could not find " + p_SoundCategory + " sound effect!");
			return;
		}

		// Set the sound effect's volume, pitch and other values that change here 
		sound.source.volume = sound.volume;
		sound.source.playOnAwake = sound.awake;
		sound.source.loop = sound.loop;
		sound.source.pitch = sound.pitch;
	
		sound.source.Play();
	}


	/// <summary>
	///		Get the SoundFX by the SoundCategory - Which is basically the sound's name 
	/// </summary>
	/// <param name="p_SoundCategory">The sound effects name </param>
	/// <returns></returns>
	private static SoundFX GetSoundEffect(SoundEffect p_SoundCategory)
	{
		// Loop through the game assets sound effects array 
		foreach (SoundFX s_GameSoundEffect in GameAssets.Assets.GameSoundEffects)
		{
			// Check if the sound effect category name is the same as the one we are searching for 
			if (s_GameSoundEffect.Category == p_SoundCategory)
			{
				// If it is, return the sound effect 
				return s_GameSoundEffect;
			}
		}

		Debug.LogWarning("Sound " + p_SoundCategory + " could not be found!");
		return null;
	}

}
