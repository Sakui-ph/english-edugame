using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableSlot : MonoBehaviour, IDropHandler
{
    public DraggableItem occupyingItem => transform.GetComponentInChildren<DraggableItem>() ? transform.GetComponentInChildren<DraggableItem>() : null;
    
    private bool isLocked = false;

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (isLocked)
            return;

        GameObject droppedObject = eventData.pointerDrag;

        if (!droppedObject.GetComponent<DraggableItem>())
            return;

        if (!IsAllowedType(droppedObject))
            return;
        
        DraggableItem draggable = droppedObject.GetComponent<DraggableItem>();
        
        if (occupyingItem == null)
            HandleEmptySlotDrop(draggable);
        else
            HandleOccupiedSlotDrop(draggable);
    }

    public virtual bool IsAllowedType(GameObject droppedObject)
    {
        return true;
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

    public void Lock()
    {
        occupyingItem.isDisabled = true;
        isLocked = true;
    }

    public void UnlocK()
    {
        occupyingItem.isDisabled = false;
        isLocked = false;
    }
}
