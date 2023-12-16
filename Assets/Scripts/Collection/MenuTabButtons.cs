using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuTabButtons : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Transform buttonPos;
    public Image backgroundImage;
    public Button ownButton;

    public Color selectColor;
    public Color unselectedColor;

    public Vector3 selectPos;
    public Vector3 unselectedPos;

    public float selectFontSize;
    public float unselectedFontSize;

    public void ResetSelection()
    {
        backgroundImage.enabled = true;
        ownButton.enabled = true;
        text.enabled = true;

        buttonPos.localPosition = unselectedPos;
        backgroundImage.color = unselectedColor;
        text.fontSize = unselectedFontSize;
    }

    public void Select()
    {
        backgroundImage.enabled = true;
        ownButton.enabled = true;
        text.enabled = true;

        buttonPos.localPosition = selectPos;
        backgroundImage.color = selectColor;
        text.fontSize = selectFontSize;
    }

    public void Hide()
    {
        backgroundImage.enabled = false;
        ownButton.enabled = false;
        text.enabled = false;
    }
}
