using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class BatEnemy : Enemy
    {
        [Header("Bat details")]
        [SerializeField] private float attackSpeed;
        [SerializeField] private float agroRadius = 7;
        [SerializeField] private float chaseDuration = 1.5f;
        
        private float _defaultSpeed;
        private float _chaseTimer;
        private Vector3 _originalPosition;
        private Vector3 _destination;
        private bool _canDetectPlayer = true;
        private Collider2D _target;
        
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        protected override void Awake()
        {
            base.Awake();
            
            _originalPosition = transform.position;
            _defaultSpeed = moveSpeed;
            CanMove = false;
        }

        protected override void Update()
        {
            base.Update();
            
            _chaseTimer -= Time.deltaTime;
            
            if (IdleTimer < 0)
                _canDetectPlayer = true;

            HandleMovement();
            HandlePlayerDetection();
        }
        
        /// <summary>
        /// Used at the end of the bat animation flipping off the ceiling.
        /// </summary>
        private void AllowMovement() => CanMove = true;

        private void HandleMovement()
        {
            if (CanMove == false)
                return;
            
            HandleFlip(_destination.x);
            transform.position = Vector2.MoveTowards(transform.position, _destination, moveSpeed * Time.deltaTime);
            
            if (_chaseTimer > 0 && _target != null)
                _destination = _target.transform.position;
            else
                moveSpeed = attackSpeed;
            
            if (Vector2.Distance(transform.position, _destination) < .1f)
            {
                if (_destination == _originalPosition)
                {
                    IdleTimer = idleDuration;
                    _canDetectPlayer = false;
                    CanMove = false;
                    Anim.SetBool(IsMoving, false);
                    _target = null;
                    moveSpeed = _defaultSpeed;
                }
                else
                {
                    _destination = _originalPosition;
                }
            }
        }

        private void HandlePlayerDetection()
        {
            if (_target == null && _canDetectPlayer)
            {
                _target = Physics2D.OverlapCircle(transform.position, agroRadius, whatIsPlayer);

                if (_target != null)
                {
                    _chaseTimer = chaseDuration;
                    _destination = _target.transform.position;
                    _canDetectPlayer = false;
                    Anim.SetBool(IsMoving, true);
                }
            }
        }

        public override void Die()
        {
            base.Die();
            
            CanMove = false;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            Gizmos.DrawWireSphere(transform.position, agroRadius);
        }
    }
}
