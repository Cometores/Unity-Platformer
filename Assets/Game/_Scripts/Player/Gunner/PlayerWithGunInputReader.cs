using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game._Scripts.Player.Gunner
{
    /// <summary>
    /// Handles input actions for a player character equipped with a gun.
    /// Subscribe to events.
    /// </summary>
    public class PlayerWithGunInputReader : PlayerWithGunInput.IPlayerActions
    {
        public event UnityAction ShootEvent;
        public event UnityAction<Vector2> AimEvent;
        
        private readonly PlayerWithGunInput _input;
        private readonly float _stickDeadZone = 0.2f;

        public PlayerWithGunInputReader()
        {
            _input = new PlayerWithGunInput();
            _input.Player.SetCallbacks(this);
        }
        
        public void Enable() => _input.Enable();
        public void Disable() => _input.Disable();

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
                ShootEvent?.Invoke();
        }

        public void OnControllerAim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var inputVector = context.ReadValue<Vector2>();
                if (inputVector.magnitude > _stickDeadZone)
                    AimEvent?.Invoke(inputVector);
            }
        }
    }
}