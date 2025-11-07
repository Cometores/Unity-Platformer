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
                SetGunPointForMice();

            _weapon.Aim(_aimDirection);
        }

        private void SetGunPointForMice()
        {
            Vector2 origin = transform.position;
            Vector2 dir = _mousePos - transform.position;
            dir.Normalize();

            _aimDirection = dir;
            gunSocket.position = origin + dir * gunAttachmentRadius;
        }

        private void OnControllerAimEvent(Vector2 dir)
        {
            Vector2 origin = transform.position;
            dir.Normalize();

            _aimDirection = dir;
            gunSocket.position = origin + dir * gunAttachmentRadius;
        }

        // private void SetGunSocketPosition()
        // {
        //     
        // }

        private bool WasMouseMoved()
        {
            var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            if (_mousePos != mousePos)
            {
                _mousePos = mousePos;
                return true;
            }

            return false;
        }

        private void OnShootEvent()
        {
            _weapon.Shoot(_rb, -_aimDirection);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, gunAttachmentRadius);
        }
    }
}