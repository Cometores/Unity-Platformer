using NaughtyAttributes;
using UI;
using UnityEngine;

namespace Managers
{
    public class FruitManager : MonoBehaviour
    {
        public static FruitManager Instance;
        
        [Header("Fruits Management")]
        public bool fruitsAreRandom;
        
        [ReadOnly, ShowIf(nameof(IsPlaying))]
        public int totalFruits;
        [ReadOnly, ShowIf(nameof(IsPlaying))]
        public int fruitsCollected;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private bool IsPlaying() => Application.isPlaying;
        
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

        #region DebugTools
        
#if UNITY_EDITOR

        [Button]
        private void ParentAllFruits()
        {
            Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
            
            foreach (Fruit fruit in allFruits)
                fruit.transform.SetParent(transform);
        }
#endif
        
        #endregion
    }
}
