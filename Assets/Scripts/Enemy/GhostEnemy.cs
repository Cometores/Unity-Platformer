using Managers;
using UnityEngine;

namespace Enemy
{
    public class GhostEnemy : Enemy
    {
        [Header("Ghost details")]
        [SerializeField] private float activeDuration;
        [Space]
        [SerializeField] private float xMinDistance;
        [SerializeField] private float yMinDistance;
        [SerializeField] private float yMaxDistance;
        
        private float _activeTimer;
        private bool _isChasing;
        private Transform _target;
        
        private static readonly int Appear = Animator.StringToHash("appear");
        private static readonly int Desappear = Animator.StringToHash("desappear");

        protected override void Update()
        {
            base.Update();
            
            if (IsDead) return;
            
            _activeTimer -= Time.deltaTime;

            if (!_isChasing && IdleTimer < 0)
            {
                StartChase();
            }
            else if (_isChasing && _activeTimer < 0)
            {
                EndChase();
            }
            
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (!CanMove)
                return;
            
            HandleFlip(_target.position.x);
            transform.position = Vector2.MoveTowards(transform.position, _target.position, moveSpeed * Time.deltaTime);
        }
        
        private void StartChase()
        {
            var player = PlayerManager.Instance.player;

            if (!player)
            {
                EndChase();
                return;
            }
            
            _target = player.transform;
            float xOffset = Random.Range(0, 100) < 50 
                ? -1 
                : 1;
            float yPosition = Random.Range(yMinDistance, yMaxDistance);
            
            transform.position = _target.position + new Vector3(xOffset * xMinDistance, yPosition, 0);
            
            
            _activeTimer = activeDuration;
            _isChasing = true;
            Anim.SetTrigger(Appear);
        }
        
        private void EndChase()
        {
            IdleTimer = idleDuration;
            _isChasing = false;
            Anim.SetTrigger(Desappear);
        }
        
        /// <summary>
        /// Used as an animation event
        /// </summary>
        private void MakeInvisible()
        {
            Sr.color = Color.clear;
            EnableColliders(false);
        }

        /// <summary>
        /// Used as an animation event
        /// </summary>
        private void MakeVisible()
        {
            Sr.color = Color.white;
            EnableColliders(true);
        }

        public override void Die()
        {
            base.Die();
            CanMove = false;
        }
    }
}
