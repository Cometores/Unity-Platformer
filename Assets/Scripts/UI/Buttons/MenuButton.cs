using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace UI.Buttons
{
    [RequireComponent(typeof(Animator), typeof(Button))]
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action Selected;
        public event Action Unselected;
        
        [ShowNonSerializedField] private bool _isSelected;
        
        private Button _button;
        private Animator _animator;
        private static readonly int SelectedHash = Animator.StringToHash("selected");

        protected void Awake()
        {
            _button   = GetComponent<Button>();
            _animator = GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button.IsActive())
                EventSystem.current?.SetSelectedGameObject(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventSystem.current?.SetSelectedGameObject(null);
        }

        protected void OnDisable()
        {
            Unselected?.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _isSelected = true;
            _animator.SetTrigger(SelectedHash);
            Selected?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _isSelected = false;
            Unselected?.Invoke();
        }
    }
}