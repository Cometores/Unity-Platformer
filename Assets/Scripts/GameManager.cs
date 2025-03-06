using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private UI_InGame _inGameUI;
    
    [Header("Level Management")]
    [SerializeField] private float levelTimer;
    [SerializeField] private int currentLevelIndex;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    public Player player;
    
    [Header("Fruits Management")]
    public int fruitsCollected;
    public bool fruitsAreRandom;
    public int totalFruits;

    [Header("Traps")]
    public GameObject arrowPrefab;
    
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _inGameUI = UI_InGame.instance;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        
        CollectFruitsInfo();
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;
        _inGameUI.UpdateTimer(levelTimer);
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void UpdateRespawnPoint(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    public void RespawnPlayer()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;
        if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
            return;
        
        StartCoroutine(RespawnCoroutine());
    }

    public void AddFruit()
    {
        fruitsCollected++;
        _inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public void RemoveFruit()
    {
        fruitsCollected--;
        _inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public int FruitsCollected() => fruitsCollected;

    public bool FruitsHaveRandomLook() => fruitsAreRandom;

    public void CreateObject(GameObject prefab, Transform target, float delay = 0)
    {
        StartCoroutine(CreateObjectCoroutine(prefab, target, delay));
    }
    
    private IEnumerator CreateObjectCoroutine(GameObject prefab, Transform target, float delay)
    {
        Vector3 newPosition = target.position;
        
        yield return new WaitForSeconds(delay);

        GameObject newObject = Instantiate(prefab, newPosition, Quaternion.identity);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene($"Level_{currentLevelIndex}");
    }
    
    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        
        if (IsFinalLevel())
            return;
        
        PlayerPrefs.SetInt($"Level{nextLevelIndex}Unlocked", 1);
        PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
        
        SaveBestTime();
        SaveFruitsInfo();
        
        SceneManager.LoadScene($"Level_{nextLevelIndex}");
    }

    private bool IsFinalLevel()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 1;
        return currentLevelIndex == lastLevelIndex;
    }

    private void SaveBestTime()
    {
        PlayerPrefs.SetFloat($"Level{currentLevelIndex}BestTime", levelTimer);
    }
    
    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;
        
        _inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
        
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
    }

    private void SaveFruitsInfo()
    {
        int maxFruits = PlayerPrefs.GetInt($"Level{currentLevelIndex}FruitsCollected");
        
        if (fruitsCollected > maxFruits)
            PlayerPrefs.SetInt($"Level{currentLevelIndex}FruitsCollected", fruitsCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");
        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
    }
}