using UnityEngine;
using UnityEngine.UI;
//fridvince@gmx.us ENJOY!

[RequireComponent(typeof(Image))] // Ensure the component has an Image
public class UIStensilReferenceModifier : MonoBehaviour
{
    [Range(0, 255)] // Adjust the range as needed
    public int stencilReference = 1; // Default value

    private Material _sharedMaterial; // Reference to the shared material

    void Awake()
    {
        // Get the Image component
        Image image = GetComponent<Image>();

        // Use the existing material for shared use
        _sharedMaterial = image.material; 

        // Ensure the material has the property before setting it
        if (_sharedMaterial.HasProperty("_StencilRef"))
        {
            _sharedMaterial.SetFloat("_StencilRef", stencilReference); // Set the stencil reference
        }
    }

    void OnValidate()
    {
        // Update the stencil reference value when changed in the Inspector
        if (_sharedMaterial != null && _sharedMaterial.HasProperty("_StencilRef"))
        {
            _sharedMaterial.SetFloat("_StencilRef", stencilReference); // Update material property
        }
    }

    void OnDestroy()
    {
        // Optionally, you can comment this out if you want to keep the material
        // to be used by other elements
        // if (_sharedMaterial != null)
        // {
        //     Destroy(_sharedMaterial);
        // }
    }
}