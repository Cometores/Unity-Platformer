using Unity.Mathematics;
using UnityEngine;

namespace Enemy
{
    public class PlantEnemy : Enemy
    {
        private static readonly int Attack1 = Animator.StringToHash("attack");

        [Header("Plant details")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float bulletSpeed = 7;

        private float _lastTimeAttacked;
     
        protected override void Update()
        {
            base.Update();

            bool canAttack = Time.time > _lastTimeAttacked + attackCooldown;
        
            if (IsPlayerDetected && canAttack)
                Attack();
        }

        private void Attack()
        {
            _lastTimeAttacked = Time.time;
            Anim.SetTrigger(Attack1);
        }

        private void CreateBullet()
        {
            Bullet newBullet = Instantiate(bulletPrefab, gunPoint.position, quaternion.identity);

            Vector2 bulletVelocity = new Vector2(FacingDir * bulletSpeed, 0);
            newBullet.SetVelocity(bulletVelocity);
        
            Destroy(newBullet.gameObject, 10);
        }

        protected override void HandleAnimator()
        {
            // empty because of the velocity
        }
    }
}
