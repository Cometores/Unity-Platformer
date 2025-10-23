using UnityEngine;

namespace Game._Scripts.Managers
{
    public enum DifficultyType
    {
        Easy,
        Normal,
        Hard
    }

    /// <summary>
    /// Manages gameplay difficulty settings and provides access to the current difficulty level.
    /// It allows changing the difficulty level and persists the chosen setting using PlayerPrefs.
    /// </summary>
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance;
        public DifficultyType difficulty;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            
            LoadDifficulty();
        }

        private void LoadDifficulty()
        {
            difficulty = SaveSystem.GetDifficulty();
        }
        
        public void SetDifficulty(DifficultyType newDifficulty)
        {
            difficulty = newDifficulty;
            SaveSystem.SetDifficulty((int)difficulty);
        }
    }
}