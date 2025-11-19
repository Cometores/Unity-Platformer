using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class ChickenEnemy : Enemy
    {
        [Header("Chicken details")]
        [SerializeField] private float aggroDuration;
        [SerializeField] private float detectionRange;
        
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        private float _aggroTimer;
        private bool _canFlip = true;

        protected override void Update()
        {
            base.Update();
        
            if (IsDead) return;
            HandleAnimator();
        
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
        
        protected virtual void HandleAnimator()
        {
            Anim.SetFloat(XVelocity, Rb.linearVelocityX);
        }

        private void HandleMovement()
        {
            if (!CanMove) return;

            if (Player)
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

        public override void Flip()
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