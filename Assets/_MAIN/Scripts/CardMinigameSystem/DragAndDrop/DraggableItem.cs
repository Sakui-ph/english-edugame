using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This class assumes that all draggableItems will come from a LayoutGroup home to a DroppableSlot
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const int DRAG_OUT_THRESHOLD = 100;
    private Canvas canvas;
    private Canvas draggingCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentOnGameStart;
    public Transform parentAfterDrag;

    public bool isDisabled = false;
    public bool isSnapToParent = true;
    public bool isInSlot = false;
    public DroppableSlot slot => isInSlot ? transform.parent.GetComponent<DroppableSlot>() : null;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public virtual void Start()
    {
        parentOnGameStart = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        // do nothing...
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDisabled)
            return;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
        draggingCanvas = gameObject.AddComponent<Canvas>();
        draggingCanvas.overrideSorting = true;
        draggingCanvas.sortingOrder = 1;

        parentAfterDrag = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDisabled)
            return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;


        if (parentAfterDrag == transform.parent)
            TryResetParent();

        ChangeParent(parentAfterDrag);
        Destroy(draggingCanvas);
    }


    private void FitToSlot()
    {
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchorMax = new Vector2(1,1);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.offsetMin = new Vector2(0,0);
        rectTransform.offsetMax = new Vector2(0,0);
    }

    private void FitToLayoutGroup()
    {
        RectTransform rectTransform = (RectTransform)transform;
        LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
        rectTransform.sizeDelta = new Vector2(layoutElement.preferredWidth, layoutElement.preferredHeight);
    }

    private void HandleDrop()
    {
        if (transform.parent.GetComponent<LayoutGroup>())
            DropIntoLayoutGroup();
        else if (transform.parent.GetComponent<DroppableSlot>())
            DropIntoDroppableSlot();
        else
            Debug.LogWarning("Dropping into this kind of object is not supported");
    }

    public void ResetParent()
    {
        transform.parent = parentOnGameStart;
        HandleDrop();
    }

    public void ChangeParent(Transform parent)
    {
        transform.parent = parent;
        HandleDrop();
    }

    private void DropIntoLayoutGroup()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)parentAfterDrag);
        FitToLayoutGroup();
        isInSlot = false;
    }

    private void DropIntoDroppableSlot()
    {
        FitToSlot();
        isInSlot = true;
    }

    private void TryResetParent()
    {
        if (isInSlot)
        {
            RectTransform rt = (RectTransform)transform;
            if (Math.Abs(rt.anchoredPosition.x) >= DRAG_OUT_THRESHOLD || Math.Abs(rt.anchoredPosition.y) >= DRAG_OUT_THRESHOLD) 
            {
                parentAfterDrag = parentOnGameStart;
            }
        }
    }
}
