using Game._Scripts.Managers;
using UnityEngine;

namespace Game._Scripts
{
    public static class SaveSystem
    {
        #region Skins

        public static int GetCurrentSkinIndex()
        {
            return PlayerPrefs.GetInt(Constants.CURRENT_SKIN_KEY, 0);
        }
        
        public static void SaveCurrentSkinIndex(int skinId)
        {
            PlayerPrefs.SetInt(Constants.CURRENT_SKIN_KEY, skinId);
            PlayerPrefs.Save();
        }
        
        public static bool IsSkinUnlocked(string skinName)
        {
            return PlayerPrefs.GetInt($"{skinName}{Constants.UNLOCKED_KEY}", 0) == 1;
        }

        public static void UnlockSkin(string skinName)
        {
            PlayerPrefs.SetInt($"{skinName}{Constants.UNLOCKED_KEY}", 1);
            PlayerPrefs.Save();
        }

        #endregion

        #region Difficulty

        public static DifficultyType GetDifficulty()
        {
            return (DifficultyType)PlayerPrefs.GetInt(Constants.DIFFICULTY_KEY, 0);
        }

        public static void SetDifficulty(int difficulty)
        {
            PlayerPrefs.SetInt(Constants.DIFFICULTY_KEY, difficulty);
            PlayerPrefs.Save();
        }

        #endregion
        
        #region Volume
        
        public static float GetVolume(string channelName)
        {
            return PlayerPrefs.GetFloat(channelName, Constants.DEFAULT_VOLUME);
        }
        
        public static void SetVolume(float volume, string channelName)
        {
            PlayerPrefs.SetFloat(channelName, volume);
            PlayerPrefs.Save();
        }
        
        #endregion
        
        #region Levels
        
        public static int GetContinueLevelIndexOrZero()
        {
            return PlayerPrefs.GetInt(Constants.CONTINUE_LEVEL_NUMBER, 0);
        }
        
        public static void SetContinueLevelIndex(int nextLevelIndex)
        {
            PlayerPrefs.SetInt(Constants.CONTINUE_LEVEL_NUMBER, nextLevelIndex);
        }
        
        public static bool IsLevelUnlocked(int levelIndex)
        {
            return PlayerPrefs.GetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.UNLOCKED_KEY}", 0) == 1;
        }
        
        public static void UnlockLevel(int levelIndex)
        {
            PlayerPrefs.SetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.UNLOCKED_KEY}", 1);
            PlayerPrefs.Save();
        }

        public static string GetBestTime(int levelIndex)
        {
            return PlayerPrefs.GetFloat($"{Constants.LEVEL_KEY}{levelIndex}{Constants.BEST_TIME}", 99).ToString("00");
        }
        public static void SaveBestTime(int levelIndex, float levelTimer)
        {
            PlayerPrefs.SetFloat($"{Constants.LEVEL_KEY}{levelIndex}{Constants.BEST_TIME}", levelTimer);
        }

        public static void SaveTotalLevelFruits(int levelIndex, int totalFruits)
        {
            PlayerPrefs.SetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.TOTAL_FRUITS}", totalFruits);
        }
        
        public static void SaveFruitsCollected(int levelIndex, int fruitsCollected)
        {
            int maxFruits = PlayerPrefs.GetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.FRUITS_COLLECTED}");
        
            if (fruitsCollected > maxFruits)
                PlayerPrefs.SetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.FRUITS_COLLECTED}", fruitsCollected);

            int totalFruitsInBank = PlayerPrefs.GetInt(Constants.FRUITS_IN_BANK);
            PlayerPrefs.SetInt(Constants.FRUITS_IN_BANK, totalFruitsInBank + fruitsCollected);
        }
        
        public static string GetFruitsCollectedInfo(int levelIndex)
        {
            int totalFruitsOnLevel = PlayerPrefs.GetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.TOTAL_FRUITS}", 0);
            string totalFruitsText = totalFruitsOnLevel == 0 ? "?" : totalFruitsOnLevel.ToString();
            int collectedFruits = PlayerPrefs.GetInt($"{Constants.LEVEL_KEY}{levelIndex}{Constants.FRUITS_COLLECTED}");

            return $"{collectedFruits} / {totalFruitsText}";
        }
        
        #endregion

        #region Currency

        public static void SaveFruitsBank(int fruitsInBank)
        {
            PlayerPrefs.SetInt(Constants.FRUITS_IN_BANK, fruitsInBank);
        }

        public static int GetFruitsInBank()
        {
            return PlayerPrefs.GetInt(Constants.FRUITS_IN_BANK);
        }

        #endregion
    }
}