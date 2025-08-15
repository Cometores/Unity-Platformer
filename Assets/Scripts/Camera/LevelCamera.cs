using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class LevelCamera : MonoBehaviour
    {
        private CinemachineCamera _cinemachine;

        private void Awake()
        {
            _cinemachine = GetComponentInChildren<CinemachineCamera>(true);
        }

        public void EnableCamera(bool enable)
        {
            _cinemachine.gameObject.SetActive(enable);
        }
        
        public void SetNewTarget(Transform newTarget)
        {
            _cinemachine.Follow = newTarget;
        }
    }
}
