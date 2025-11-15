using Game._Scripts.Managers;
using Game._Scripts.Player;
using UnityEngine;

namespace Game._Scripts.Environment.Checkpoint
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

            PlayerBase player = other.GetComponent<PlayerBase>();

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
