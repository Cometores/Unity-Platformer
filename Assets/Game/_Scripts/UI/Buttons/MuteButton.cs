using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game._Scripts.UI.Buttons
{
    public class MuteButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string mixerName;
        
        [Header("Sprites")]
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite mutedSprite;

        [Header("Animation")] 
        [SerializeField] private float hoverScale = 2f;
        
        private Vector3 _originalScale;
        private Image Image => GetComponent<Image>();

        private void Start()
        {
            _originalScale = transform.localScale;
            
            var volume = SaveSystem.GetVolume(mixerName);
            Image.sprite = volume > 0 
                ? normalSprite 
                : mutedSprite;
            
            AudioManager.Instance.VolumeChanged += OnVolumeChanged;
        }

        private void OnDisable()
        {
            transform.localScale = _originalScale;
        }

        private void OnDestroy()
        {
            AudioManager.Instance.VolumeChanged -= OnVolumeChanged;
        }
        
        private void OnVolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            if (mixerName != e.MixerName) return;

            Image.sprite = e.NewVolume >= Constants.MUTE_THRESHOLD 
                ? normalSprite 
                : mutedSprite;
        }

        #region Mouse pointer behaviour

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _originalScale * hoverScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _originalScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.ToggleMute(mixerName);
        }

        #endregion
    }
}