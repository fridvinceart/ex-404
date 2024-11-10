using UnityEngine;
//fidvince@gmx.us ENJOY!

public class WorldInputRotate : MonoBehaviour
{
    public Transform targetTransform;
    public float rotationSpeed = 1.0f;
    public bool isRotationActive = false;

    void Update()
    {
        if (isRotationActive && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchDelta = touch.deltaPosition;

            float rotationY = touchDelta.x * rotationSpeed * Time.deltaTime;

            if (targetTransform != null)
            {
                targetTransform.Rotate(Vector3.up * rotationY);
            }
            else
            {
                Debug.LogError("Target Transform is not assigned.");
            }
        }
    }

    public void ActivateRotation()
    {
        isRotationActive = true;
    }

    public void DeactivateRotation()
    {
        isRotationActive = false;
    }
}