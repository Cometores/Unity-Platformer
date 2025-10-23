using System;
using System.Collections;
using Game._Scripts.Environment.Checkpoint;
using Unity.Mathematics;
using UnityEngine;

namespace Game._Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static event Action OnPlayerRespawn;
        public static PlayerManager Instance;
    
        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float respawnDelay = 1.5f;
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
        
        #region Player respawn logic
        
        public void RespawnPlayer()
        {
            DifficultyManager difficultyManager = DifficultyManager.Instance;
            if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
                return;
        
            StartCoroutine(RespawnCoroutine());
        }
        
        private IEnumerator RespawnCoroutine()
        {
            yield return Helpers.GetWait(respawnDelay);
        
            GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, quaternion.identity);
            player = newPlayer.GetComponent<Player.Player>();
            
            OnPlayerRespawn?.Invoke();
        }
        
        public void UpdateRespawnPoint(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
        
        #endregion
    }
}
