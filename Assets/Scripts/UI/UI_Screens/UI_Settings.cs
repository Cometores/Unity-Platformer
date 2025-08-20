using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class UI_Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float _mixerMultiplier = 25;
        
        [Header("BGM Settings")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private TextMeshProUGUI bgmSliderText;
        [SerializeField] private string bgmParameter;
    
        [Header("SFX Settings")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TextMeshProUGUI sfxSliderText;
        [SerializeField] private string sfxParameter;

        public void SFXSliderValue(float value)
        {
            sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
            float newValue = Mathf.Log10(value) * _mixerMultiplier;
            audioMixer.SetFloat(sfxParameter, newValue);
        }
    
        public void BGMSliderValue(float value)
        {
            sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
            float newValue = Mathf.Log10(value) * _mixerMultiplier;
            audioMixer.SetFloat(bgmParameter, newValue);
        }

        private void OnEnable()
        {
            PlayerPrefs.GetFloat(sfxParameter, sfxSlider.value);
            PlayerPrefs.GetFloat(bgmParameter, bgmSlider.value);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
            PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
        }
    }
}
