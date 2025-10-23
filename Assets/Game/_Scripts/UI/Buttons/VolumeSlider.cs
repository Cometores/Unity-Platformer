using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game._Scripts.UI.Buttons
{
    [RequireComponent(typeof(Image))]
    public class VolumeSlider : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] string mixerName;
        [SerializeField] private Image previousImage;
        private Image _fillImage;

        private void Awake()
        {
            _fillImage = GetComponent<Image>();
        }

        private void Start()
        {
            float volume = PlayerPrefs.GetFloat(mixerName, Constants.DEFAULT_VOLUME);
            SetFillAmountSafe(volume);
        }

        private void OnEnable()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.volumeChanged += OnVolumeChanged;
        }

        private void OnDisable()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.volumeChanged -= OnVolumeChanged;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_fillImage == null) return;

            RectTransform fillRect = _fillImage.rectTransform;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(fillRect, eventData.position, 
                    eventData.pressEventCamera, out Vector2 localPoint))
                return;

            float width = fillRect.rect.width;
            float normalizedX = (localPoint.x / width) + 0.5f;

            float volume = Mathf.Round(Mathf.Clamp01(normalizedX) * 20f) / 20f;
            
            SetFillAmountSafe(volume);

            if (volume == 0)
                AudioManager.Instance?.ToggleMute(mixerName);
            else
                AudioManager.Instance?.SetVolume(volume, mixerName);
        }
        
        private void OnVolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            if (mixerName != e.MixerName) return;
            if (!this || !isActiveAndEnabled || _fillImage == null) return;
            if (Mathf.Approximately(e.NewVolume, e.OldVolume)) return;
        
            SetFillAmountSafe(e.NewVolume);
        
            if (previousImage != null)
            {
                bool shouldShow = e.NewVolume == 0f;
                previousImage.gameObject.SetActive(shouldShow);
                if (shouldShow)
                    previousImage.fillAmount = e.OldVolume;
            }
        }

        private void SetFillAmountSafe(float value)
        {
            if (_fillImage != null)
                _fillImage.fillAmount = value;
        }
    }

    internal static class Constants
    {
        public const float MUTE_THRESHOLD = 0.04f;
        public const float DEFAULT_VOLUME = 0.5f;
    }
}
