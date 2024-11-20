using UnityEngine;
//fidvince@gmx.us ENJOY!

public class CommonAutoRotation : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 rotationSpeed = new Vector3(10, 10, 10);
    public bool loop = false;
    public bool resetOnStop = true;
    private bool rotate = false;
    private Vector3 startRotation;
    private Vector3 initialRotation;

    void Start()
    {
        if (targetTransform != null)
        {
            startRotation = targetTransform.eulerAngles;
            initialRotation = Vector3.zero;
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
                targetTransform.Rotate(rotationStep);
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

        return false;
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
}