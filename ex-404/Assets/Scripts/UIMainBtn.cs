using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
//fridvince@gmx.us ENJOY!

[RequireComponent(typeof(CanvasRenderer))]
public class UIMainBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 highlightedScale = Vector3.one;
    public Vector3 pressedScale = new Vector3(0.9f, 0.9f, 0.9f);
    public Vector3 disabledScale = new Vector3(0.1f, 0.1f, 0.1f);
    public bool isInteractable = true;

    private Transform buttonTransform;
    public UnityEvent onClick;

    private void Start()
    {
        buttonTransform = transform;
        SetButtonScale(normalScale);
    }

    private void SetButtonScale(Vector3 scale)
    {
        buttonTransform.localScale = scale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInteractable)
        {
            SetButtonScale(highlightedScale);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInteractable)
        {
            SetButtonScale(normalScale);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isInteractable)
        {
            SetButtonScale(pressedScale);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInteractable)
        {
            SetButtonScale(highlightedScale);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInteractable)
        {
            onClick.Invoke(); // Trigger the click event
        }
    }

    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
        SetButtonScale(interactable ? normalScale : disabledScale);
    }
}