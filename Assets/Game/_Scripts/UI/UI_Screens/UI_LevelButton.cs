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
    
        private string _sceneName;
        private int _levelIndex;

        public void SetupButton(int levelIndex)
        {
            _levelIndex = levelIndex;
            _sceneName = $"Level_{levelIndex}";
            
            levelNumberText.text = $"Level {levelIndex}";
            bestTimeText.text = SaveSystem.GetBestTime(_levelIndex);
            fruitsText.text = SaveSystem.GetFruitsCollectedInfo(_levelIndex);
        }
        public void LoadLevel() => SceneManager.LoadScene(_sceneName);
    }
}
