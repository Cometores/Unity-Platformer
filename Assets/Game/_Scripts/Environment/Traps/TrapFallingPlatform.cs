using System.Collections;
using Game._Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game._Scripts.Environment.Traps
{
    public class TrapFallingPlatform : MonoBehaviour
    {
        private Animator _anim;
        private Rigidbody2D _rb;
        private BoxCollider2D[] _colliders;
    
        private bool _canMove = false;
        [SerializeField] private float speed = 0.75f;
        [SerializeField] private float travelDistance;
        private Vector3[] _wayPoints;
        private int _wayPointIndex;
    
        [Header("Platform fall detais")]
        [SerializeField] private float fallDelay = .5f;
        [SerializeField] private float impactSpeed = 3;
        [SerializeField] private float impactDuration = .1f;
    
        private float _impactTimer;
        private bool _wasImpact = false;
    
        private static readonly int Deactivate = Animator.StringToHash("deactivate");

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _colliders = GetComponents<BoxCollider2D>();
        }

        private IEnumerator Start()
        {
            SetupWayPoints();
            float randomDelay = Random.Range(0, .6f);
        
            yield return Helpers.GetWait(randomDelay);
        
            _canMove = true;
        }

        private void SetupWayPoints()
        {
            _wayPoints = new Vector3[3];

            float yOffset = travelDistance / 2;

            _wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
            _wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);
        }

        private void Update()
        {
            HandleMovement();
            HandleImpact();
        }

        private void HandleMovement()
        {
            if (_canMove == false)
                return;

            transform.position = Vector2.MoveTowards(transform.position, _wayPoints[_wayPointIndex], speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _wayPoints[_wayPointIndex]) < .1f)
                _wayPointIndex = (_wayPointIndex + 1) % 2;
        }

        private void HandleImpact()
        {
            if (_wasImpact) return;
        
            if (_impactTimer < 0)
                return;
        
            _impactTimer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                transform.position, 
                transform.position + (Vector3.down * 10), 
                impactSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();

            if (player != null)
            {
                Invoke(nameof(SwitchOffPlatform), fallDelay);
                _impactTimer = impactDuration;
                _wasImpact = true;
            }
        }

        private void SwitchOffPlatform()
        {
            _anim.SetTrigger(Deactivate);

            _canMove = false;

            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 3.5f;
            _rb.linearDamping = .5f;
            
            foreach (BoxCollider2D boxCollider2D in _colliders)
            {
                boxCollider2D.enabled = false;
            }
        }
    }
}
