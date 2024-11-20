using UnityEngine;
//fridvince@gmx.us ENJOY!

public class CommonAutoRotation : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);
    public bool loop = false;
    public bool resetOnStop = true;
    public float rotationThreshold = 360f;
    private bool rotate = false;
    private Vector3 startRotation;
    private Vector3 initialRotation;

    void Start()
    {
        if (targetTransform != null)
        {
            initialRotation = targetTransform.eulerAngles;
        }
        else
        {
            Debug.LogError("Target Transform is not assigned.");
        }
    }

    void Update()
    {
        if (rotate)
        {
            Vector3 rotationStep = rotationSpeed * Time.deltaTime;
            targetTransform.Rotate(rotationStep);

            if (loop)
            {
                return;
            }
            else if (ReachedRotationThreshold())
            {
                rotate = false;
                if (resetOnStop)
                {
                    ResetRotation();
                }
                Debug.Log("Rotation stopped.");
            }
        }
    }

    private bool ReachedRotationThreshold()
    {
        Vector3 deltaRotation = targetTransform.eulerAngles - startRotation;
        return Mathf.Abs(deltaRotation.x) >= rotationThreshold ||
               Mathf.Abs(deltaRotation.y) >= rotationThreshold ||
               Mathf.Abs(deltaRotation.z) >= rotationThreshold;
    }

    private void ResetRotation()
    {
        targetTransform.eulerAngles = initialRotation;
    }

    public void StartRotation()
    {
        if (targetTransform != null)
        {
            rotate = true;
            startRotation = targetTransform.eulerAngles;
        }
        else
        {
            Debug.LogError("Target Transform is not assigned.");
        }
    }

    public void StopRotation()
    {
        rotate = false;
        if (resetOnStop)
        {
            ResetRotation();
        }
    }
    public void SetRotationSpeed(Vector3 newSpeed)
    {
        rotationSpeed = newSpeed;
    }
    public void SetRotationThreshold(float newThreshold)
    {
        rotationThreshold = newThreshold;
    }
}
