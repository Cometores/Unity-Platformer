using UnityEngine;

namespace Game._Scripts.Environment.Traps
{
    public class TrapSpikeBalll : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D spikeRigidbody;
        [SerializeField] private float pushForce;

        private void Start()
        {
            spikeRigidbody.AddForceX(pushForce, ForceMode2D.Impulse);
        }
    }
}
