using Game._Scripts.Managers;
using UnityEngine;

namespace Game._Scripts
{
    public enum FruitType
    {
        Apple,
        Banana,
        Strawberry,
        Pineapple,
        Orange,
        Melon,
        Kiwi,
        Cherry
    }

    public class Fruit : MonoBehaviour
    {
        private GameManager _gameManager;
        private Animator _animator;

        [SerializeField] private FruitType fruitType;

        [SerializeField] private GameObject pickupVFX;
    
        [SerializeField] private int animCount = 8;
        private static readonly int FruitIndex = Animator.StringToHash("fruitIndex");


        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            SetRandomLookIfNeeded();
        }

        private void SetRandomLookIfNeeded()
        {
            if (!FruitManager.Instance.FruitsHaveRandomLook())
            {
                UpdateFruitVisuals();
                return;
            }

            int randomIndex = Random.Range(0, animCount);
            _animator.SetFloat(FruitIndex, randomIndex);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player.Player player = other.GetComponent<Player.Player>();

            if (player)
            {
                FruitManager.Instance.AddFruit();
                AudioManager.Instance.PlaySfx(8);
                Destroy(gameObject);

                Instantiate(pickupVFX, transform.position, Quaternion.identity);
            }
        }

        private void UpdateFruitVisuals() => _animator.SetFloat(FruitIndex, (int)fruitType);
    }
}