using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class PistolBullet: MonoBehaviour
    {
        private Rigidbody2D _rb;
        private int _ricochetNums = 3;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(int ricochetNums, float speed, Vector2 direction)
        {
            _ricochetNums = ricochetNums;
            _rb.linearVelocity = direction * speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _ricochetNums--;
            
            if (_ricochetNums <= 0)
                Destroy(gameObject);
        }
    }
}