using UnityEngine;

namespace Environment.Checkpoint
{
    public class StartPoint : MonoBehaviour
    {
        private static readonly int Activate = Animator.StringToHash("activate");
        private Animator anim => GetComponent<Animator>();

        private void OnTriggerExit2D(Collider2D other)
        {
            Player.Player player = other.GetComponent<Player.Player>();
        
            if(player != null)
                anim.SetTrigger(Activate);
        }
    }
}
