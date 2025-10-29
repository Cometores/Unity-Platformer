using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game._Scripts.Managers
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Traps")]
        public GameObject arrowPrefab;

        public static ObjectSpawner Instance => _instance;
        private static ObjectSpawner _instance;

        private int _spawnEpoch;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnEnable() => _spawnEpoch++;
        private void OnDisable()
        {
            _spawnEpoch++;
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene _, Scene __)
        {
            _spawnEpoch++;
            StopAllCoroutines();
        }

        public void CreateObject(GameObject prefab, Transform target, float delay = 0f)
        {
            if (!prefab || !target) return;
            StartCoroutine(CreateObjectCoroutine(prefab, target, delay, _spawnEpoch));
        }

        private IEnumerator CreateObjectCoroutine(GameObject prefab, Transform target, float delay, int epochAtStart)
        {
            Vector3 newPosition = target.position;

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            if (epochAtStart != _spawnEpoch || this == null || !isActiveAndEnabled)
                yield break;

            Instantiate(prefab, newPosition, Quaternion.identity);
        }

        // На случай, если доменный перезагрузчик в Unity отключён (Enter Play Mode Options),
        // и статика не сбрасывается между перезапусками Play Mode:
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => _instance = null;
    }
}
