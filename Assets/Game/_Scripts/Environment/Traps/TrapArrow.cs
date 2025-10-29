using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game._Scripts.Environment.Traps
{
    public class TrapArrow : TrapTrampoline
    {
        [Header("Additional info")]
        [SerializeField] private float cooldown;
        [SerializeField] private bool rotationRight;
        [SerializeField] private float rotationSpeed = 120;
        private int _direction = 1;

        [FormerlySerializedAs("growSpeed")]
        [Space]
        [SerializeField] private float scaleUpSpeed = 10;
        [SerializeField] private Vector3 targetScale;

        private void Start()
        {
            transform.localScale = new Vector3(.3f, .3f, .3f);
        }

        private void Update()
        {
            HandleScaleUp();
            HandleRotation();
        }

        private void HandleScaleUp()
        {
            if (transform.localScale.x < targetScale.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            _direction = rotationRight ? -1 : 1;
            transform.Rotate(0, 0, (rotationSpeed * _direction) * Time.deltaTime);
        }

        private void DestroyMe()
        {
            GameObject arrowPrefab = ObjectSpawner.Instance.arrowPrefab;
            ObjectSpawner.Instance.CreateObject(arrowPrefab, transform, cooldown);
            Destroy(gameObject);
        }
    }
}