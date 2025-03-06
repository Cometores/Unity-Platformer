using System;
using UnityEngine;

public class UI_Difficulty : MonoBehaviour
{
    private DifficultyManager _difficultyManager;

    private void Start()
    {
        _difficultyManager = DifficultyManager.instance;
    }

    public void SetEasyMode() => _difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetNormalMode() => _difficultyManager.SetDifficulty(DifficultyType.Normal);
    public void SetHardMode() => _difficultyManager.SetDifficulty(DifficultyType.Hard);
}
