using System;
using System.Collections;
using Game._Scripts.Environment.Checkpoint;
using Game._Scripts.Player;
using Game._Scripts.Utils;
using Unity.Mathematics;
using UnityEngine;

namespace Game._Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static event Action OnPlayerRespawn;
        public static PlayerManager Instance;
    
        [Header("Player")]
        public GameObject playerPrefab;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float respawnDelay = 1.5f;
        public PlayerBase player;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            
            if (!respawnPoint)
                respawnPoint = FindFirstObjectByType<StartPoint>().transform;

            if (!player)
                player = FindFirstObjectByType<PlayerBase>();
        }
        
        #region Player respawn logic
        
        public void UpdateRespawnPoint(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
        
        public void TryRespawnPlayer()
        {
            DifficultyManager difficultyManager = DifficultyManager.Instance;
            if (difficultyManager && difficultyManager.difficulty == DifficultyType.Hard)
                return;
        
            StartCoroutine(RespawnCoroutine());
        }
        
        private IEnumerator RespawnCoroutine()
        {
            yield return Helpers.GetWait(respawnDelay);
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, quaternion.identity);
            player = newPlayer.GetComponent<PlayerBase>();
            
            OnPlayerRespawn?.Invoke();
        }

        #endregion
        
        public void ChangePlayer(GameObject newPlayerPrefab)
        {
            playerPrefab = newPlayerPrefab;
            Destroy(player.gameObject);
            SpawnPlayer();
        }
    }
}
