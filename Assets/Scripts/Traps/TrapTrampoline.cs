using UnityEngine;

public class TrapTrampoline : MonoBehaviour
{
    private Animator _anim;

    [SerializeField] private float pushPower;
    [SerializeField] private float duration =.5f;
    
    private static readonly int Activate = Animator.StringToHash("activate");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player)
        {
            player.Push(transform.up * pushPower, duration);
            _anim.SetTrigger(Activate);
        }
    }
}
