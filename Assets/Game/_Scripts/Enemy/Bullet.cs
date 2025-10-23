using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        [SerializeField] private string playerLayerName = "Player";
        [SerializeField] private string groundLayerName = "Ground";
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        public void FlipSprite() => _sr.flipX = !_sr.flipX;

        public void SetVelocity(Vector2 velocity) => _rb.linearVelocity = velocity;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
            {
                other.GetComponent<Player.Player>().Knockback(transform.position.x);
                Destroy(gameObject);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
            {
                Destroy(gameObject, .05f);
            }

        }
    }
}

