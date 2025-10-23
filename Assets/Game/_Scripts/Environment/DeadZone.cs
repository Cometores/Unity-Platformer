using Game._Scripts.Managers;
using UnityEngine;

namespace Game._Scripts.Environment
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            TryKillPlayer(other);
            TryKillEnemy(other);
        }

        private static void TryKillEnemy(Collider2D other)
        {
            Enemy.Enemy enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (enemy)
            {
                enemy.Die();
            }
        }

        private static void TryKillPlayer(Collider2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();
            if (player)
            {
                player.Damage();
                player.Die();
                PlayerManager.Instance.RespawnPlayer();
            }
        }
    }
}
