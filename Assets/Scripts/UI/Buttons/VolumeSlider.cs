using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlappyBird.UI.Buttons
{
    [RequireComponent(typeof(Image))]
    public class VolumeSlider : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image previousImage;
        private Image _fillImage;

        private void Awake()
        {
            _fillImage = GetComponent<Image>();
        }

        private void Start()
        {
            // float volume = PlayerPrefs.GetFloat("Volume", Constants.DEFAULT_VOLUME);
            // SetFillAmountSafe(volume);
        }

        private void OnEnable()
        {
            // if (AudioManager.Instance != null)
            //     AudioManager.Instance.VolumeChanged += OnVolumeChanged;
        }

        private void OnDisable()
        {
            // if (AudioManager.Instance != null)
            //     AudioManager.Instance.VolumeChanged -= OnVolumeChanged;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_fillImage == null) return;

            RectTransform fillRect = _fillImage.rectTransform;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    fillRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
                return;

            float width = fillRect.rect.width;
            float normalizedX = (localPoint.x / width) + 0.5f;

            float volume = Mathf.Round(Mathf.Clamp01(normalizedX) * 20f) / 20f;

            SetFillAmountSafe(volume);

            // if (volume == 0)
            //     AudioManager.Instance?.ToggleMute();
            // else
            //     AudioManager.Instance?.SetVolume(volume);
        }


        // private void OnVolumeChanged(object sender, VolumeChangedEventArgs e)
        // {
        //     if (!this || !isActiveAndEnabled || _fillImage == null) return;
        //     if (Mathf.Approximately(e.NewVolume, e.OldVolume)) return;
        //
        //     SetFillAmountSafe(e.NewVolume);
        //
        //     if (previousImage != null)
        //     {
        //         bool shouldShow = e.NewVolume == 0f;
        //         previousImage.gameObject.SetActive(shouldShow);
        //         if (shouldShow)
        //             previousImage.fillAmount = e.OldVolume;
        //     }
        // }

        private void SetFillAmountSafe(float value)
        {
            if (_fillImage != null)
                _fillImage.fillAmount = value;
        }
    }
}
