using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class RocketBullet: BulletBase
    {

        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (enemy)
            {
                enemy.Die();
            }
            
            Destroy(gameObject);
        }
    }
}