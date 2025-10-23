using UnityEngine;

namespace Game._Scripts.Camera
{
    public class LevelCameraTrigger : MonoBehaviour
    {
        private LevelCamera _levelCamera;

        private void Awake()
        {
            _levelCamera = GetComponentInParent<LevelCamera>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();

            if (player)
            {
                _levelCamera.EnableCamera(true);
                _levelCamera.SetNewTarget(player.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();

            if (player)
            {
                _levelCamera.EnableCamera(false);
            }
        }
    }
}
