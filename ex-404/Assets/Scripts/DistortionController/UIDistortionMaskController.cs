using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class UIDistortionMaskController : MonoBehaviour
{
    [SerializeField] private Material customMaterial;
    [SerializeField] private Vector2 topLeftOffset = Vector2.zero;
    [SerializeField] private Vector2 topRightOffset = Vector2.zero;
    [SerializeField] private Vector2 bottomLeftOffset = Vector2.zero;
    [SerializeField] private Vector2 bottomRightOffset = Vector2.zero;

    private Material _materialInstance;
    private Image _imageComponent;

    private static readonly int TopLeftOffsetID = Shader.PropertyToID("_TopLeftOffset");
    private static readonly int TopRightOffsetID = Shader.PropertyToID("_TopRightOffset");
    private static readonly int BottomLeftOffsetID = Shader.PropertyToID("_BottomLeftOffset");
    private static readonly int BottomRightOffsetID = Shader.PropertyToID("_BottomRightOffset");

    private void OnEnable()
    {
        _imageComponent = GetComponent<Image>();
        if (_imageComponent == null)
        {
            Debug.LogError("Image component is missing.");
            return;
        }

        ApplyMaterial();
        UpdateMaterialProperties();
    }

    private void OnDisable()
    {
        if (_materialInstance != null)
        {
            if (Application.isPlaying)
            {
                Destroy(_materialInstance);
            }
            else
            {
                DestroyImmediate(_materialInstance);
            }
            _materialInstance = null;
        }

        if (_imageComponent != null)
        {
            _imageComponent.material = null;
        }
    }


    private void OnRectTransformDimensionsChange()
    {
        UpdateMaterialProperties();
    }

    private void ApplyMaterial()
    {
        if (_imageComponent.material != null && _imageComponent.material.shader.name == "fridvince/UIDistortedMask")
        {
            _materialInstance = Instantiate(_imageComponent.material);
        }
        else if (customMaterial != null)
        {
            _materialInstance = Instantiate(customMaterial);
        }
        else
        {
            Debug.LogError("No material assigned to the Image or custom material provided.");
            return;
        }

        _imageComponent.material = _materialInstance;
    }

    private void UpdateMaterialProperties()
    {
        if (_materialInstance == null)
            return;

        _materialInstance.SetVector(TopLeftOffsetID, topLeftOffset);
        _materialInstance.SetVector(TopRightOffsetID, topRightOffset);
        _materialInstance.SetVector(BottomLeftOffsetID, bottomLeftOffset);
        _materialInstance.SetVector(BottomRightOffsetID, bottomRightOffset);
    }

    private void Update()
    {
        // Update material properties only if offsets are changing frequently
        // Remove this if offsets are static or change on specific events.
        UpdateMaterialProperties();
    }
}
