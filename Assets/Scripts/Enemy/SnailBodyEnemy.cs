using System;
using UnityEngine;

public class SnailBodyEnemy : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private float _zRotation;

    public void SetupBody(float yVelocity, float zRotation, int facingDir)
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        
        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, yVelocity);
        _zRotation = zRotation;

        if (facingDir == 1)
            _sr.flipX = true;
    }

    private void Update()
    {
        transform.Rotate(0, 0, _zRotation * Time.deltaTime);
    }
}
