using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game._Scripts.Debugging
{
    [RequireComponent(typeof(Button))]
    public class ChangePlayerControllerButton : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
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

        private void OnPressed() => PlayerManager.Instance.ChangePlayer(playerPrefab);
    }
}