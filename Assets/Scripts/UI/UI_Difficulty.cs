using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UI_Difficulty : MonoBehaviour
    {
        [SerializeField] private GameObject firstSelected;
        private DifficultyManager _difficultyManager;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        private void Start()
        {
            _difficultyManager = DifficultyManager.Instance;
        }

        public void SetEasyMode() => _difficultyManager.SetDifficulty(DifficultyType.Easy);
        public void SetNormalMode() => _difficultyManager.SetDifficulty(DifficultyType.Normal);
        public void SetHardMode() => _difficultyManager.SetDifficulty(DifficultyType.Hard);
    }
}
