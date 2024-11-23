using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;

    [SerializeField] private float moveSpeed;

    public float xInput;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        _anim.SetBool(IsRunning, _rb.linearVelocity.x != 0);
        HandleMovement();
    }

    private void HandleMovement()
    {
        _rb.linearVelocity = new Vector2(xInput * moveSpeed, _rb.linearVelocity.y);
    }
}