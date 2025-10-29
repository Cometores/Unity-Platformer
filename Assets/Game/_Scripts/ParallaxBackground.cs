using UnityEngine;

[ExecuteAlways]
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float baseParallaxStrength = -0.1f;

    private Transform[] _layers;
    private Vector3 _previousCameraPos;

    private void Awake()
    {
        if (!cameraTransform)
            cameraTransform = Camera.main.transform;

        int count = transform.childCount;
        _layers = new Transform[count];
        for (int i = 0; i < count; i++)
            _layers[i] = transform.GetChild(i);

        _previousCameraPos = cameraTransform.position;
    }

    private void LateUpdate()
    {
        if (!cameraTransform || _layers == null) return;

        Vector3 delta = cameraTransform.position - _previousCameraPos;

        for (int i = 0; i < _layers.Length; i++)
        {
            var layer = _layers[i];
            if (!layer) continue;

            // Чем дальше слой, тем меньше его движение (глубина — через индекс)
            float parallax = baseParallaxStrength * (i + 1) / _layers.Length;
            float moveX = delta.x * parallax;

            layer.position += new Vector3(moveX, 0f, 0f);
        }

        _previousCameraPos = cameraTransform.position;
    }
}