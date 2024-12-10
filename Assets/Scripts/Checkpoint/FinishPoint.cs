using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private static readonly int Activate = Animator.StringToHash("activate");
    private Animator _anim => GetComponent<Animator>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            _anim.SetTrigger(Activate);
            Debug.Log("Level completed!");
        }
    }
}
