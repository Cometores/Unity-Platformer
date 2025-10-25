using System;
using Game._Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game._Scripts.UI.UI_Screens
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

        private DefaultInputActions _defaultInput;

        private float _inputCooldown = .3f;
        private float _lastTimeInput;

        private void Awake()
        {
            _defaultInput = new DefaultInputActions();
        }

        private Action<InputAction.CallbackContext> NavigateOnperformed()
        {
            return ctx =>
            {
                if (Time.time - _lastTimeInput < _inputCooldown)
                    return;
                
                if(ctx.ReadValue<Vector2>().x <= -1)
                    PreviousSkin();
                else if (ctx.ReadValue<Vector2>().x >= 1)
                    NextSkin();
            };
        }

        private void OnEnable()
        {
            _defaultInput.Enable();
            _defaultInput.UI.Navigate.performed += NavigateOnperformed();
        }
        

        private void OnDisable()
        {
            _defaultInput.UI.Navigate.performed += NavigateOnperformed();
            _defaultInput.Disable();
        }

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
                bool skinUnlocked = SaveSystem.IsSkinUnlocked(skinName);

                if (skinUnlocked || i == 0)
                    skins[i].unlocked = true;
            }
        }

        public void ConfirmSkin()
        {
            if (skins[_skinIndex].unlocked == false)
                BuySkin(_skinIndex);
            else
                SkinManager.Instance.SetSkin(_skinIndex);
        
            UpdateSkinDisplay();
        }

        public void NextSkin()
        {
            _lastTimeInput = Time.time;
            
            _skinIndex = (_skinIndex + 1) % skinAnimator.layerCount;
            UpdateSkinDisplay();
        }

        public void PreviousSkin()
        {
            _lastTimeInput = Time.time;
            
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
                selectBtnText.text = "SELECT";
            }
            else
            {
                priceText.transform.parent.gameObject.SetActive(true);
                priceText.text = skins[_skinIndex].skinPrice.ToString();
                selectBtnText.text = "BUY";
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

            SaveSystem.UnlockSkin(skinName);
        }

        private int FruitsInBank() => SaveSystem.GetFruitsInBank();

        private bool HaveEnoughFruits(int price)
        {
            if (FruitsInBank() >= price)
            {
                SaveSystem.SaveFruitsBank(FruitsInBank() - price);
                return true;
            }

            return false;
        }
    }
}