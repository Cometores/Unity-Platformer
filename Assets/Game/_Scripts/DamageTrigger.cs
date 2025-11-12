using Game._Scripts.Player;
using UnityEngine;

namespace Game._Scripts
{
    public class DamageTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerBase player = other.gameObject.GetComponent<PlayerBase>();

            if (player)
            {
                player.GetDamage();
                player.Knockback(transform.position.x);
            }
        }
    }
}
