using System.Collections;
using Environment.Checkpoint;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
    
        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float respawnDelay;
        public Player.Player player;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (respawnPoint == null)
                respawnPoint = FindFirstObjectByType<StartPoint>().transform;

            if (player == null)
                player = FindFirstObjectByType<Player.Player>();
        }
        
        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(respawnDelay);
        
            GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, quaternion.identity);
            player = newPlayer.GetComponent<Player.Player>();
        }
        
        public void RespawnPlayer()
        {
            DifficultyManager difficultyManager = DifficultyManager.Instance;
            if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
                return;
        
            StartCoroutine(RespawnCoroutine());
        }
        
        public void UpdateRespawnPoint(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
    }
}
