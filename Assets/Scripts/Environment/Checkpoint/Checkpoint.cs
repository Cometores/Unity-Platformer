using Managers;
using UnityEngine;

namespace Environment.Checkpoint
{
    public class Checkpoint : MonoBehaviour
    {
        private Animator _anim;
        private bool _active;
        private static readonly int Activate = Animator.StringToHash("activate");
        
        private void Awake() => _anim = GetComponent<Animator>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_active) return;

            Player.Player player = other.GetComponent<Player.Player>();

            if (player)
                ActivateCheckpoint();
        }

        private void ActivateCheckpoint()
        {
            _active = true;
            _anim.SetTrigger(Activate);
            PlayerManager.Instance.UpdateRespawnPoint(transform);
        }
    }
}
