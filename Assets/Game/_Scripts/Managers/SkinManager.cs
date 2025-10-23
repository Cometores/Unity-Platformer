using UnityEngine;

namespace Game._Scripts.Managers
{
    public class SkinManager : MonoBehaviour
    {
        public static SkinManager Instance;
        public int ChosenSkinId { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}
