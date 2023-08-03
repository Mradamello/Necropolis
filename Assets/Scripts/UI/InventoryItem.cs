using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemsSO itemSO;
    [SerializeField] Image icon;
    
    void Update()
    {
        icon.sprite = itemSO.icon;
    }
}