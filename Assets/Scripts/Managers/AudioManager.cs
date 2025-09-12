using System;
using UI.Buttons;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public EventHandler<VolumeChangedEventArgs> volumeChanged;
        
        [Header("Audio Source")]
        [SerializeField] private AudioSource[] sfx;
        [SerializeField] private AudioSource[] bgm;
        [SerializeField] private AudioMixer audioMixer;
        
        private bool IsBgmMuted => _bgmVolume <= Constants.MUTE_THRESHOLD;
        private bool IsSfxMuted => _sfxVolume <= Constants.MUTE_THRESHOLD;
        
        private float _sfxVolume;
        private float _bgmVolume;
        private float _sfxVolumeBeforeMute;
        private float _bgmVolumeBeforeMute;
        
        private int _bgmIndex;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            
            InvokeRepeating(nameof(PlayRandomBgmIfNeeded),0,2);
        }

        private void Start()
        {
            _bgmVolume = PlayerPrefs.GetFloat("bgm", Constants.DEFAULT_VOLUME);
            if (_bgmVolume == 0)
                _bgmVolume = Constants.DEFAULT_VOLUME;
            
            ApplyAndSaveVolume(_bgmVolume, "bgm");
            RaiseVolumeChangedEvent(_bgmVolume, _bgmVolume, "bgm");
            
            
            _sfxVolume = PlayerPrefs.GetFloat("sfx", Constants.DEFAULT_VOLUME);
            if (_sfxVolume == 0)
                _sfxVolume = Constants.DEFAULT_VOLUME;
            
            ApplyAndSaveVolume(_sfxVolume, "sfx");
            RaiseVolumeChangedEvent(_sfxVolume, _sfxVolume, "sfx");
        }

        public void ToggleMute(string channel)
        {
            if (channel == "sfx")
            {
                float oldVolume = _sfxVolume;

                if (IsSfxMuted)
                {
                    _sfxVolume = _sfxVolumeBeforeMute;
                }
                else
                {
                    _sfxVolumeBeforeMute = _sfxVolume;
                    _sfxVolume = 0f;
                }

                ApplyAndSaveVolume(_sfxVolume, channel);
                RaiseVolumeChangedEvent(oldVolume, _sfxVolume, channel);
            }
            else if (channel == "bgm")
            {
                float oldVolume = _bgmVolume;

                if (IsBgmMuted)
                {
                    _bgmVolume = _bgmVolumeBeforeMute;
                }
                else
                {
                    _bgmVolumeBeforeMute = _bgmVolume;
                    _bgmVolume = 0f;
                }

                ApplyAndSaveVolume(_bgmVolume, channel);
                RaiseVolumeChangedEvent(oldVolume, _bgmVolume, channel);
            }
        }

        public void SetVolume(float newVolume, string mixerName)
        {
            if (mixerName == "sfx")
            {
                float clamped = VolumeUtils.ClampVolume(newVolume, Constants.MUTE_THRESHOLD);
                float oldVolume = _sfxVolume;

                if (Mathf.Approximately(oldVolume, clamped)) return;

                _sfxVolume = clamped;
                ApplyAndSaveVolume(_sfxVolume, mixerName);
                RaiseVolumeChangedEvent(oldVolume, _sfxVolume, mixerName);
            }
            else if (mixerName == "bgm")
            {
                float clamped = VolumeUtils.ClampVolume(newVolume, Constants.MUTE_THRESHOLD);
                float oldVolume = _bgmVolume;

                if (Mathf.Approximately(oldVolume, clamped)) return;

                _bgmVolume = clamped;
                ApplyAndSaveVolume(_bgmVolume, mixerName);
                RaiseVolumeChangedEvent(oldVolume, _bgmVolume, mixerName);
            }
        }

        private void ApplyAndSaveVolume(float volume, string mixerName)
        {
            PlayerPrefs.SetFloat($"{mixerName}", volume);
            PlayerPrefs.Save();
            
            float db = Mathf.Lerp(-20f, 20f, volume);
            if (Mathf.Approximately(db, -20)) db = -80;
            audioMixer.SetFloat(mixerName, db);
        }

        private void RaiseVolumeChangedEvent(float oldVolume, float newVolume, string mixerName)
        {
            volumeChanged?.Invoke(this, new VolumeChangedEventArgs(oldVolume, newVolume, mixerName));
        }
        
        public void PlaySfx(int sfxToPlay, bool randomPitch = true)
        {
            if (sfxToPlay >= sfx.Length) 
                return;
            
            if (randomPitch)
                sfx[sfxToPlay].pitch = Random.Range(0.8f, 1.2f);
            
            sfx[sfxToPlay].Play();
        }

        public void PlayRandomBgm()
        {
            _bgmIndex= Random. Range(0, bgm. Length);
            PlayBGM(_bgmIndex) ;
        }
        
        public void StopSfx(int sfxToStop) => sfx[sfxToStop].Stop();

        public void PlayRandomBgmIfNeeded()
        {
            if (bgm[_bgmIndex].isPlaying == false)
                PlayRandomBgm();
        }
        
        public void PlayBGM(int bgmToPlay)
        {
            foreach (var t in bgm)
            {
                t.Stop();
            }

            _bgmIndex = bgmToPlay;
            bgm[bgmToPlay].Play();
        }
    }
    
    public static class VolumeUtils
    {
        public static float ClampVolume(float volume, float muteThreshold) =>
            volume < muteThreshold ? 0f : Mathf.Clamp01(volume);
    }
}
