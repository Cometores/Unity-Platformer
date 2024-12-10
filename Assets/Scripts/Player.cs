using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
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
    private float coyoteJumpActivated = -1;
    
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

    [Header("VFX")]
    [SerializeField] private GameObject deathVfx;

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
    private static readonly int Knockback1 = Animator.StringToHash("knockback");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<CapsuleCollider2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _defaultGravityScale = _rb.gravityScale;
        RespawnFinished(false);
    }

    private void Update()
    {
        UpdateAirborneStatus();
        
        if (!_canBeControlled) return;
        if (_isKnocked) return;
        
        HandleWallSlide();
        HandleInput();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimation();
    }

    public void RespawnFinished(bool finished)
    {
        if (finished)
        {
            _rb.gravityScale = _defaultGravityScale;
            _canBeControlled = true;
            _cd.enabled = true;
        }
        else
        {
            _rb.gravityScale = 0;
            _canBeControlled = false;
            _cd.enabled = false;
        }
    }
    
    public void Knockback()
    {
        if (_isKnocked) return;
        
        StartCoroutine(KnockbackRoutine());
        _anim.SetTrigger(Knockback1);
        _rb.linearVelocity = new Vector2(knockbackPower.x * -_facingDirection, knockbackPower.y);
    }

    public void Die()
    {
        Instantiate(deathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator KnockbackRoutine()
    {
        _isKnocked = true;

        yield return new WaitForSeconds(knockbackDuration);

        _isKnocked = false;
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
        
        if(_isWallJumping)
            return;

        _rb.linearVelocityX = _xInput * moveSpeed;
    }

    #region Jump

    private void JumpButton()
    {
        bool coyoteJumpAvalible = Time.time < coyoteJumpActivated + coyouteJumpWindow;
        
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

    private void Jump() => _rb.linearVelocityY = jumpForce;

    private void DoubleJump()
    {
        _isWallJumping = false;
        _canDoubleJump = false;
        _rb.linearVelocityY = doubleJumpForce;
    }
    
    private void WallJump()
    {
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

    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;

    private void CancelCoyouteJump() => coyoteJumpActivated = 0;
    
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
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (wallCheckDistance * _facingDirection), transform.position.y));
    }
}