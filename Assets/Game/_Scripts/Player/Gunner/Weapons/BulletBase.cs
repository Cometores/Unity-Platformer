using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public abstract class BulletBase: MonoBehaviour
    {
        private Rigidbody2D _rb;
        private TrailRenderer _trail;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _trail = GetComponentInChildren<TrailRenderer>();
        }

        public virtual void Initialize(int ricochetNums, float speed, Vector2 direction)
        {
            _rb.linearVelocity = direction * speed;
        }
        
        protected void DetachTrail(Vector2 endPosition)
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