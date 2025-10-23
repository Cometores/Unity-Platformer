using UnityEngine;

namespace Game._Scripts
{
    public class DamageTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();

            if (player)
            {
                player.Damage();
                player.Knockback(transform.position.x);
            }
        }
    }
}
