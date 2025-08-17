using UI;
using UnityEngine;

namespace Managers
{
    public class FruitManager : MonoBehaviour
    {
        public static FruitManager Instance;
        
        [Header("Fruits Management")]
        public int fruitsCollected;
        public bool fruitsAreRandom;
        public int totalFruits;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        public void AddFruit()
        {
            fruitsCollected++;
            UIInGame.Instance.UpdateFruitUI(fruitsCollected, totalFruits);
        }

        public void RemoveFruit()
        {
            fruitsCollected--;
            UIInGame.Instance.UpdateFruitUI(fruitsCollected, totalFruits);
        }

        public int FruitsCollected() => fruitsCollected;

        public bool FruitsHaveRandomLook() => fruitsAreRandom;
        
        
        public void CollectFruitsInfo(int currentLevelIndex)
        {
            Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
            totalFruits = allFruits.Length;
        
            UIInGame.Instance.UpdateFruitUI(fruitsCollected, totalFruits);
        
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
        }

        public void SaveFruitsInfo(int currentLevelIndex)
        {
            int maxFruits = PlayerPrefs.GetInt($"Level{currentLevelIndex}FruitsCollected");
        
            if (fruitsCollected > maxFruits)
                PlayerPrefs.SetInt($"Level{currentLevelIndex}FruitsCollected", fruitsCollected);

            int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");
            PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
        }
    }
}
