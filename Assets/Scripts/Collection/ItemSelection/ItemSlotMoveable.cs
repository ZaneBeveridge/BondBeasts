using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class ItemSlotMoveable : MonoBehaviour
{
    public ItemSlotType type;

    public Image icon;
    

    public Button button;

    [HideInInspector] public MonsterItemSO item;


    [HideInInspector]public GameManager manager;


    public abstract void OnClick();
}

public enum ItemSlotType
{
    EquipSlot,
    StorageSlot
}
