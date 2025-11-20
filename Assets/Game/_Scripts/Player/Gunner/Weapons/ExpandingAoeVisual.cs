using UnityEngine;

namespace Game._Scripts.Player.Gunner.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ExpandingAoeVisual : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private AnimationCurve _alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        private ExpandingAoeWave _wave;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _wave = GetComponent<ExpandingAoeWave>();
            _renderer = GetComponent<SpriteRenderer>();

            _renderer.color = _color;
            _renderer.transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            float t = Mathf.Clamp01(_wave.Elapsed / _wave.Duration);

            float scale = _wave.CurrentRadius * 2f; // for circle sprite
            _renderer.transform.localScale = new Vector3(scale, scale, 1f);

            Color c = _renderer.color;
            c.a = _alphaCurve.Evaluate(t);
            _renderer.color = c;
        }
    }
}