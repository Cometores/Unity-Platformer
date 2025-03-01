using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText;
    public string sceneName;

    private int _levelIndex;

    public void SetupButton(int newLevelIndex)
    {
        _levelIndex = newLevelIndex;

        levelNumberText.text = $"Level {newLevelIndex}";
        sceneName = $"Level_{newLevelIndex}";
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
