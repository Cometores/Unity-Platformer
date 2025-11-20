using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class RocketBullet: BulletBase
    {
        [SerializeField] private GameObject _aoeDamage;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (enemy)
            {
                enemy.Die();
            }
            
            DetachTrail(other.GetContact(0).point);
            Instantiate(_aoeDamage, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}