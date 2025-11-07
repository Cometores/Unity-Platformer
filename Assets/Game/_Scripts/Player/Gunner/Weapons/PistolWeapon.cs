using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class PistolWeapon : MonoBehaviour, IWeapon
    {
        private readonly float _shotStrength = 10f;
        private bool _isFacingRight = true;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void Shoot(Rigidbody2D playerRb, Vector2 shootDirection)
        {
            playerRb.AddForce(shootDirection * _shotStrength, ForceMode2D.Impulse);
        }

        public void Aim(Vector2 shootDirection)
        {
            transform.right = shootDirection;
            FlipWeaponIfNeeded(shootDirection.x);
        }

        private void FlipWeaponIfNeeded(float shootDirectionX)
        {
            if ((_isFacingRight && shootDirectionX < 0)
                || (!_isFacingRight && shootDirectionX > 0))
            {
                _sr.flipY = !_sr.flipY;
                _isFacingRight = !_isFacingRight;
            }
        }
    }
}