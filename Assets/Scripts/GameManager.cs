using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Level Management")]
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
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        CollectFruitsInfo();
    }

    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void UpdateRespawnPoint(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    public void RespawnPlayer() => StartCoroutine(RespawnCoroutine());

    public void AddFruit() => fruitsCollected++;
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

    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        
        if (IsFinalLevel())
            return;
        
        PlayerPrefs.SetInt($"Level{nextLevelIndex}Unlocked", 1);
        PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
        
        SceneManager.LoadScene($"Level_{nextLevelIndex}");
    }

    private bool IsFinalLevel()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 1;
        return currentLevelIndex == lastLevelIndex;
    }
}