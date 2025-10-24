using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game._Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private int currentLevelIndex;

        [Header("Traps")]
        public GameObject arrowPrefab;

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

        public void CreateObject(GameObject prefab, Transform target, float delay = 0)
        {
            StartCoroutine(CreateObjectCoroutine(prefab, target, delay));
        }
    
        private IEnumerator CreateObjectCoroutine(GameObject prefab, Transform target, float delay)
        {
            Vector3 newPosition = target.position;
        
            yield return Helpers.GetWait(delay);

            GameObject newObject = Instantiate(prefab, newPosition, Quaternion.identity);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
        public void LoadNextLevel()
        {
            int nextLevelIndex = currentLevelIndex + 1;
        
            if (IsFinalLevel())
                return;
        
            PlayerPrefs.SetInt($"Level{nextLevelIndex}Unlocked", 1);
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
        
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