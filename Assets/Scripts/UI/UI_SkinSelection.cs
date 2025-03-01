using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private Animator skinDisplay;
    private int _currentIndex;


    public void ConfirmSkin()
    {
        SkinManager.instance.ChosenSkinId = _currentIndex;
    }
    
    public void NextSkin()
    {
        _currentIndex = (_currentIndex + 1) % skinDisplay.layerCount;
        UpdateSkinDisplay();
    }

    public void PreviousSkin()
    {
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = skinDisplay.layerCount - 1;
        }
        
        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
             skinDisplay.SetLayerWeight(i, 0 );
        }

        skinDisplay.SetLayerWeight(_currentIndex, 1);
    }
}
