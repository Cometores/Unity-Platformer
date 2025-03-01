using UnityEngine;

public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 movementDirection;
    private MeshRenderer _mesh;
    
    [Header("Color")]
    [SerializeField] private BackgroundType backgroundType;

    [SerializeField] private Texture2D[] textures;

    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }

    private void Update()
    {
        _mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    [ContextMenu("Update background")]
    private void UpdateBackgroundTexture()
    {
        if (_mesh == null)
            _mesh = GetComponent<MeshRenderer>();
        
        _mesh.sharedMaterial.mainTexture = textures[(int)backgroundType];
    }
}
