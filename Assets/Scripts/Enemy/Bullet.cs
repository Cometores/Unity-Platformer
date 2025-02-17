using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite() => sr.flipX = !sr.flipX;

    public void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            other.GetComponent<Player>().Knockback(transform.position.x);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(gameObject, .05f);
        }

    }
}

