using Game._Scripts.Player;
using UnityEngine;

namespace Game._Scripts.Environment.Traps
{
    public class TrapFireButton : MonoBehaviour
    {
        private Animator _anim;
        private TrapFire _trapFire;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _trapFire = GetComponentInParent<TrapFire>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerBase player = other.gameObject.GetComponent<PlayerBase>();

            if (player)
            {
                _anim.SetTrigger("activate");
                _trapFire.SwitchOffFire();
            }
        }
    }
}