using UnityEngine;

namespace Game._Scripts.Managers
{
    public class SkinManager : MonoBehaviour
    {
        public static SkinManager Instance;
        public int ChosenSkinId { get; set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            GetSkin();
        }
        
        private void GetSkin()
        {
            ChosenSkinId = SaveSystem.GetSkin();
        }
        
        public void SetSkin(int skinId)
        {
            ChosenSkinId = skinId;
            SaveSystem.SetSkin(skinId);
        }
    }
}
