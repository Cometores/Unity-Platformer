using System.Collections;
using Managers;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private DifficultyType _gameDifficulty;
        private GameManager _gameManager;

        private Rigidbody2D _rb;
        private Animator _anim;
        private CapsuleCollider2D _cd;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;

        [SerializeField] private float jumpForce;
        [SerializeField] private float doubleJumpForce;

        [Header("Buffer & Coyote Jump")]
        [SerializeField] private float bufferJumpWindow = .25f;

        private float _bufferJumpActivated = -1;
        [SerializeField] private float coyouteJumpWindow;
        private float _coyoteJumpActivated = -1;

        [Header("Wall interactions")]
        [SerializeField] private float wallJumpDuration = .6f;

        [SerializeField] private Vector2 wallJumpForce;

        [Header("Knockback")]
        [SerializeField] private float knockbackDuration;

        [SerializeField] private Vector2 knockbackPower;

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

        private bool _isFacingRight = true;
        private bool _isGrounded;
        private bool _isAirborne;
        private bool _isWallDetected;
        private bool _canDoubleJump;
        private bool _isWallJumping;
        private bool _isKnocked;
        private bool _canBeControlled = false;

        private float _xInput;
        private float _yInput;

        private float _defaultGravityScale;

        private int _facingDirection = 1;

        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int IsWallDetected = Animator.StringToHash("isWallDetected");
        private static readonly int IsKnocked = Animator.StringToHash("isKnocked");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _cd = GetComponent<CapsuleCollider2D>();
            _anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _defaultGravityScale = _rb.gravityScale;

            SetGameDifficulty();
            RespawnFinished(false);
            LoadSkin();
        }

        private void Update()
        {
            UpdateAirborneStatus();

            if (!_canBeControlled)
            {
                HandleCollision();
                HandleAnimation();
                return;
            }

            if (_isKnocked) return;

            HandleEnemyDetection();
            HandleWallSlide();
            HandleInput();
            HandleMovement();
            HandleFlip();
            HandleCollision();
            HandleAnimation();
        }

        private void LoadSkin()
        {
            SkinManager skinManager = SkinManager.instance;

            if (!skinManager)
                return;

            _anim.runtimeAnimatorController = animators[skinManager.ChosenSkinId];
        }

        public void Damage()
        {
            if (_gameDifficulty == DifficultyType.Normal)
            {
                if (_gameManager.FruitsCollected() <= 0)
                    _gameManager.RestartLevel();
                else
                    _gameManager.RemoveFruit();

                return;
            }

            if (_gameDifficulty == DifficultyType.Hard)
            {
                _gameManager.RestartLevel();
            }
        }

        private void SetGameDifficulty()
        {
            DifficultyManager difficultyManager = DifficultyManager.instance;
            if (difficultyManager != null)
                _gameDifficulty = difficultyManager.difficulty;
        }

        private void HandleEnemyDetection()
        {
            if (_rb.linearVelocityY >= 0 || !enemyCheck)
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
                _rb.gravityScale = _defaultGravityScale;
                _canBeControlled = true;
                _cd.enabled = true;
                
                AudioManager.Instance.PlaySfx(11);
            }
            else
            {
                _rb.gravityScale = 0;
                _canBeControlled = false;
                _cd.enabled = false;
            }
        }

        public void Knockback(float sourceDamageXPosition)
        {
            if (_isKnocked) return;
            
            AudioManager.Instance.PlaySfx(9);
            
            float knockbackDir = transform.position.x < sourceDamageXPosition ? -1 : 1;

            CameraManager.Instance.CameraShake(knockbackDir);

            StartCoroutine(KnockbackRoutine());
            _rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        }

        public void Die()
        {
            AudioManager.Instance.PlaySfx(8);
            
            Instantiate(deathVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void Push(Vector2 direction, float duration = 0)
        {
            StartCoroutine(PushCoroutine(direction, duration));
        }

        private IEnumerator PushCoroutine(Vector2 direction, float duration)
        {
            _canBeControlled = false;

            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(direction, ForceMode2D.Impulse);

            yield return new WaitForSeconds(duration);

            _canBeControlled = true;
        }

        private IEnumerator KnockbackRoutine()
        {
            _isKnocked = true;
            _anim.SetBool(IsKnocked, true);

            yield return new WaitForSeconds(knockbackDuration);

            _isKnocked = false;
            _anim.SetBool(IsKnocked, false);
        }

        private void HandleWallSlide()
        {
            bool canWallSlide = _isWallDetected && _rb.linearVelocityY < 0;
            if (!canWallSlide) return;

            float yModifier = _yInput < 0 ? 1 : .05f;
            _rb.linearVelocityY *= yModifier;
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

            if (_rb.linearVelocityY < 0)
                ActivateCoyoteJump();
        }

        private void HandleLanding()
        {
            _canDoubleJump = true;
            _isAirborne = false;

            AttemptBufferJump();
        }

        private void HandleInput()
        {
            _xInput = Input.GetAxisRaw("Horizontal");
            _yInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpButton();
                RequestBufferJump();
            }
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

            _rb.linearVelocityX = _xInput * moveSpeed;
        }

        #region Jump

        private void JumpButton()
        {
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
            _rb.linearVelocityY = jumpForce;
        }

        private void DoubleJump()
        {
            AudioManager.Instance.PlaySfx(3);
            
            _isWallJumping = false;
            _canDoubleJump = false;
            _rb.linearVelocityY = doubleJumpForce;
        }

        private void WallJump()
        {
            AudioManager.Instance.PlaySfx(12);
            
            _canDoubleJump = true;
            _rb.linearVelocity = new Vector2(wallJumpForce.x * -_facingDirection, wallJumpForce.y);

            FLip();

            StopAllCoroutines();
            StartCoroutine(WallJumpRoutine());
        }

        private IEnumerator WallJumpRoutine()
        {
            _isWallJumping = true;
            yield return new WaitForSeconds(wallJumpDuration);
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
            if (_xInput < 0 && _isFacingRight || _xInput > 0 && !_isFacingRight)
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
            _anim.SetFloat(XVelocity, _rb.linearVelocityX);
            _anim.SetFloat(YVelocity, _rb.linearVelocityY);
            _anim.SetBool(IsGrounded, _isGrounded);
            _anim.SetBool(IsWallDetected, _isWallDetected);
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