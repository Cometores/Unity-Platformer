using Game._Scripts.Managers;
using Game._Scripts.Player;
using UnityEngine;

namespace Game._Scripts.Environment.Checkpoint
{
    public class FinishPoint : MonoBehaviour
    {
        private static readonly int Activate = Animator.StringToHash("activate");
        private Animator _anim;

        private void Awake() => _anim = GetComponent<Animator>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerBase player = other.GetComponent<PlayerBase>();

            if (player)
            {
                AudioManager.Instance.PlaySfx(2);
                
                _anim.SetTrigger(Activate);
                GameManager.Instance.LoadNextLevel();
            }
        }
    }
}
