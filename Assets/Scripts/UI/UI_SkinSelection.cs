using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    [System.Serializable]
    public struct Skin
    {
        public string skinName;
        public int skinPrice;
        public bool unlocked;
    }

    public class UI_SkinSelection : MonoBehaviour
    {
        [SerializeField] private Skin[] skins;

        [Header("UI details")]
        [SerializeField] private Animator skinAnimator;
        private int _skinIndex;

        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI bankText;
        [SerializeField] private TextMeshProUGUI selectBtnText;

        private void Start()
        {
            LoadSkinsUnlocks();
            UpdateSkinDisplay();
        }

        private void LoadSkinsUnlocks()
        {
            for (int i = 0; i < skins.Length; i++)
            {
                string skinName = skins[i].skinName;
                bool skinUnlocked = PlayerPrefs.GetInt($"{skinName}Unlocked", 0) == 1;

                if (skinUnlocked || i == 0)
                    skins[i].unlocked = true;
            }
        }

        public void ConfirmSkin()
        {
            if (skins[_skinIndex].unlocked == false)
                BuySkin(_skinIndex);
            else
                SkinManager.instance.ChosenSkinId = _skinIndex;
        
            UpdateSkinDisplay();
        }

        public void NextSkin()
        {
            _skinIndex = (_skinIndex + 1) % skinAnimator.layerCount;
            UpdateSkinDisplay();
        }

        public void PreviousSkin()
        {
            _skinIndex--;
            if (_skinIndex < 0)
            {
                _skinIndex = skinAnimator.layerCount - 1;
            }

            UpdateSkinDisplay();
        }


        /// <summary>
        /// Updates the display of the selected skin to reflect its status and price.
        /// </summary>
        private void UpdateSkinDisplay()
        {
            bankText.text = FruitsInBank().ToString();

            for (int i = 0; i < skinAnimator.layerCount; i++)
            {
                skinAnimator.SetLayerWeight(i, 0);
            }

            skinAnimator.SetLayerWeight(_skinIndex, 1);

            if (skins[_skinIndex].unlocked)
            {
                priceText.transform.parent.gameObject.SetActive(false);
                selectBtnText.text = "Select";
            }
            else
            {
                priceText.transform.parent.gameObject.SetActive(true);
                priceText.text = skins[_skinIndex].skinPrice.ToString();
                selectBtnText.text = "Buy";
            }
        }

        private void BuySkin(int index)
        {
            if (HaveEnoughFruits(skins[index].skinPrice) == false)
            {
                AudioManager.Instance.PlaySfx(6);
                return;
            }

            AudioManager.Instance.PlaySfx(10);
            string skinName = skins[index].skinName;
            skins[index].unlocked = true;

            PlayerPrefs.SetInt($"{skinName}Unlocked", 1);
        }

        private int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");

        private bool HaveEnoughFruits(int price)
        {
            if (FruitsInBank() >= price)
            {
                PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price);
                return true;
            }

            return false;
        }
    }
}