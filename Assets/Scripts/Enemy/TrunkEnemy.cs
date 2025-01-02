using Unity.Mathematics;
using UnityEngine;

public class TrunkEnemy : Enemy
{
    [Header("Trunk details")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 7;
    
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int Attack1 = Animator.StringToHash("attack");

    private float _lastTimeAttacked;
    
    protected override void Update()
    {
        base.Update();

        if (IsDead) return;
        
        bool canAttack = Time.time > _lastTimeAttacked + attackCooldown;
        
        if (IsPlayerDetected && canAttack)
            Attack();

        HandleMovement();

        if (IsGrounded)
            HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!IsGroundInFront || IsWallDetected)
        {
            Flip();
            IdleTimer = idleDuration;
            Rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (IdleTimer > 0)
            return;

        if (IsGroundInFront)
            Rb.linearVelocityX = moveSpeed * FacingDir;
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
