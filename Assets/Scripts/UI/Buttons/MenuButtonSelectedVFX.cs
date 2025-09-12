using UnityEngine;

namespace UI.Buttons
{
    public class SelectedVFX : MonoBehaviour
    {
        private MenuButton _menuButton;
        private GameObject _vfxRoot;
        private Animator[] _animators;

        private static readonly int SelectedHash = Animator.StringToHash("selected");
        private static readonly int LostFocusHash = Animator.StringToHash("lostFocus");
        private static readonly int RandHash = Animator.StringToHash("rand");

        private void Reset()
        {
            if (!_menuButton) _menuButton = GetComponentInParent<MenuButton>();
            if (!_vfxRoot) _vfxRoot = gameObject;
        }

        private void Awake()
        {
            _menuButton = GetComponentInParent<MenuButton>();
            _animators = GetComponentsInChildren<Animator>(true);

            if (_animators == null || _animators.Length == 0)
                Debug.LogError(
                    $"No Animator found in {nameof(SelectedVFX)}",
                    this
                );
        }

        private void OnEnable()
        {
            foreach (var a in _animators)
                a.gameObject.SetActive(false);

            _menuButton.Selected += OnSelected;
            _menuButton.Unselected += OnUnselected;
        }

        private void OnDisable()
        {
            _menuButton.Selected -= OnSelected;
            _menuButton.Unselected -= OnUnselected;
        }

        private void OnSelected()
        {
            int rand = Random.Range(0, 3);

            foreach (var a in _animators)
            {
                a.gameObject.SetActive(true);
                a.SetInteger(RandHash, rand);
                a.SetTrigger(SelectedHash);
            }
        }

        private void OnUnselected()
        {
            foreach (var a in _animators)
                a.SetTrigger(LostFocusHash);

            if (_vfxRoot) _vfxRoot.SetActive(false);
        }
    }
}