using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class ChickenEnemy : Enemy
    {
        [Header("Chicken details")]
        [SerializeField] private float aggroDuration;
        [SerializeField] private float detectionRange;

        private float _aggroTimer;
        private bool _canFlip = true;

        protected override void Update()
        {
            base.Update();
        
            if (IsDead) return;
        
            _aggroTimer -= Time.deltaTime;

            if (IsPlayerDetected)
            {
                CanMove = true;
                _aggroTimer = aggroDuration;
            }
        
            if (_aggroTimer < 0)
            {
                CanMove = false;
            }

            HandleMovement();

            if (IsGrounded)
                HandleTurnAround();
        }

        private void HandleTurnAround()
        {
            if (!IsGroundInFront || IsWallDetected)
            {
                Flip();
                CanMove = false;
                Rb.linearVelocity = Vector2.zero;
            }
        }

        private void HandleMovement()
        {
            if (!CanMove) return;

            HandleFlip(Player.transform.position.x);

            if (IsGroundInFront)
                Rb.linearVelocityX = moveSpeed * FacingDir;
        }

        protected override void HandleFlip(float xValue)
        {
            if (xValue < transform.position.x && FacingRight || xValue > transform.position.x && !FacingRight)
            {
                if (_canFlip)
                {
                    _canFlip = false;
                    Invoke(nameof(Flip), .3f);
                }
            }
        }

        protected override void Flip()
        {
            base.Flip();

            _canFlip = true;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x + (detectionRange * FacingDir), transform.position.y));

        }
    }
}