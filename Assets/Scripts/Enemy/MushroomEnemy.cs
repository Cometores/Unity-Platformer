using System;
using UnityEngine;

public class MushroomEnemy : Enemy
{
    private BoxCollider2D _boxCollider;
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");

    protected override void Awake()
    {
        base.Awake();
        
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

        Anim.SetFloat(XVelocity, Rb.linearVelocityX);

        if (isDead) return;

        HandleCollision();
        HandleMovement();

        if (isGrounded)
            HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!IsGroundInFront || IsWallDetected)
        {
            FLip();
            idleTimer = idleDuration;
            Rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return;

        if (IsGroundInFront)
            Rb.linearVelocityX = moveSpeed * FacingDir;
    }

    public override void Die()
    {
        base.Die();
        _boxCollider.enabled = false;
    }
}