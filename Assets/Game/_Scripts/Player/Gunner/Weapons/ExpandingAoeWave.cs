using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class ExpandingAoeWave : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _maxRadius = 5f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private LayerMask _targetMask;

        private float _elapsed;
        private float _currentRadius;

        public float CurrentRadius => _currentRadius;
        public float Duration => _duration;
        public float Elapsed => _elapsed;

        private void Update()
        {
            Expand();
            Damage();
        }

        private void Expand()
        {
            _elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsed / _duration);
            _currentRadius = Mathf.Lerp(0f, _maxRadius, t);

            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }

        private void Damage()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, _currentRadius, _targetMask);

            foreach (var hit in hits)
            {
                var enemy = hit.GetComponent<Enemy.Enemy>();
                if (enemy == null) continue;

                enemy.Die();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _currentRadius);
        }
    }
}