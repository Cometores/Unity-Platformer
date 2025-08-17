using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        private SpriteRenderer Sr => GetComponent<SpriteRenderer>();
        protected Transform Player;
        protected Animator Anim;
        protected Rigidbody2D Rb;
        protected Collider2D[] Colliders;

        [Header("General info")]
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected float idleDuration = 1.5f;
        protected bool CanMove = true;
        protected float IdleTimer;

        [Header("Death details")]
        [SerializeField] protected float deathImpactSpeed = 5;
        [SerializeField] protected float deathRotationSpeed = 150;
        protected int DeathRotationDirection = 1;
        protected bool IsDead;

        [Header("Basic collision")]
        [SerializeField] protected float groundCheckDistance = 1.1f;
        [SerializeField] protected float wallCheckDistance = .7f;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected Transform groundCheck;
        [SerializeField] protected float playerDetectionDistance = 15;

        protected bool IsPlayerDetected;
        protected bool IsGrounded;
        protected bool IsWallDetected;
        protected bool IsGroundInFront;

        protected int FacingDir = -1;
        protected bool FacingRight = false;
    
        protected static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            Colliders = GetComponentsInChildren<Collider2D>();
        }

        protected virtual void Start()
        {
            if (Sr.flipX == true && !FacingRight)
            {
                Sr.flipX = false;
                Flip();
            }

            UpdatePlayerReference();
            PlayerManager.OnPlayerRespawn += UpdatePlayerReference;
        }

        private void UpdatePlayerReference()
        {
            if (!Player)
                Player = PlayerManager.Instance.player.transform;
        }

        protected virtual void Update()
        {
            HandleCollision();
            HandleAnimator();

            IdleTimer -= Time.deltaTime;

            if (IsDead)
                HandleDeathRotation();
        }

        public virtual void Die()
        {
            foreach (var col in Colliders) 
                col.enabled = false;

            Anim.SetTrigger(Hit);
            Rb.linearVelocityY = deathImpactSpeed;
            IsDead = true;

            if (Random.Range(0, 100) < 50)
                DeathRotationDirection *= -1;
        
            PlayerManager.OnPlayerRespawn -= UpdatePlayerReference;
            Destroy(gameObject, 10);
        }

        private void HandleDeathRotation()
        {
            transform.Rotate(0, 0, (deathRotationSpeed * DeathRotationDirection) * Time.deltaTime);
        }

        protected virtual void HandleCollision()
        {
            IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
            IsGroundInFront = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
            IsWallDetected = Physics2D.Raycast(transform.position, Vector2.right * FacingDir, wallCheckDistance,
                whatIsGround);
            IsPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * FacingDir, playerDetectionDistance,
                whatIsPlayer);
        }

        protected virtual void HandleFlip(float xValue)
        {
            if (xValue < transform.position.x && FacingRight || xValue > transform.position.x && !FacingRight)
                Flip();
        }

        protected virtual void Flip()
        {
            transform.Rotate(0, 180, 0);
            FacingRight = !FacingRight;
            FacingDir *= -1;
        }

        protected virtual void HandleAnimator()
        {
            Anim.SetFloat(XVelocity, Rb.linearVelocityX);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
            Gizmos.DrawLine(groundCheck.position,
                new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x + (wallCheckDistance * FacingDir), transform.position.y));

            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x + (playerDetectionDistance * FacingDir), transform.position.y));
        }

        [ContextMenu("Change Facing Direction")]
        public void FlipDefaultFacingDirection()
        {
            Sr.flipX = !Sr.flipX;
        }
    }
}