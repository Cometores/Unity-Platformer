using Game._Scripts.Managers;
using UnityEngine;

namespace Game._Scripts
{
    public static class SaveSystem
    {
        public static int GetSkin()
        {
            return PlayerPrefs.GetInt("CurrentSkin", 0);
        }
        
        public static void SetSkin(int skinId)
        {
            PlayerPrefs.SetInt("CurrentSkin", skinId);
        }

        public static DifficultyType GetDifficulty()
        {
            return (DifficultyType)PlayerPrefs.GetInt("Difficulty", 0);
        }

        public static void SetDifficulty(int difficulty)
        {
            PlayerPrefs.SetInt("Difficulty", difficulty);
        }
    }
}