using UnityEngine;

namespace Game._Scripts.Managers
{
    public enum DifficultyType
    {
        Easy,
        Normal,
        Hard
    }

    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance;
        public DifficultyType difficulty;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void SetDifficulty(DifficultyType newDifficulty) => difficulty = newDifficulty;
    }
}