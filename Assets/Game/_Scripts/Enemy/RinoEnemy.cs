using Unity.Cinemachine;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Game._Scripts.Enemy
{
    public class RinoEnemy : Enemy
    {
        [Header("Rino details")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float speedUpRate = .6f;
        [SerializeField] private Vector2 impactPower;

        [Header("Effects")]
        [SerializeField] private Vector2 cameraShakeImpuls;

        private CinemachineImpulseSource _impulseSource;
        private float _defaultSpeed;
        private static readonly int HitWall = Animator.StringToHash("hitWall");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        protected override void Start()
        {
            base.Start();

            CanMove = false;
            _defaultSpeed = moveSpeed;
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        protected override void Update()
        {
            HandleAnimator();
            base.Update();
            HandleCharge();
        }

        private void HandleCharge()
        {
            if (!CanMove) return;

            HandleSpeedUp();

            if (IsWallDetected)
                WallHit();
        
            if (!IsGroundInFront)
                TurnAround();
        }
        
        protected virtual void HandleAnimator()
        {
            Anim.SetFloat(XVelocity, Rb.linearVelocityX);
        }

        private void HandleSpeedUp()
        {
            moveSpeed += Time.deltaTime * speedUpRate;
            if (moveSpeed >= maxSpeed)
                maxSpeed = moveSpeed;

            Rb.linearVelocityX = moveSpeed * FacingDir;
        }

        private void ShakeCameraOnWallHit()
        {
            _impulseSource.DefaultVelocity = new Vector2(cameraShakeImpuls.x * FacingDir, cameraShakeImpuls.y);
            _impulseSource.GenerateImpulse();
        }

        private void TurnAround()
        {
            moveSpeed = _defaultSpeed;
            CanMove = false;
            Rb.linearVelocity = Vector2.zero;
            Flip();
            moveSpeed = _defaultSpeed;
        }

        private void WallHit()
        {
            CanMove = false;
            
            ShakeCameraOnWallHit();
            moveSpeed = _defaultSpeed;
            Anim.SetBool(HitWall, true);
            Rb.linearVelocity = new Vector2(impactPower.x * -FacingDir, impactPower.y);
        }

        private void ChargeIsOver()
        {
            Anim.SetBool(HitWall, false);
            Invoke(nameof(Flip), 1);
        }

        protected override void HandleCollision()
        {
            base.HandleCollision();

            if (IsPlayerDetected)
                CanMove = true;
        }
    }
}