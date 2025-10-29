using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Game._Scripts.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class SelectableGuard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => EnsureSelection();

        public void OnPointerExit(PointerEventData eventData) => UnselectSafe();

        private void EnsureSelection()
        {
            var es = EventSystem.current;
            
            if (es == null) return;
            if (es.currentSelectedGameObject == gameObject) return;
            if (es.alreadySelecting) return;
            
            es.SetSelectedGameObject(gameObject);
        }

        private void UnselectSafe()
        {
            var es = EventSystem.current;
            if (es == null) return;
            if (es.currentSelectedGameObject != gameObject) return;

            if (es.alreadySelecting)
            {
                StartCoroutine(ClearNextFrame());
                return;
            }
            
            es.SetSelectedGameObject(null);
        }

        private IEnumerator ClearNextFrame()
        {
            yield return new WaitForEndOfFrame();
            
            var es = EventSystem.current;
            if (es != null && es.currentSelectedGameObject == gameObject && !es.alreadySelecting)
                es.SetSelectedGameObject(null);
        }
    }
}