using System;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private static readonly int Activate = Animator.StringToHash("activate");
    private Animator anim => GetComponent<Animator>();

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        
        if(player != null)
            anim.SetTrigger(Activate);
    }
}
