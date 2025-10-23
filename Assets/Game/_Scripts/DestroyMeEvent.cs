using UnityEngine;

namespace Game._Scripts
{
    public class DestroyMeEvent : MonoBehaviour
    {
        public void DestroyMe() => Destroy(gameObject);
    }
}
