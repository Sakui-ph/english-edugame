using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableSlot : MonoBehaviour, IDropHandler
{
    public DraggableItem occupyingItem => transform.GetComponentInChildren<DraggableItem>() ? transform.GetComponentInChildren<DraggableItem>() : null;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (!droppedObject.GetComponent<DraggableItem>())
            return;
        
        DraggableItem draggable = droppedObject.GetComponent<DraggableItem>();
        
        if (occupyingItem == null)
            HandleEmptySlotDrop(draggable);
        else
            HandleOccupiedSlotDrop(draggable);
    }

    public void HandleEmptySlotDrop(DraggableItem draggable)
    {
        draggable.parentAfterDrag = transform;
    }

    public void HandleOccupiedSlotDrop(DraggableItem draggable)
    {
        // child swap
        if (draggable.isInSlot)
            occupyingItem.ChangeParent(draggable.slot.transform);
        // replace
        else 
            occupyingItem.ResetParent();

        draggable.parentAfterDrag = transform;
    }
}
