using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class ItemSlot : MonoBehaviour
{
    public ItemSlotType slotType;

    public ItemType itemType;

    public Image icon;
    

    public Button button;

    public int amount;

    [HideInInspector]public MonsterItemSO item;


    [HideInInspector]public GameManager manager;


    public abstract void OnClick();
}

public enum ItemSlotType
{
    StorageSlot,
    EquipSlot
}
