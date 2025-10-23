using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class MushroomEnemy : Enemy
    {
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        protected override void Update()
        {
            base.Update();

            if (IsDead) return;

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
}