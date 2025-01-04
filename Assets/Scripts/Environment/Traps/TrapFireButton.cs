using System;
using UnityEngine;

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
        Player player = other.gameObject.GetComponent<Player>();

        if (player)
        {
            _anim.SetTrigger("activate");
            _trapFire.SwitchOffFire();
        }
    }
}