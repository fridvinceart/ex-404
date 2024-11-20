using UnityEngine;
using UnityEngine.UI;
//fidvince@gmx.us ENJOY!

[RequireComponent(typeof(Image))]
public class UIDistortionMaskController : MonoBehaviour
{
    public Material customMaterial;
    public Vector2 TopLeftOffset = Vector2.zero;
    public Vector2 TopRightOffset = Vector2.zero;
    public Vector2 BottomLeftOffset = Vector2.zero;
    public Vector2 BottomRightOffset = Vector2.zero;

    private Material materialInstance;
    private Image imageComponent;

    void OnEnable()
    {
        imageComponent = GetComponent<Image>();


        ApplyMaterial();
        UpdateMaterialProperties();
    }

    void OnRectTransformDimensionsChange()
    {

        UpdateMaterialProperties();
    }

    void ApplyMaterial()
    {
        if (imageComponent.material != null && imageComponent.material.shader == Shader.Find("fridvince/UIDistortedMask"))
        {
            materialInstance = Instantiate(imageComponent.material);
        }
        else if (customMaterial != null)
        {
            materialInstance = Instantiate(customMaterial);
        }
        else
        {
            Debug.LogError("No material assigned to the Image or custom material provided.");
            return;
        }

        imageComponent.material = materialInstance;
    }

    void UpdateMaterialProperties()
    {
        if (materialInstance != null)
        {
            materialInstance.SetVector("_TopLeftOffset", TopLeftOffset);
            materialInstance.SetVector("_TopRightOffset", TopRightOffset);
            materialInstance.SetVector("_BottomLeftOffset", BottomLeftOffset);
            materialInstance.SetVector("_BottomRightOffset", BottomRightOffset);
        }
    }

    void Update()
    {
        UpdateMaterialProperties();
    }
}