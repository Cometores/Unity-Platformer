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

    private void HandleSpeedUp()
    {
        moveSpeed += Time.deltaTime * speedUpRate;
        if (moveSpeed >= maxSpeed)
            maxSpeed = moveSpeed;
        
        Rb.linearVelocityX = moveSpeed * FacingDir;
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
