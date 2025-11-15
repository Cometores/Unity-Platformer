using Game._Scripts.Player;
using UnityEngine;

namespace Game._Scripts.Environment.Checkpoint
{
    public class StartPoint : MonoBehaviour
    {
        private static readonly int Activate = Animator.StringToHash("activate");
        private Animator _anim;

        private void Awake() => _anim = GetComponent<Animator>();

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerBase player = other.GetComponent<PlayerBase>();
        
            if(player)
                _anim.SetTrigger(Activate);
        }
    }
}
