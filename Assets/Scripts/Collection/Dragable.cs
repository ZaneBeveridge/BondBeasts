using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    public bool active = true;
    private RectTransform draggingObject;
    private Button button;
    private CanvasGroup canvasGroup;

    private Vector3 startPosition;

    private GameManager GM;
    private void Awake()
    {
        draggingObject = transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!active) { return; }
        startPosition = draggingObject.position;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        button.interactable = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        GM.collectionManager.StartDrag(this.gameObject); // Created and ready for implementing of ondraggin funtions
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!active) { return; }
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObject, eventData.position, eventData.pressEventCamera, out var globalMousePosition))
        {
            draggingObject.position = globalMousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!active) { return; }
        GM.collectionManager.EndDrag(this.gameObject);// Created and ready for implementing of ondraggin funtions (that turns off at end)
        draggingObject.position = startPosition;
        transform.SetParent(parentAfterDrag);

        button.interactable = true;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        
    }

}
