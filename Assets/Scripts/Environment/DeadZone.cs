using Managers;
using UnityEngine;

namespace Environment
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();
            if (player != null)
            {
                player.Damage();
                player.Die();
                GameManager.Instance.RespawnPlayer();
            }
        
            Enemy.Enemy enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
