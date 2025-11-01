using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    public interface IWeapon
    {
        public void Shoot(Rigidbody2D playerRb, Vector2 shootDirection);
    }
}