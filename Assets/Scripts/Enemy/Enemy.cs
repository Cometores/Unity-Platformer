using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    protected Animator Anim;
    protected Rigidbody2D Rb;

    [SerializeField] private GameObject damageTrigger;
    [Space]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float idleDuration = 1.5f;
    protected float idleTimer;
    
    [FormerlySerializedAs("deathImpact")]
    [Header("Death details")]
    [SerializeField] private float deathImpactSpeed = 5;
    [SerializeField] private float deathRotationSpeed = 150;
    private int _deathRotationDirection = 1;
    protected bool isDead;
    
    [Header("Basic collision")]
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = .7f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    protected bool isGrounded;
    protected bool IsWallDetected;
    protected bool IsGroundInFront;

    protected int FacingDir = -1;
    protected bool FacingRight = false;
    private static readonly int Hit = Animator.StringToHash("hit");

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;
        
        if (isDead)
            HandleDeathRotation();
    }

    public virtual void Die()
    {
        damageTrigger.SetActive(false);
        Anim.SetTrigger(Hit);
        Rb.linearVelocityY = deathImpactSpeed;
        isDead = true;

        if (Random.Range(0, 100) < 50)
            _deathRotationDirection *= -1;
    }

    private void HandleDeathRotation()
    {
        transform.Rotate(0, 0, (deathRotationSpeed * _deathRotationDirection) * Time.deltaTime);
    }
    
    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        IsGroundInFront = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        IsWallDetected = Physics2D.Raycast(transform.position, Vector2.right * FacingDir, wallCheckDistance,
            whatIsGround);
    }
    
    protected virtual void HandleFlip(float xValue)
    {
        if (xValue < 0 && FacingRight || xValue > 0 && !FacingRight)
            FLip();
    }

    protected virtual void FLip()
    {
        transform.Rotate(0, 180, 0);
        FacingRight = !FacingRight;
        FacingDir *= -1;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (wallCheckDistance * FacingDir), transform.position.y));
    }
}
