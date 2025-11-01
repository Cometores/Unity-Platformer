using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class BeeEnemy : Enemy
    {
        [Header("Bee details")]
        [SerializeField] private BeeBulletEnemy bulletPrefab;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float bulletSpeed = 7;
        [SerializeField] private float bulletLifeTime = 2.5f;

        [SerializeField] private float wayPointsOffset = .25f;
        private List<Vector3> _wayPoints = new();
        private int _wayIndex;

        private Transform _target;

        private float _lastTimeAttacked;
        private static readonly int Attack1 = Animator.StringToHash("attack");

        protected override void Start()
        {
            base.Start();

            InitializeWayPoints();
        }

        protected override void Update()
        {
            base.Update();

            HandleMovement();
            DetectPlayerBelow();

            bool canAttack = Time.time > _lastTimeAttacked + attackCooldown && _target != null;

            if (canAttack)
                Attack();
        }

        private void DetectPlayerBelow()
        {
            if (!_target)
            {
                // Checking if the player is below
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, float.MaxValue, whatIsPlayer);

                if (hit.collider != null)
                    _target = hit.transform;
            }
        }

        private void HandleMovement()
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _wayPoints[_wayIndex], moveSpeed * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, _wayPoints[_wayIndex]) < .1f)
            {
                _wayIndex++;
                _wayIndex %= _wayPoints.Count;
            }
        }

        private void Attack()
        {
            _lastTimeAttacked = Time.time;
            Anim.SetTrigger(Attack1);
        }

        private void CreateBullet()
        {
            BeeBulletEnemy newBullet = Instantiate(bulletPrefab, gunPoint.position, quaternion.identity);
            newBullet.SetupBullet(_target, bulletSpeed, bulletLifeTime);

            _target = null;
            Destroy(newBullet.gameObject, 10);
        }

        private void InitializeWayPoints()
        {
            _wayPoints = new List<Vector3>
            {
                transform.position + new Vector3(wayPointsOffset, wayPointsOffset),
                transform.position + new Vector3(wayPointsOffset, -wayPointsOffset),
                transform.position + new Vector3(-wayPointsOffset, -wayPointsOffset),
                transform.position + new Vector3(-wayPointsOffset, wayPointsOffset)
            };
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            
            // InitializeWayPoints();
            
            foreach (var wayPoint in _wayPoints)
                Gizmos.DrawWireSphere(wayPoint, 1f);
        }
    }
}