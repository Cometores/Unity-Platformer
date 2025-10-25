using UnityEngine;

namespace Game._Scripts.Utils
{
    public class DestroyMeEvent : MonoBehaviour
    {
        public void DestroyMe() => Destroy(gameObject);
    }
}
