using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class SnailEnemy : Enemy
    {
        [Header("Snail details")]
        [SerializeField] private SnailBodyEnemy bodyPrefab;
        [SerializeField] private float maxSpeed = 10;
        private bool _hasBody = true;
        private static readonly int WallHit = Animator.StringToHash("wallHit");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        protected override void Update()
        {
            HandleAnimator();
            
            base.Update();

            if (IsDead) return;

            HandleMovement();

            if (IsGrounded)
                HandleTurnAround();
        }

        public override void Die()
        {
            if (_hasBody)
            {
                CanMove = false;
                _hasBody = false;
                Anim.SetTrigger(Hit);

                Rb.linearVelocity = Vector2.zero;
                idleDuration = 0;
            }
            else if(CanMove == false && _hasBody == false)
            {
                Anim.SetTrigger(Hit);
                CanMove = true;
                moveSpeed = maxSpeed;
            }
            else
            {
                base.Die();
            }
        }
        
        protected virtual void HandleAnimator()
        {
            Anim.SetFloat(XVelocity, Rb.linearVelocityX);
        }

        private void HandleTurnAround()
        {
            bool canFlipFromLedge = !IsGroundInFront && _hasBody;
            if (canFlipFromLedge || IsWallDetected)
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
        
            if (CanMove == false)
                return;

            if (IsGroundInFront)
                Rb.linearVelocityX = moveSpeed * FacingDir;
        }

        private void CreateBody()
        {
            var body = Instantiate(bodyPrefab, transform.position, Quaternion.identity);

            if (Random.Range(0, 100) < 50)
                DeathRotationDirection = DeathRotationDirection * -1;
        
            body.SetupBody(deathImpactSpeed, deathRotationSpeed * DeathRotationDirection, FacingDir);
        
            Destroy(body, 10);
        }

        public override void Flip()
        {
            base.Flip();

            if (_hasBody == false)
            {
                Anim.SetTrigger(WallHit);
            }
        }
    }
}