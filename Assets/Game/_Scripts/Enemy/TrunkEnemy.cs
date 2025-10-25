using Game._Scripts.Enemy.Movement;
using Unity.Mathematics;
using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class TrunkEnemy : Enemy
    {
        [Header("Trunk details")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float bulletSpeed = 7;
    
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private float _lastTimeAttacked;
        private IMovement _groundMovement;

        protected override void Awake()
        {
            base.Awake();
            _groundMovement = new GroundMovement(this);
        }
        
        protected override void Update()
        {
            base.Update();

            if (IsDead) return;
        
            bool canAttack = Time.time > _lastTimeAttacked + attackCooldown;
        
            if (IsPlayerDetected && canAttack)
                Attack();

            if (IdleTimer < 0)
                _groundMovement.ManageEnemyMovement();
        }
    
        private void Attack()
        {
            IdleTimer = idleDuration + attackCooldown;
            _lastTimeAttacked = Time.time;
            Anim.SetTrigger(Attack1);
        }

        private void CreateBullet()
        {
            Bullet newBullet = Instantiate(bulletPrefab, gunPoint.position, quaternion.identity);

            Vector2 bulletVelocity = new Vector2(FacingDir * bulletSpeed, 0);
            newBullet.SetVelocity(bulletVelocity);
        
            if (FacingDir == 1)
                newBullet.FlipSprite();
        
            Destroy(newBullet.gameObject, 10);
        }
    }
}
