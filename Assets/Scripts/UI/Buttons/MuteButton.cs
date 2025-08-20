using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlappyBird.UI.Buttons
{
    public class MuteButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("Sprites")] [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite mutedSprite;

        [Header("Animation")] [SerializeField] private float hoverScale = 2f;

        private Vector3 _originalScale;
        private Image Image => GetComponent<Image>();
        // private bool IsMuted => AudioManager.Instance;

        private void Start()
        {
            _originalScale = transform.localScale;
            
            // var volume = PlayerPrefs.GetFloat("Volume", Constants.DEFAULT_VOLUME);
            // Image.sprite = volume > 0 ? normalSprite : mutedSprite;
            Image.sprite = normalSprite;
        }

        private void OnEnable()
        {
            // if (AudioManager.Instance)
            //     AudioManager.Instance.VolumeChanged += OnVolumeChanged;
        }

        protected void OnDisable()
        {

            transform.localScale = _originalScale;
            // AudioManager.Instance.VolumeChanged -= OnVolumeChanged;
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
            // AudioManager.Instance.ToggleMute();
        }

        #endregion

        // private void OnVolumeChanged(object sender, VolumeChangedEventArgs e)
        // {
        //     if (e.NewVolume == 0f)
        //     {
        //         Image.sprite = mutedSprite;
        //     }
        //     else if (e.NewVolume >= Constants.MUTE_THRESHOLD)
        //         Image.sprite = normalSprite;
        // }
    }
}