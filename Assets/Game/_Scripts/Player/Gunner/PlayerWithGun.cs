using Game._Scripts.Player.Gunner.Weapons;
using UnityEngine;

namespace Game._Scripts.Player.Gunner
{
    public class PlayerWithGun : PlayerBase
    {
        [SerializeField, Range(0f, 30f)] private float gunAttachmentRadius = 2f;
        [SerializeField] private Transform gunSocket;

        private UnityEngine.Camera _cam;
        private Rigidbody2D _rb;
        private PlayerWithGunInputReader _input;
        
        private WeaponBase[] _weapons;
        private WeaponBase _activeWeapon;
        private int _weaponNums;
        private int _currentWeaponIndex;
        
        private Vector2 _aimDirection;
        private Vector3 _mousePos;

        protected override void Awake()
        {
            base.Awake();
            
            _rb = GetComponent<Rigidbody2D>();
            _cam = UnityEngine.Camera.main;
            _input = new PlayerWithGunInputReader();
            
            _weapons = GetComponentsInChildren<WeaponBase>(true);
            _weaponNums = _weapons.Length;
            _currentWeaponIndex = 0;
            _activeWeapon = _weapons[0];
        }

        #region Input enabling / disabling

        private void OnEnable()
        {
            _input.ShootEvent += OnShootEvent;
            _input.AimEvent += OnControllerAimEvent;
            _input.WeaponChangeEvent += OnWeaponChangeEvent;
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.ShootEvent -= OnShootEvent;
            _input.AimEvent -= OnControllerAimEvent;
            _input.WeaponChangeEvent -= OnWeaponChangeEvent;
            _input.Disable();
        }
        
        public override void DisableInput() => _input.Disable();

        public override void EnableInput() => _input.Enable();

        #endregion

        private void Update()
        {
            if (WasMouseMoved())
                SetGunSocketPosition(_mousePos - transform.position);

            _activeWeapon.Aim(_aimDirection);
        }

        private void OnControllerAimEvent(Vector2 aimDirection) => SetGunSocketPosition(aimDirection);
        
        private void OnShootEvent()
        {
            if (IsKnocked)
                return;
            
            _activeWeapon.Shoot(_rb, -_aimDirection);
        }

        private void OnWeaponChangeEvent(int direction)
        {
            _activeWeapon.gameObject.SetActive(false);
            
            _currentWeaponIndex = (_currentWeaponIndex + direction) % _weaponNums;
            if (_currentWeaponIndex == -1)
                _currentWeaponIndex = _weaponNums - 1;
            _activeWeapon = _weapons[_currentWeaponIndex];
            
            _activeWeapon.gameObject.SetActive(true);
        }

        private void SetGunSocketPosition(Vector2 aimDirection)
        {
            Vector2 origin = transform.position;
            aimDirection.Normalize();

            _aimDirection = aimDirection;
            gunSocket.position = origin + aimDirection * gunAttachmentRadius;
        }

        private bool WasMouseMoved()
        {
            // TODO: Doesn't work with cinemachine
            
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