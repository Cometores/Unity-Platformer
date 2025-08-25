using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadFocusGuard : MonoBehaviour
{
    [SerializeField] private Selectable fallbackSelected;
    private DefaultInputActions _input;

    private void Awake()
    {
        _input = new DefaultInputActions();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.UI.Navigate.performed += OnNavigatePerformed;
    }

    private void OnDisable()
    {
        _input.UI.Navigate.performed -= OnNavigatePerformed;
        _input.Disable();
    }

    private void OnNavigatePerformed(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject != null)
            return;

        EnsureSelection();
    }

    private void EnsureSelection()
    {
        if (fallbackSelected == null) return;

        fallbackSelected.Select();
    }
}