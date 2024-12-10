using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator _anim => GetComponent<Animator>();
    private bool _active;
    private static readonly int Activate = Animator.StringToHash("activate");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_active) return;

        Player player = other.GetComponent<Player>();

        if (player)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        _active = true;
        _anim.SetTrigger(Activate);
        GameManager.Instance.UpdateRespawnPoint(transform);
    }
}
