using System;
using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class PistolBullet: BulletBase
    {
        private int _ricochetNums = 3;

        public override void Initialize(int ricochetNums, float speed, Vector2 direction)
        {
            base.Initialize(ricochetNums, speed, direction);
            
            _ricochetNums = ricochetNums;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (enemy)
            {
                enemy.Die();
            }
            
            _ricochetNums--;

            if (_ricochetNums <= 0)
            {
                DetachTrail(other.GetContact(0).point);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}