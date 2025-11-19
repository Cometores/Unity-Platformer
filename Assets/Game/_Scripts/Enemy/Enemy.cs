using Game._Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game._Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        protected SpriteRenderer Sr => GetComponent<SpriteRenderer>();
        protected Transform Player;
        public Animator Anim;
        public Rigidbody2D Rb;
        protected Collider2D[] Colliders;

        [Header("General info")]
        [SerializeField]
        public float moveSpeed = 2f;
        [SerializeField] public float idleDuration = 1.5f;
        protected bool CanMove = true;
        public float IdleTimer;

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
        public bool IsGrounded;
        public bool IsWallDetected;
        public bool IsGroundInFront;

        public int FacingDir = -1;
        protected bool FacingRight = false;
    
        protected static readonly int Hit = Animator.StringToHash("hit");

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

            PlayerManager.OnPlayerRespawn += UpdatePlayerReference;
            UpdatePlayerReference();
        }

        private void UpdatePlayerReference()
        {
            // if (!Player) 
            Player = PlayerManager.Instance.player.transform;
        }

        protected virtual void Update()
        {
            HandleCollision();

            IdleTimer -= Time.deltaTime;

            if (IsDead)
                HandleDeathRotation();
        }

        public virtual void Die()
        {
            if (Rb.bodyType == RigidbodyType2D.Kinematic)
                Rb.bodyType = RigidbodyType2D.Dynamic;
            
            EnableColliders(false);

            Anim.SetTrigger(Hit);
            Rb.linearVelocityY = deathImpactSpeed;
            IsDead = true;

            if (Random.Range(0, 100) < 50)
                DeathRotationDirection *= -1;
        
            PlayerManager.OnPlayerRespawn -= UpdatePlayerReference;
            Destroy(gameObject, 10);
        }

        protected void EnableColliders(bool isEnabled)
        {
            foreach (var col in Colliders) 
                col.enabled = isEnabled;
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

        public virtual void Flip()
        { 
            if (IsDead) return;
            
            transform.Rotate(0, 180, 0);
            FacingRight = !FacingRight;
            FacingDir *= -1;
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