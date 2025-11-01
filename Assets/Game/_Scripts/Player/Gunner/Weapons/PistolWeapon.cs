using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public class PistolWeapon : IWeapon
    {
        private readonly float _shotStrength = 10f;
        
        public void Shoot(Rigidbody2D playerRb, Vector2 shootDirection)
        {
            playerRb.AddForce(shootDirection * _shotStrength, ForceMode2D.Impulse);
        }
    }
}