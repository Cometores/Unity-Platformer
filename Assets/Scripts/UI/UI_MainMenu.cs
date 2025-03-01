using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public string sceneName;

    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if (HasLevelProgression())
            continueButton.SetActive(true);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    
    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (var uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        
        uiToEnable.SetActive(true);
    }

    private bool HasLevelProgression() => PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;

    public void ContinueGame()
    {
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        SceneManager.LoadScene($"Level_{levelToLoad}");
    }
}
