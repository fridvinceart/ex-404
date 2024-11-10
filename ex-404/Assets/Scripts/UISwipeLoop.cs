using UnityEngine;
using UnityEngine.Events;
//fridvince@gmx.us ENJOY!

public class UISwipeLoop : MonoBehaviour
{
    [System.Serializable] public class SwipeEvent
    {
        public Vector3 position;
        public UnityEvent onSelected;
        public UnityEvent onDeselected;
    }

    private float swipeThreshold = 50f;
    private float swipeSpeed = 1f;
    public float autoSwapDistance = 300f;

    public GameObject[] objects;
    public SwipeEvent[] swipeEvents;

    private Vector2 startTouchPosition;
    private bool swipeInProgress = false;
    private int currentIndex = 0;
    private Vector3 dragOffset = Vector3.zero;

    private int lastSelectedIndex = -1;

    void Start()
    {
        if (swipeEvents.Length == 0 || objects.Length == 0)
        {
            Debug.LogError("No swipe events or objects assigned.");
            return;
        }
        UpdatePositions();
        CheckAndSetSelectedItem();
    }

    void OnEnable()
    {
        ResetAnimationTriggers();
        CheckAndSetSelectedItem();
    }

    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                swipeInProgress = true;
            }
            else if (touch.phase == TouchPhase.Moved && swipeInProgress)
            {
                float deltaX = touch.position.x - startTouchPosition.x;
                dragOffset = new Vector3(deltaX, 0, 0);
                UpdateDragPositions(dragOffset);

                if (Mathf.Abs(deltaX) >= autoSwapDistance)
                {
                    if (deltaX > 0)
                    {
                        SwipeRight();
                    }
                    else
                    {
                        SwipeLeft();
                    }
                    swipeInProgress = false;
                }
            }
            else if (touch.phase == TouchPhase.Ended && swipeInProgress)
            {
                swipeInProgress = false;
                float swipeDeltaX = touch.position.x - startTouchPosition.x;

                if (Mathf.Abs(swipeDeltaX) > swipeThreshold)
                {
                    if (swipeDeltaX > 0)
                    {
                        SwipeRight();
                    }
                    else
                    {
                        SwipeLeft();
                    }
                }
                else
                {
                    ResetPositions();
                }
            }
        }

        MoveObjectsToPositions();
    }

    void UpdateDragPositions(Vector3 offset)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            int positionIndex = (currentIndex + i - objects.Length / 2 + objects.Length) % objects.Length;
            objects[i].transform.localPosition = swipeEvents[positionIndex].position + offset;
        }
    }

    void SwipeLeft()
    {
        currentIndex = (currentIndex + 1) % objects.Length;
        UpdatePositions();
        TriggerSelectedEvent();
    }

    void SwipeRight()
    {
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length;
        UpdatePositions();
        TriggerSelectedEvent();
    }

    void UpdatePositions()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            int positionIndex = (currentIndex + i - objects.Length / 2 + objects.Length) % objects.Length;
            objects[i].transform.localPosition = swipeEvents[positionIndex].position;
        }
    }

    void ResetPositions()
    {
        dragOffset = Vector3.zero;
        UpdatePositions();
    }

    void MoveObjectsToPositions()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            int positionIndex = (currentIndex + i - objects.Length / 2 + objects.Length) % objects.Length;
            Vector3 targetPosition = swipeEvents[positionIndex].position;
            objects[i].transform.localPosition = Vector3.Lerp(objects[i].transform.localPosition, targetPosition, Time.deltaTime * swipeSpeed);
        }
        
        CheckAndSetSelectedItem();
    }

    void CheckAndSetSelectedItem()
    {
        float minDistance = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < objects.Length; i++)
        {
            float distance = Mathf.Abs(objects[i].transform.localPosition.x);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        if (closestIndex != lastSelectedIndex)
        {
            if (lastSelectedIndex >= 0 && lastSelectedIndex < swipeEvents.Length)
            {
                swipeEvents[lastSelectedIndex].onDeselected.Invoke();
            }

            lastSelectedIndex = closestIndex;

            if (closestIndex >= 0 && closestIndex < swipeEvents.Length)
            {
                swipeEvents[closestIndex].onSelected.Invoke();
            }
        }
    }

    void TriggerSelectedEvent()
    {
        CheckAndSetSelectedItem();
    }

    void ResetAnimationTriggers()
    {
        if (lastSelectedIndex >= 0 && lastSelectedIndex < swipeEvents.Length)
        {
            swipeEvents[lastSelectedIndex].onDeselected.Invoke();
            swipeEvents[lastSelectedIndex].onSelected.Invoke();
        }
    }
}
