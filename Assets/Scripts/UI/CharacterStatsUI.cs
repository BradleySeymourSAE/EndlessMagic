#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion


[System.Serializable]
/// <summary>
///		Data class for setting the character prefab's stats 
/// </summary>
public class CharacterStatsUI : MonoBehaviour
{

	/// <summary>
	///		Reference to the character stats data class 
	/// </summary>
	[SerializeField] private CharacterStats m_CharacterStats;

	/// <summary>
	///		Reference to the current character stats UI transform 
	/// </summary>
	[SerializeField] private Transform m_CharacterUIReference;

	/// <summary>
	///		Reference to the current player ui 
	/// </summary>
	private int m_CurrentPlayer;

	private TMP_Text m_CharacterName;
	
	private TMP_Text m_CharacterStatsHeader;

	private TMP_Text m_FlavourText;

	private Slider m_WeightSlider;
	private TMP_Text m_WeightSliderTextValue;

	private Slider m_HealthRatingSlider;
	private TMP_Text m_HealthRatingTextValue;

	private Slider m_DefenceRatingSlider;
	private TMP_Text m_DefenceRatingTextValue;

	private Slider m_AttackRatingSlider;
	private TMP_Text m_AttackRatingTextValue;

	private Slider m_AbilityCooldownSlider;
	private TMP_Text m_AbilityCooldownTextValue;



	public void Setup(int p_CurrentPlayerReference, CharacterStats p_CharacterStatsData)
	{

		if (p_CurrentPlayerReference != null && p_CharacterStatsData != null)
		{
			m_CurrentPlayer = p_CurrentPlayerReference;
			m_CharacterStats = p_CharacterStatsData;
			m_CharacterUIReference = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_CharacterStats);
			

			if (m_CharacterUIReference != null)
			{ 

				m_CharacterStatsHeader = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_StatsTitle).GetComponentInChildren<TMP_Text>();
				m_CharacterStatsHeader.text = "Character Stats";

				m_CharacterName = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_CharacterName).GetComponentInChildren<TMP_Text>();
				m_FlavourText = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_FlavourText).GetComponentInChildren<TMP_Text>();

				m_WeightSlider = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_Mobility_WeightSlider).GetComponent<Slider>();
				m_WeightSlider.SetValueWithoutNotify(0);

				m_WeightSliderTextValue = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_Mobility_WeightBarTextValue).GetComponentInChildren<TMP_Text>();

				m_HealthRatingSlider = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_Survivability_HealthRatingSlider).GetComponent<Slider>();
				m_HealthRatingSlider.SetValueWithoutNotify(0);

				m_HealthRatingTextValue = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_Survivability_HealthRatingBarTextValue).GetComponentInChildren<TMP_Text>();

				m_DefenceRatingSlider = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_Survivability_DefenceRatingSlider).GetComponent<Slider>();
				m_DefenceRatingSlider.SetValueWithoutNotify(0);

				m_DefenceRatingTextValue = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_Survivability_DefenceRatingBarTextValue).GetComponentInChildren<TMP_Text>();

				m_AttackRatingSlider = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_OffensiveAbility_AttackRatingSlider).GetComponent<Slider>();
				m_AttackRatingSlider.SetValueWithoutNotify(0);

				m_AttackRatingTextValue = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_OffensiveAbility_AttackRatingBarTextValue).GetComponentInChildren<TMP_Text>();

				m_AbilityCooldownSlider = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_OffensiveAbility_AbilityCooldownSlider).GetComponent<Slider>();
				m_AbilityCooldownSlider.SetValueWithoutNotify(0);

				m_AbilityCooldownTextValue = GameEntity.FindSceneAsset(m_CurrentPlayer, SceneAsset.SelectionUI_OffensiveAbility_AbilityCooldownBarTextValue).GetComponentInChildren<TMP_Text>();
			}
		}
		else
		{
			Debug.LogError("[CharacterStatsUI.Setup]: " + "Could not return wizard character type!");
			return;
		}
	}

	/// <summary>
	///		Toggles displaying the ui
	/// </summary>
	/// <param name="ShouldDisplayUI"></param>
	public void Show(bool ShouldDisplayUI) => m_CharacterUIReference.gameObject.SetActive(ShouldDisplayUI);
	

	/// <summary>
	///		Handes displaying the stats UI 
	/// </summary>
	public void DisplayCharacterUI()
	{
		Debug.Log("[CharacterStatsUI.DisplayStats]: " + "Displaying stats for character " + m_CurrentPlayer);

		m_CharacterName.text = m_CharacterStats.characterName;
		m_FlavourText.text = "The Flavour" + "\n" + $"{m_CharacterStats.characterFlavourText}";
		m_CharacterStatsHeader.text = $"{m_CharacterStats.characterName} - Statistics";

		if (m_WeightSlider != null)
		{ 
			m_WeightSlider.value = normalize(m_CharacterStats.weight, 0, 300);
			m_WeightSliderTextValue.text = Mathf.RoundToInt(m_CharacterStats.weight).ToString("0") + "kg";
		}

		if (m_HealthRatingSlider != null)
		{ 
			m_HealthRatingSlider.value = normalize(m_CharacterStats.healthRating, 0, 300);
			m_HealthRatingTextValue.text = Mathf.RoundToInt(m_CharacterStats.healthRating).ToString("0");
		}

		if (m_DefenceRatingSlider != null)
		{ 
			m_DefenceRatingSlider.value = normalize(m_CharacterStats.defenceRating, 0, 300);
			m_DefenceRatingTextValue.text = Mathf.RoundToInt(m_CharacterStats.defenceRating).ToString("0");
		}

		if (m_AttackRatingSlider != null)
		{
			m_AttackRatingSlider.value = normalize(m_CharacterStats.attackRating, 0, 300);
			m_AttackRatingTextValue.text = Mathf.RoundToInt(m_CharacterStats.attackRating).ToString("0");
		}

		if (m_AbilityCooldownSlider != null)
		{ 
			m_AbilityCooldownSlider.value = normalize(m_CharacterStats.abilityCooldown, 0, 300);
			m_AbilityCooldownTextValue.text = Mathf.RoundToInt(m_CharacterStats.abilityCooldown).ToString("0");
		}
	}


	// Normalization Example
	// $int = 12
	// $min = 10
	// $max = 20
	// $normalized = normalize($int, $min, $max) => 0.2
	// $denormalized = denormalize($normalized, $int, $max) => 12

	/// <summary>
	///		Normalises a value between a minimum and maximum 
	/// </summary>
	/// <param name="value">The value thats coming in</param>
	/// <param name="min">The minimum value to scale between</param>
	/// <param name="max">The maximum value to scale between</param>
	/// <returns></returns>
	private float normalize(float value, float min = 0, float max = 100)
	{
		var result = (value - min) / (max - min);
		return result;
	}

	/// <summary>
	///		Denormalises a nromalised value
	/// </summary>
	/// <param name="NormalizedValue">The normalized value coming in</param>
	/// <param name="min">The minimum value to scale between</param>
	/// <param name="max">The maximum value to scale between</param>
	/// <returns></returns>
	private float denormalize(float NormalizedValue, float min = 0, float max = 100)
	{
		var result = (NormalizedValue * (max - min) + min);
		return result;
	}
}