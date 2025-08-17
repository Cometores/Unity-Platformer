using Managers;
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
        
        private PlayerInput _playerInput;
        private bool _isPaused;
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.UI.Pause.performed += ctx => PauseButton();
        }

        private void OnDisable()
        {
            _playerInput.UI.Pause.performed -= ctx => PauseButton();
            _playerInput.Disable();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                PauseButton();
        }

        public void PauseButton()
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        private void PauseGame()
        {
            PlayerManager.Instance.player.PlayerInput.Disable();
            _isPaused = true;
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }

        private void ResumeGame()
        {
            PlayerManager.Instance.player.PlayerInput.Enable();
            _isPaused = false;
            Time.timeScale = 1f;
            pauseUI.SetActive(false);
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
