using Game._Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game._Scripts.UI.UI_Screens
{
    public class UI_MainMenu : MonoBehaviour
    {
        public string sceneName;

        [SerializeField] private GameObject[] uiElements;
        [SerializeField] private GameObject continueButton;

        private void Start()
        { 
            Time.timeScale = 1;
            
            if (HasLevelProgression())
                continueButton.SetActive(true);
            else
                continueButton.SetActive(false);
        }

        public void NewGame()
        {
            SceneManager.LoadScene(sceneName);
            AudioManager.Instance.PlaySfx(4);
        }

    
        public void SwitchUI(GameObject uiToEnable)
        {
            foreach (var uiElement in uiElements)
            {
                uiElement.SetActive(false);
            }
        
            uiToEnable.SetActive(true);
            
            AudioManager.Instance.PlaySfx(4);
        }

        private bool HasLevelProgression() => PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;

        public void ContinueGame()
        {
            int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
            SceneManager.LoadScene($"Level_{levelToLoad}");
        }
    }
}
