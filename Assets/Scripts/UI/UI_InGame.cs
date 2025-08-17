using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIInGame : MonoBehaviour
    {
        public static UIInGame Instance;
    
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI fruitsText;

        [SerializeField] private GameObject pauseUI;
        private bool _isPaused;
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                PauseButton();
        }

        public void PauseButton()
        {
            if (_isPaused)
            {
                _isPaused = false;
                Time.timeScale = 1f;
                pauseUI.SetActive(false);
            }
            else
            {
                _isPaused = true;
                Time.timeScale = 0;
                pauseUI.SetActive(true);
            }
        }

        public void GoToMainMenuButton()
        {
            SceneManager.LoadScene(0);
        }

        public void UpdateTimer(float time)
        {
            timerText.text = time.ToString("00") + " s";
        }
        public void UpdateFruitUI(int collectedFruits, int totalFruits)
        {
            fruitsText.text = $"{collectedFruits} / {totalFruits}";
        }
    }
}
