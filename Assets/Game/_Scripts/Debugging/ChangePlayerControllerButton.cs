using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game._Scripts.Debugging
{
    /// <summary>
    /// A UI button component that allows switching or changing the player's controller
    /// in the game.
    /// </summary>
    /// <remarks>
    /// This script is used in debugging or development scenarios to facilitate testing
    /// of different player controllers during gameplay.
    /// </remarks>
    [RequireComponent(typeof(Button))]
    public class ChangePlayerControllerButton : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

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