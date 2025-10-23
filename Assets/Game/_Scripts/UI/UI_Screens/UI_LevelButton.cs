using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game._Scripts.UI.UI_Screens
{
    public class UI_LevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI bestTimeText;
        [SerializeField] private TextMeshProUGUI fruitsText;
    
        public string sceneName;

        private int _levelIndex;

        public void SetupButton(int newLevelIndex)
        {
            _levelIndex = newLevelIndex;

            levelNumberText.text = $"Level {newLevelIndex}";
            sceneName = $"Level_{newLevelIndex}";

            bestTimeText.text = GetBestTime();
            fruitsText.text = GetFruits();

        }
        public void LoadLevel()
        {
            SceneManager.LoadScene(sceneName);
        }

        private string GetBestTime()
        {
            return PlayerPrefs.GetFloat($"Level{_levelIndex}BestTime", 99).ToString("00");
        }

        private string GetFruits()
        {
            int totalFruitsOnLevel = PlayerPrefs.GetInt("Level" + _levelIndex + "TotalFruits", 0);
            string totalFruitsText = totalFruitsOnLevel == 0 ? "?" : totalFruitsOnLevel.ToString();

            int collectedFruits = PlayerPrefs.GetInt($"Level{_levelIndex}FruitsCollected");

            return $"{collectedFruits} / {totalFruitsText}";
        }
    }
}
