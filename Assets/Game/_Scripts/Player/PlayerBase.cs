using System.Collections;
using Game._Scripts.Managers;
using Game._Scripts.Utils;
using UnityEngine;

namespace Game._Scripts.Player
{
    public abstract class PlayerBase: MonoBehaviour
    {
        [Header("Knockback")]
        [SerializeField] private float knockbackDuration;
        [SerializeField] private Vector2 knockbackPower;
        
        [SerializeField] private DifficultyType gameDifficulty;
        
        protected Rigidbody2D Rb;
        protected Animator Anim;
        protected bool IsKnocked;
        
        private static readonly int IsKnockedHash = Animator.StringToHash("isKnocked");

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Anim = GetComponentInChildren<Animator>();
        }

        protected virtual void Start()
        {
            SetGameDifficulty();
        }
        
        public abstract void DisableInput();
        public abstract void EnableInput();
        
        public void Knockback(float sourceDamageXPosition)
        {
            if (IsKnocked) return;
            
            AudioManager.Instance.PlaySfx(9);
            
            float knockbackDir = transform.position.x < sourceDamageXPosition ? -1 : 1;

            if (CameraManager.Instance)
                CameraManager.Instance.CameraShake(knockbackDir);

            StartCoroutine(KnockbackRoutine());
            Rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        }
        
        private IEnumerator KnockbackRoutine()
        {
            IsKnocked = true;
            Anim.SetBool(IsKnockedHash, true);

            yield return Helpers.GetWait(knockbackDuration);

            IsKnocked = false;
            Anim.SetBool(IsKnockedHash, false);
        }
        
        public void GetDamage()
        {
            if (gameDifficulty == DifficultyType.Normal)
            {
                if (FruitManager.Instance.FruitsCollected() <= 0)
                    GameManager.RestartLevel();
                else
                    FruitManager.Instance.RemoveFruit();

                return;
            }

            if (gameDifficulty == DifficultyType.Hard)
            {
                GameManager.RestartLevel();
            }
        }

        private void SetGameDifficulty()
        {
            DifficultyManager difficultyManager = DifficultyManager.Instance;
            if (difficultyManager != null)
                gameDifficulty = difficultyManager.difficulty;
        }
    }
}