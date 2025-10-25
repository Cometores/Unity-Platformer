using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Game._Scripts.UI.UI_Screens
{
    public class UI_LevelSelection : MonoBehaviour
    {
        [SerializeField] private UI_LevelButton buttonPrefab;
        [SerializeField] private Transform buttonsParent;

        [SerializeField] private bool[] levelsUnlocked;
    
        private int _levelsAmount;
        
        private void Awake()
        {
            _levelsAmount = SceneManager.sceneCountInBuildSettings - 1; // -1 for MainMenu

            LoadLevelsInfo();
            CreateLevelButtons();
        }
        
        private void OnEnable()
        {
            GameObject firstLevelButton = buttonsParent.GetChild(0).gameObject;
            if(firstLevelButton)
                EventSystem.current.SetSelectedGameObject(firstLevelButton);
        }

        private void LoadLevelsInfo()
        {
            levelsUnlocked = new bool[_levelsAmount + 1];

            for (int i = 0; i <= _levelsAmount; i++)
            {
                levelsUnlocked[i] = SaveSystem.IsLevelUnlocked(i);
            }
        
            levelsUnlocked[1] = true;
        }
    
        private void CreateLevelButtons()
        {
            for (int i = 1; i <= _levelsAmount; i++)
            {
                if (!IsLevelUnlocked(i)) return;
            
                UI_LevelButton newButton = Instantiate(buttonPrefab, buttonsParent);
                newButton.SetupButton(i);
            }
        }

        private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];
    }
}
