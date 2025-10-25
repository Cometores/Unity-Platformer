using System;
using System.Collections;
using System.Collections.Generic;
using Game._Scripts.Utils;
using UnityEngine;

namespace Game._Scripts.Enemy
{
    public class BeeBulletEnemy : MonoBehaviour
    {
        private Transform _target;
        private List<Vector3> _wayPoints = new();
        private int _wayIndex;

        [SerializeField] private GameObject endVfx;
        [SerializeField] private float wayPointUpdateCooldown;
        private float _speed;

        public void SetupBullet(Transform target, float speed, float lifeTime)
        {
            _target = target;
            _speed = speed;
            
            transform.up = target.position - transform.position;
            
            StartCoroutine(AddWayPointRoutine());
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            if (_wayPoints.Count <= 0) return;
            
            transform.position = Vector2.MoveTowards(transform.position, _wayPoints[_wayIndex], _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _wayPoints[_wayIndex]) < .1f)
            {
                _wayIndex++;
                transform.up = transform.position - _wayPoints[_wayIndex];
            }
        }

        private IEnumerator AddWayPointRoutine()
        {
            while (true)
            {
                AddWayPoint();
                yield return Helpers.GetWait(wayPointUpdateCooldown);
            }
        }

        private void AddWayPoint()
        {
            if (_target == null) return;

            foreach (Vector3 wayPoint in _wayPoints)
            {
                if (wayPoint == _target.position)
                    return;
            }
            
            _wayPoints.Add(_target.position);
        }
        
        private void OnDestroy()
        {
            if(!this.gameObject.scene.isLoaded) return;
            
            GameObject newFx = Instantiate(endVfx, transform.position, Quaternion.identity);
            newFx.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }
}
