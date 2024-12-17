using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class RinoEnemy : Enemy
{
    [Header("Rino details")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUpRate = .6f;
    private float _defaultSpeed;
    [SerializeField] private Vector2 impactPower;
    [SerializeField] private float detectionRange;
    private bool _playerDetected;
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int HitWall = Animator.StringToHash("hitWall");

    protected override void Start()
    {
        base.Start();
        
        CanMove = false;
        _defaultSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        
        Anim.SetFloat(XVelocity, Rb.linearVelocityX);
        
        HandleCollision();
        HandleCharge();
    }

    private void HandleCharge()
    {
        if (!CanMove) return;

        moveSpeed += Time.deltaTime * speedUpRate;
        if (moveSpeed >= maxSpeed)
            maxSpeed = moveSpeed;
        
        Rb.linearVelocityX = moveSpeed * FacingDir;

        if (IsWallDetected)
            WallHit();
        
        if (!IsGroundInFront)
            TurnAround();
        
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
        moveSpeed = _defaultSpeed;
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

        _playerDetected = Physics2D.Raycast(transform.position, Vector2.right * FacingDir, detectionRange, whatIsPlayer);
        if (_playerDetected)
            CanMove = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (detectionRange * FacingDir), transform.position.y));

    }
}
