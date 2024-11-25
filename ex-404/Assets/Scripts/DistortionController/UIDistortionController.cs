using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class UIDistortionController : MonoBehaviour
{
    [Header("Distortion Offset Animation Curves")]
    [SerializeField] private AnimationCurve _topLeftCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _topLeftCurveY = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _topRightCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _topRightCurveY = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _bottomLeftCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _bottomLeftCurveY = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _bottomRightCurveX = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private AnimationCurve _bottomRightCurveY = AnimationCurve.Linear(0, 0, 1, 0);

    [Header("Animation Settings")]
    public float timeMultiplier = 1.0f;
    public bool loopAnimation = true;

    [Header("Alpha Cutoff")]
    public bool useAlphaCutoff = true;

    [Header("Slicing Borders (Normalized UV Coordinates)")]
    [Tooltip("Set according to your sprite's 9-slice borders.")]
    public float borderLeft = 0.4f;
    public float borderBottom = 0.4f;
    public float borderRight = 0.4f;
    public float borderTop = 0.4f;

    private Material _material;
    private float _timeElapsed;

    private void Start()
    {
        var image = GetComponent<Image>();
        if (image.material != null)
        {
            _material = new Material(image.material);
            image.material = _material;
        }
        else
        {
            Debug.LogError("Image material is not assigned. Please assign a valid material.");
        }
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime * timeMultiplier;

        if (loopAnimation)
        {
            float maxTime = GetMaxCurveTime();
            if (_timeElapsed > maxTime)
            {
                _timeElapsed = 0;
            }
        }

        UpdateShader();
    }

    private float GetMaxCurveTime()
    {
        float maxTime = Mathf.Max(
            GetCurveMaxTime(_topLeftCurveX),
            GetCurveMaxTime(_topLeftCurveY),
            GetCurveMaxTime(_topRightCurveX),
            GetCurveMaxTime(_topRightCurveY),
            GetCurveMaxTime(_bottomLeftCurveX),
            GetCurveMaxTime(_bottomLeftCurveY),
            GetCurveMaxTime(_bottomRightCurveX),
            GetCurveMaxTime(_bottomRightCurveY)
        );
        return maxTime;
    }

    private float GetCurveMaxTime(AnimationCurve curve)
    {
        return curve.keys.Length > 0 ? curve.keys[curve.keys.Length - 1].time : 0f;
    }

    private void UpdateShader()
    {
        if (_material == null) return;

        Vector2 topLeftOffset = new Vector2(_topLeftCurveX.Evaluate(_timeElapsed), _topLeftCurveY.Evaluate(_timeElapsed));
        Vector2 topRightOffset = new Vector2(_topRightCurveX.Evaluate(_timeElapsed), _topRightCurveY.Evaluate(_timeElapsed));
        Vector2 bottomLeftOffset = new Vector2(_bottomLeftCurveX.Evaluate(_timeElapsed), _bottomLeftCurveY.Evaluate(_timeElapsed));
        Vector2 bottomRightOffset = new Vector2(_bottomRightCurveX.Evaluate(_timeElapsed), _bottomRightCurveY.Evaluate(_timeElapsed));

        _material.SetVector("_TopLeftOffset", topLeftOffset);
        _material.SetVector("_TopRightOffset", topRightOffset);
        _material.SetVector("_BottomLeftOffset", bottomLeftOffset);
        _material.SetVector("_BottomRightOffset", bottomRightOffset);

        _material.SetFloat("_UseAlphaCutoff", useAlphaCutoff ? 1.0f : 0.0f);
        _material.SetFloat("_BorderLeft", borderLeft);
        _material.SetFloat("_BorderBottom", borderBottom);
        _material.SetFloat("_BorderRight", borderRight);
        _material.SetFloat("_BorderTop", borderTop);
    }
}
