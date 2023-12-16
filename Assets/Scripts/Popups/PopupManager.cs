using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public Transform parentObject;
    public GameObject healedObject;

    public void FullyHealed()
    {
        GameObject obj = Instantiate(healedObject, parentObject);
        obj.GetComponent<FadeOutText>().Fade(1.5f);
    }
}
