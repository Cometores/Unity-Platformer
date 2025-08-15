using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        
        [Header("Audio Source")]
        [SerializeField] private AudioSource[] sfx;
        [SerializeField] private AudioSource[] bgm;
        
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
}
