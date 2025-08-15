using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
    
        [Header("Camera Shake")]
        [SerializeField] private Vector2 shakeVelocity;

        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            Instance = this;
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void CameraShake(float shakeDirection)
        {
            _impulseSource.DefaultVelocity = new Vector2(shakeVelocity.x * shakeDirection, shakeVelocity.y);
            _impulseSource.GenerateImpulse();
        }
    }
}