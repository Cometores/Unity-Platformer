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
        [SerializeField] private AudioChannelSO[] audioChannels;

        private int _bgmIndex;

        #region Unity methods

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            InvokeRepeating(nameof(PlayRandomBgmIfNeeded), 0, 2);
        }

        private void Start()
        {
            foreach (var audioChannel in audioChannels)
            {
                var volume = audioChannel.GetSavedVolume();
                ApplyAndSaveVolume(volume, audioChannel.channelName);
                RaiseVolumeChangedEvent(volume, volume, audioChannel.channelName);
            }
        }

        #endregion
        
        #region Volume
        
        public void ToggleMute(string channelName)
        {
            var audioChannel = Array.Find(audioChannels, x => x.channelName == channelName);
            var oldVolume = audioChannel.volume;

            if (audioChannel.IsMuted)
            {
                audioChannel.volume = audioChannel.volumeBeforeMute;
            }
            else
            {
                audioChannel.volumeBeforeMute = audioChannel.volume;
                audioChannel.volume = 0f;
            }

            ApplyAndSaveVolume(audioChannel.volume, channelName);
            RaiseVolumeChangedEvent(oldVolume, audioChannel.volume, channelName);
        }

        public void SetVolume(float newVolume, string channelName)
        {
            var audioChannel = Array.Find(audioChannels, x => x.channelName == channelName);

            float clamped = VolumeUtils.ClampVolume(newVolume, Constants.MUTE_THRESHOLD);
            float oldVolume = audioChannel.volume;

            if (Mathf.Approximately(oldVolume, clamped)) return;

            audioChannel.volume = clamped;
            ApplyAndSaveVolume(audioChannel.volume, channelName);
            RaiseVolumeChangedEvent(oldVolume, audioChannel.volume, channelName);
        }
        
        #endregion
        
        #region Play / Stop 
        
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
            _bgmIndex = Random.Range(0, bgm.Length);
            PlayBGM(_bgmIndex);
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
        
        #endregion
        
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
    }

    public static class VolumeUtils
    {
        public static float ClampVolume(float volume, float muteThreshold) =>
            volume < muteThreshold ? 0f : Mathf.Clamp01(volume);
    }
}