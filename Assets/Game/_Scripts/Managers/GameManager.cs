using System.Collections;
using Game._Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game._Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private int currentLevelIndex;

        [Header("Managers")]
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private SkinManager skinManager;
        [SerializeField] private DifficultyManager difficultyManager;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private FruitManager fruitManager;
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            CreateManagersIfNeeded();
            FruitManager.Instance.CollectFruitsInfo(currentLevelIndex);
        }
        
        private void CreateManagersIfNeeded()
        {
            if (!AudioManager.Instance)
                Instantiate(audioManager);
            
            if (!PlayerManager.Instance)
                Instantiate(playerManager);
            
            if (!SkinManager.Instance)
                Instantiate(skinManager);
            
            if (!DifficultyManager.Instance)
                Instantiate(difficultyManager);
            
            if (!TimeManager.Instance)
                Instantiate(timeManager);
            
            if (!FruitManager.Instance)
                Instantiate(fruitManager);
        }

        public static void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
        public void LoadNextLevel()
        {
            int nextLevelIndex = currentLevelIndex + 1;
        
            if (IsFinalLevel())
                return;
        
            SaveSystem.UnlockLevel(nextLevelIndex);
            SaveSystem.SetContinueLevelIndex(nextLevelIndex);
        
            TimeManager.Instance.SaveBestTimeForLevel(currentLevelIndex);
            FruitManager.Instance.SaveFruitsInfo(currentLevelIndex);
        
            SceneManager.LoadScene($"Level_{nextLevelIndex}");
        }

        private bool IsFinalLevel()
        {
            int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 1;
            return currentLevelIndex == lastLevelIndex;
        }
    }
}