using Game._Scripts.Player.Gunner.Weapons;
using UnityEngine;

namespace Game._Scripts.Player.Gunner
{
    public class PlayerWithGun : MonoBehaviour
    {
        [SerializeField, Range(0f, 30f)] private float gunAttachmentRadius = 2f;
        [SerializeField] private Transform gunSocket;

        private UnityEngine.Camera _cam;
        private Rigidbody2D _rb;
        private PlayerWithGunInputReader _input;
        private IWeapon _weapon;
        
        private Vector2 _aimDirection;
        private Vector3 _mousePos;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _cam = UnityEngine.Camera.main;
            _input = new PlayerWithGunInputReader();
            _weapon = GetComponentInChildren<IWeapon>();
        }

        private void OnEnable()
        {
            _input.ShootEvent += OnShootEvent;
            _input.AimEvent += OnControllerAimEvent;
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.ShootEvent -= OnShootEvent;
            _input.AimEvent -= OnControllerAimEvent;
            _input.Disable();
        }

        private void Update()
        {
            if (WasMouseMoved())
                SetGunSocketPosition(_mousePos - transform.position);

            _weapon.Aim(_aimDirection);
        }

        private void OnControllerAimEvent(Vector2 aimDirection) => SetGunSocketPosition(aimDirection);
        private void OnShootEvent() => _weapon.Shoot(_rb, -_aimDirection);

        private void SetGunSocketPosition(Vector2 aimDirection)
        {
            Vector2 origin = transform.position;
            aimDirection.Normalize();

            _aimDirection = aimDirection;
            gunSocket.position = origin + aimDirection * gunAttachmentRadius;
        }

        private bool WasMouseMoved()
        {
            var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            if (_mousePos == mousePos) 
                return false;
            
            _mousePos = mousePos;
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, gunAttachmentRadius);
        }
    }
}