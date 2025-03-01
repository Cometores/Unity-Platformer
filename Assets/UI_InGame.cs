using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI fruitsText;

    private void Awake()
    {
        instance = this;
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
