using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game._Scripts.UI
{
    /// <summary>
    /// Helps with gamepad navigation and element selection.
    /// </summary>
    public class GamepadFocusGuard : MonoBehaviour
    {
        [SerializeField] private Selectable fallbackSelected;
        [SerializeField] private float stickDeadZone = 0.5f;
        private DefaultInputActions _input;

        private void Awake()
        {
            _input = new DefaultInputActions();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.UI.Navigate.performed += OnNavigatePerformed;
            
            EnsureSelection();
        }

        private void OnDisable()
        {
            _input.UI.Navigate.performed -= OnNavigatePerformed;
            _input.Disable();
        }

        /// <summary>
        /// Handles navigation input performed events and ensures a UI element is selected
        /// when necessary, particularly for cases where the gamepad input magnitude
        /// exceeds the defined dead zone.
        /// </summary>
        /// <param name="ctx">The input action callback context containing the navigation input data.</param>
        private void OnNavigatePerformed(InputAction.CallbackContext ctx)
        {
            if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject != null)
                return;

            Vector2 input = ctx.ReadValue<Vector2>();
            if (input.magnitude < stickDeadZone)
                return;

            EnsureSelection();
        }

        private void EnsureSelection()
        {
            if (fallbackSelected == null) 
                return;
            
            fallbackSelected.Select();
        }
    }
}