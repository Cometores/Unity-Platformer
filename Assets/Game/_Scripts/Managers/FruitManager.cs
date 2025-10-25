using Game._Scripts.UI.UI_Screens;
using NaughtyAttributes;
using UnityEngine;

namespace Game._Scripts.Managers
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
        
            SaveSystem.SaveTotalLevelFruits(currentLevelIndex, totalFruits);
        }
        
        public void SaveFruitsInfo(int currentLevelIndex) => 
            SaveSystem.SaveFruitsCollected(currentLevelIndex, fruitsCollected);

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
