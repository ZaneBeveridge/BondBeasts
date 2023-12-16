using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Slot : MonoBehaviour
{
    public SlotType type;

    public Image dynamicImage;
    public Image staticImage;
    public Image variantImage;

    

    public Button button;

    public Monster storedMonster;


    public CollectionManager manager;


    public abstract void OnClick();
}

public enum SlotType
{
    Collection,
    Party
}

