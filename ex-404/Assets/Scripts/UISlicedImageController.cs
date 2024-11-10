using UnityEngine;
using UnityEngine.UI;
//fidvince@gmx.us ENJOY!

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class UISlicedImageController : MonoBehaviour
{
    [Header("Distortion Offset Animation Curves")]
    public AnimationCurve topLeftCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve topLeftCurveY = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve topRightCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve topRightCurveY = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve bottomLeftCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve bottomLeftCurveY = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve bottomRightCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve bottomRightCurveY = AnimationCurve.Linear(0, 0, 1, 0);

    [Header("Animation Settings")]
    public float timeMultiplier = 1.0f;
    public bool loopAnimation = true;
    
    [Header("Alpha Cutoff")]
    public bool useAlphaCutoff = true;

    [Header("Slicing Borders (Normalized UV Coordinates)")]
    [Tooltip("Set according to your sprite's 9-slice borders.")]
    public float borderLeft = 0.25f;
    public float borderBottom = 0.25f;
    public float borderRight = 0.75f;
    public float borderTop = 0.75f;

    private Material material;
    private float timeElapsed;

    private void Start()
    {
        Image image = GetComponent<Image>();
        
    
        material = new Material(image.material);
        image.material = material;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * timeMultiplier;

        if (loopAnimation)
        {
            float maxTime = GetMaxCurveTime();
            if (timeElapsed > maxTime)
            {
                timeElapsed = 0;
            }
        }

        UpdateShader();
    }

    private float GetMaxCurveTime()
    {
        float maxTime = Mathf.Max(
            topLeftCurveX.keys[topLeftCurveX.length - 1].time,
            topLeftCurveY.keys[topLeftCurveY.length - 1].time,
            topRightCurveX.keys[topRightCurveX.length - 1].time,
            topRightCurveY.keys[topRightCurveY.length - 1].time,
            bottomLeftCurveX.keys[bottomLeftCurveX.length - 1].time,
            bottomLeftCurveY.keys[bottomLeftCurveY.length - 1].time,
            bottomRightCurveX.keys[bottomRightCurveX.length - 1].time,
            bottomRightCurveY.keys[bottomRightCurveY.length - 1].time
        );
        return maxTime;
    }

    private void UpdateShader()
    {
        Vector2 topLeftOffset = new Vector2(topLeftCurveX.Evaluate(timeElapsed), topLeftCurveY.Evaluate(timeElapsed));
        Vector2 topRightOffset = new Vector2(topRightCurveX.Evaluate(timeElapsed), topRightCurveY.Evaluate(timeElapsed));
        Vector2 bottomLeftOffset = new Vector2(bottomLeftCurveX.Evaluate(timeElapsed), bottomLeftCurveY.Evaluate(timeElapsed));
        Vector2 bottomRightOffset = new Vector2(bottomRightCurveX.Evaluate(timeElapsed), bottomRightCurveY.Evaluate(timeElapsed));

        material.SetVector("_TopLeftOffset", topLeftOffset);
        material.SetVector("_TopRightOffset", topRightOffset);
        material.SetVector("_BottomLeftOffset", bottomLeftOffset);
        material.SetVector("_BottomRightOffset", bottomRightOffset);

        material.SetFloat("_UseAlphaCutoff", useAlphaCutoff ? 1.0f : 0.0f);
        material.SetFloat("_BorderLeft", borderLeft);
        material.SetFloat("_BorderBottom", borderBottom);
        material.SetFloat("_BorderRight", borderRight);
        material.SetFloat("_BorderTop", borderTop);
    }
}