using UnityEngine;

public class MushroomEnemy : Enemy
{
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");

    protected override void Update()
    {
        base.Update();

        Anim.SetFloat(XVelocity, Rb.linearVelocityX);

        if (IsDead) return;

        HandleCollision();
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
}