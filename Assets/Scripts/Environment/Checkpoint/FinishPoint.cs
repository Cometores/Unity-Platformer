using Managers;
using UnityEngine;

namespace Environment.Checkpoint
{
    public class FinishPoint : MonoBehaviour
    {
        private static readonly int Activate = Animator.StringToHash("activate");
        private Animator _anim => GetComponent<Animator>();
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            Player.Player player = other.GetComponent<Player.Player>();

            if (player)
            {
                AudioManager.Instance.PlaySfx(2);
                
                _anim.SetTrigger(Activate);
                GameManager.Instance.LoadNextLevel();
            }
        }
    }
}
