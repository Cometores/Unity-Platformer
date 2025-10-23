using UnityEngine;

namespace Game._Scripts.UI.Buttons
{
    public class FireballVFX : MonoBehaviour
    {
        /// <summary>
        /// Used as Event Trigger on fireball animation
        /// </summary>
        public void TurnOffVFX() => gameObject.SetActive(false);
    }
}