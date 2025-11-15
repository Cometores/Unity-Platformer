using System.Collections;
using Game._Scripts.Managers;
using Game._Scripts.Utils;
using UnityEngine;

namespace Game._Scripts.Player
{
    public class Player : PlayerBase
    {
        public PlayerInput PlayerInput {get; private set; }
        
        private GameManager _gameManager;
        private CapsuleCollider2D _cd;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float doubleJumpForce;

        [Header("Buffer & Coyote Jump")]
        [SerializeField] private float bufferJumpWindow = .25f;
        [SerializeField] private float coyouteJumpWindow;
        private float _bufferJumpActivated = -1;
        private float _coyoteJumpActivated = -1;

        [Header("Wall interactions")]
        [SerializeField] private float wallJumpDuration = .6f;
        [SerializeField] private Vector2 wallJumpForce;

        [Header("Collision")]
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private LayerMask whatIsGround;

        [Space]
        [SerializeField] private LayerMask whatIsEnemy;
        [SerializeField] private Transform enemyCheck;
        [SerializeField] private float enemyCheckRadius;

        [Header("Player visuals")]
        [SerializeField] private AnimatorOverrideController[] animators;
        [SerializeField] private GameObject deathVfx;
        [SerializeField] private int skinId;

        private Vector2 _moveInput;
        
        private bool _isFacingRight = true;
        private bool _isGrounded;
        private bool _isAirborne;
        private bool _isWallDetected;
        private bool _canDoubleJump;
        private bool _isWallJumping;

        private float _defaultGravityScale;

        private int _facingDirection = 1;

        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int IsWallDetected = Animator.StringToHash("isWallDetected");

        #region Input enabling / disabling

        private void OnEnable()
        {
            PlayerInput.Player.Jump.performed += OnJumpPerformed;
            PlayerInput.Player.Movement.performed += OnMovementPerformed;
            PlayerInput.Player.Movement.canceled += OnMovementCanceled;

            EnableInput();
        }

        private void OnDisable()
        {
            PlayerInput.Player.Jump.performed -= OnJumpPerformed;
            PlayerInput.Player.Movement.performed -= OnMovementPerformed;
            PlayerInput.Player.Movement.canceled -= OnMovementCanceled;
            
            DisableInput();
        }
        
        public override void DisableInput() => PlayerInput.Disable();

        public override void EnableInput() => PlayerInput.Enable();
        
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _cd = GetComponent<CapsuleCollider2D>();
            PlayerInput = new PlayerInput();
        }
        
        protected override void Start()
        {
            base.Start();
            
            _gameManager = GameManager.Instance;
            _defaultGravityScale = Rb.gravityScale;

            RespawnFinished(false);
            LoadSkin();
        }

        private void Update()
        {
            UpdateAirborneStatus();

            if (!CanBeControlled)
            {
                HandleCollision();
                HandleAnimation();
                return;
            }

            if (IsKnocked) return;

            HandleEnemyDetection();
            HandleWallSlide();
            HandleMovement();
            HandleFlip();
            HandleCollision();
            HandleAnimation();
        }

        private void LoadSkin()
        {
            SkinManager skinManager = SkinManager.Instance;

            if (!skinManager)
                return;

            Anim.runtimeAnimatorController = animators[skinManager.ChosenSkinId];
        }

        private void HandleEnemyDetection()
        {
            if (Rb.linearVelocityY >= 0 || !enemyCheck)
                return;

            Collider2D[] detectedEnemies = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius, whatIsEnemy);
            foreach (var enemy in detectedEnemies)
            {
                Enemy.Enemy newEnemy = enemy.GetComponent<Enemy.Enemy>();
                if (newEnemy)
                {
                    AudioManager.Instance.PlaySfx(1);
                    newEnemy.Die();
                    Jump();
                }
            }
        }

        public void RespawnFinished(bool finished)
        {
            if (finished)
            {
                Rb.gravityScale = _defaultGravityScale;
                CanBeControlled = true;
                _cd.enabled = true;
                
                AudioManager.Instance.PlaySfx(11);
            }
            else
            {
                Rb.gravityScale = 0;
                CanBeControlled = false;
                _cd.enabled = false;
            }
        }

        public override void Die()
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
            base.Die();
        }

        private void HandleWallSlide()
        {
            bool canWallSlide = _isWallDetected && Rb.linearVelocityY < 0;
            if (!canWallSlide) return;

            float yModifier = _moveInput.y < 0 ? 1 : .05f;
            Rb.linearVelocityY *= yModifier;
        }

        private void UpdateAirborneStatus()
        {
            if (_isGrounded && _isAirborne)
            {
                HandleLanding();
            }

            if (!_isGrounded && !_isAirborne)
            {
                BecomeAirborne();
            }
        }

        private void BecomeAirborne()
        {
            _isAirborne = true;

            if (Rb.linearVelocityY < 0)
                ActivateCoyoteJump();
        }

        private void HandleLanding()
        {
            _canDoubleJump = true;
            _isAirborne = false;

            AttemptBufferJump();
        }

        private void HandleCollision()
        {
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
            _isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * _facingDirection, wallCheckDistance,
                whatIsGround);
        }

        private void HandleMovement()
        {
            if (_isWallDetected)
                return;

            if (_isWallJumping)
                return;

            Rb.linearVelocityX = _moveInput.x * moveSpeed;
        }
        
        private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            _moveInput = ctx.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            _moveInput = Vector2.zero;
        }

        #region Jump

        private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext _) => JumpButton();

        private void JumpButton()
        {
            if (IsKnocked) return;
            
            bool coyoteJumpAvalible = Time.time < _coyoteJumpActivated + coyouteJumpWindow;

            if (_isGrounded || coyoteJumpAvalible)
            {
                Jump();
            }
            else if (_isWallDetected && !_isGrounded)
            {
                WallJump();
            }
            else if (_canDoubleJump)
            {
                DoubleJump();
            }

            CancelCoyouteJump();
        }

        private void Jump()
        {
            AudioManager.Instance.PlaySfx(3);
            Rb.linearVelocityY = jumpForce;
        }

        private void DoubleJump()
        {
            AudioManager.Instance.PlaySfx(3);
            
            _isWallJumping = false;
            _canDoubleJump = false;
            Rb.linearVelocityY = doubleJumpForce;
        }

        private void WallJump()
        {
            AudioManager.Instance.PlaySfx(12);
            
            _canDoubleJump = true;
            Rb.linearVelocity = new Vector2(wallJumpForce.x * -_facingDirection, wallJumpForce.y);

            FLip();

            StopAllCoroutines();
            StartCoroutine(WallJumpRoutine());
        }

        private IEnumerator WallJumpRoutine()
        {
            _isWallJumping = true;
            yield return Helpers.GetWait(wallJumpDuration);
            _isWallJumping = false;
        }

        #endregion

        #region Buffer & Coyoute Jump

        private void ActivateCoyoteJump() => _coyoteJumpActivated = Time.time;

        private void CancelCoyouteJump() => _coyoteJumpActivated = 0;

        private void RequestBufferJump()
        {
            if (_isAirborne)
            {
                _bufferJumpActivated = Time.time;
            }
        }

        private void AttemptBufferJump()
        {
            if (Time.time < _bufferJumpActivated + bufferJumpWindow)
            {
                _bufferJumpActivated = 0;
                Jump();
            }
        }

        #endregion

        #region Flip

        private void HandleFlip()
        {
            if (_moveInput.x < 0 && _isFacingRight || _moveInput.x > 0 && !_isFacingRight)
                FLip();
        }

        private void FLip()
        {
            transform.Rotate(0, 180, 0);
            _isFacingRight = !_isFacingRight;
            _facingDirection *= -1;
        }

        #endregion

        private void HandleAnimation()
        {
            Anim.SetFloat(XVelocity, Rb.linearVelocityX);
            Anim.SetFloat(YVelocity, Rb.linearVelocityY);
            Anim.SetBool(IsGrounded, _isGrounded);
            Anim.SetBool(IsWallDetected, _isWallDetected);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x + (wallCheckDistance * _facingDirection), transform.position.y));
        }
    }
}