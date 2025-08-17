using UI;
using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;
        [SerializeField] private float levelTimer;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        private void Update()
        {
            levelTimer += Time.deltaTime;
            UIInGame.Instance.UpdateTimer(levelTimer);
        }
        
        public void SaveBestTimeForLevel(int levelIndex)
        {
            PlayerPrefs.SetFloat($"Level{levelIndex}BestTime", levelTimer);
        }
    }
}
