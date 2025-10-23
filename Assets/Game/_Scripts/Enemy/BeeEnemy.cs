using Unity.Mathematics;
using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class BeeEnemy :Enemy
    {
        [Header("Bee details")]
        [SerializeField] private BeeBulletEnemy bulletPrefab;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float bulletSpeed = 7;
        [SerializeField] private float bulletLifeTime = 2.5f;

        private Transform _target;
        
        private float _lastTimeAttacked;
        private static readonly int Attack1 = Animator.StringToHash("attack");
     
        protected override void Update()
        {
            base.Update();

            if (!_target)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, float.MaxValue, whatIsPlayer);
                
                if (hit.collider != null)
                    _target = hit.transform;
            }

            bool canAttack = Time.time > _lastTimeAttacked + attackCooldown && _target != null;
        
            if (canAttack)
                Attack();
        }

        private void Attack()
        {
            _lastTimeAttacked = Time.time;
            Anim.SetTrigger(Attack1);
        }

        private void CreateBullet()
        {
            BeeBulletEnemy newBullet = Instantiate(bulletPrefab, gunPoint.position, quaternion.identity);

            _target = null;
            Destroy(newBullet.gameObject, 10);
        }

        protected override void HandleAnimator()
        {
            // empty because of the velocity
        }
    }
}
