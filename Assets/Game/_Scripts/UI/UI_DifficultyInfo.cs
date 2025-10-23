using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game._Scripts.UI
{
    public class UI_DifficultyInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI difficultyInfo;
    
        [TextArea]
        [SerializeField] private string description;
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current?.SetSelectedGameObject(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventSystem.current?.SetSelectedGameObject(null);
        }

        public void OnSelect(BaseEventData eventData)
        {
            difficultyInfo.text = description;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            difficultyInfo.text = "";
        }
    }
}
