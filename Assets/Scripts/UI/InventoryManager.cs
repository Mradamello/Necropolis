using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject Inventory;
    GameObject draggedItem;
    GameObject lastItemSlot;
    private bool openInventory = false;

    void Update()
    {
        if(draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }

        if(Input.GetKeyDown("e"))
        {
            openInventory = !openInventory;
        }

        Inventory.SetActive(openInventory);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            InventorySlot slot = clickedObject.GetComponent<InventorySlot>();
            if(slot != null && slot.heldItem != null)
            {
                draggedItem = slot.heldItem;
                slot.heldItem = null;
                lastItemSlot = clickedObject;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(draggedItem != null && eventData.pointerCurrentRaycast.gameObject != null && eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            InventorySlot slot = clickedObject.GetComponent<InventorySlot>();
            if(slot != null && slot.heldItem == null)
            {
                slot.SetHeldItem(draggedItem);
                draggedItem = null;
            }else if(slot != null && slot.heldItem != null)
            {
                lastItemSlot.GetComponent<InventorySlot>().SetHeldItem(slot.heldItem);
                slot.SetHeldItem(draggedItem);
                draggedItem = null;
            }
        }
    }
}
