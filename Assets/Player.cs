using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;

    [Header("Movement")] [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    public bool canDoubleJump;

    [Header("Collision info")] [SerializeField]
    private float groundCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    private bool _isGrounded;

    private float _xInput;
    private bool _facingRight = true;
    private int _facingDirection = 1;

    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleFlip();
        HandleAnimation();
    }

    private void HandleCollision()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.linearVelocityY = jumpForce;
    }

    private void HandleAnimation()
    {
        _anim.SetFloat(XVelocity, _rb.linearVelocityX);
        _anim.SetFloat(YVelocity, _rb.linearVelocityY);
        _anim.SetBool(IsGrounded, _isGrounded);
    }

    private void HandleMovement()
    {
        _rb.linearVelocityX = _xInput * moveSpeed;
    }

    private void HandleFlip()
    {
        if (_rb.linearVelocityX < 0 && _facingRight || _rb.linearVelocityX > 0 && !_facingRight)
            FLip();
    }

    private void FLip()
    {
        transform.Rotate(0, 180, 0);
        _facingRight = !_facingRight;
        _facingDirection *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}