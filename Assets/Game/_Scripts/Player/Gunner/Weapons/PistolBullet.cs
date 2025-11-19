using System;
using System.Collections;
using Game._Scripts.Utils;
using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class PistolBullet: MonoBehaviour
    {
        private Rigidbody2D _rb;
        private TrailRenderer _trail;
        private int _ricochetNums = 3;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _trail = GetComponentInChildren<TrailRenderer>();
        }

        public void Initialize(int ricochetNums, float speed, Vector2 direction)
        {
            _ricochetNums = ricochetNums;
            _rb.linearVelocity = direction * speed;
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
        
        private void DetachTrail(Vector2 endPosition)
        {
            _trail.transform.parent = null;
            _trail.transform.position = endPosition;
            _trail.autodestruct = true;
            _trail.emitting = false;
            _trail.endColor = Color.clear;
        }

        private void OnBecameInvisible() => Destroy(gameObject);
    }
}