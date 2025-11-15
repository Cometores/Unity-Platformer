using Game._Scripts.Player;
using UnityEngine;

namespace Game._Scripts.Environment.Traps
{
    public class TrapTrampoline : MonoBehaviour
    {
        private Animator _anim;

        [SerializeField] private float pushPower;
        [SerializeField] private float duration =.5f;
    
        private static readonly int Activate = Animator.StringToHash("activate");

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerBase player = other.gameObject.GetComponent<PlayerBase>();

            if (player)
            {
                player.Push(transform.up * pushPower, duration);
                _anim.SetTrigger(Activate);
            }
        }
    }
}
