using System.Collections;
using UnityEngine;

namespace Game._Scripts.Environment.Traps
{
    public class TrapFire : MonoBehaviour
    {
        [SerializeField] private float offDuration;
        [SerializeField] private TrapFireButton fireButton;
    
        private Animator _anim;
        private CapsuleCollider2D _fireCollider;
        private bool _isActive;
        private static readonly int Active = Animator.StringToHash("active");


        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _fireCollider = GetComponent<CapsuleCollider2D>();
        }

        private void Start()
        {
            if (fireButton == null)
                Debug.LogWarning("You don't have fire button on " + gameObject.name + "!");
        
            SetFire(true);
        }

        public void SwitchOffFire()
        {
            if (!_isActive)
                return;
        
            StartCoroutine(FireCoroutine());
        }

        private IEnumerator FireCoroutine()
        {
            SetFire(false);
        
            yield return Helpers.GetWait(offDuration);
        
            SetFire(true);
        }

        private void SetFire(bool active)
        {
            _anim.SetBool(Active, active);
            _fireCollider.enabled = active;
            _isActive = active;
        }
    }
}
