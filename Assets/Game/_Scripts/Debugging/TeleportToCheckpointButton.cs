using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game._Scripts.Debugging
{
    [RequireComponent(typeof(Button))]
    public class TeleportToCheckpointButton : MonoBehaviour
    {
        [SerializeField] private GameObject checkpoint;
        private Button _button;
        private static Player.Player Player => PlayerManager.Instance.player;

        private void Awake()
        {
            _button = GetComponent<Button>();

            if (checkpoint == null)
                Debug.LogWarning("Checkpoint is not assigned.", this);
        }

        private void OnEnable()
        {
            if (_button != null)
                _button.onClick.AddListener(OnPressed);
        }

        private void OnDisable()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnPressed);
        }

        private void OnPressed()
        {
            Player.transform.position = checkpoint.transform.position;
        }
    }
}