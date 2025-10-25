using UnityEngine;

namespace Game._Scripts.Utils
{
    public class TurnOffMeEvent : MonoBehaviour
    {
        /// <summary>
        /// Used as Event Trigger on fireball animation
        /// </summary>
        public void TurnOffMe() => gameObject.SetActive(false);
    }
}