using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;

    private RectTransform draggingObject;
    private Button button;
    private CanvasGroup canvasGroup;

    [HideInInspector] public bool dragging = false;

    [HideInInspector] public GameManager manager;

    private Vector3 startPosition;
    private void Awake()
    {
        draggingObject = transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = draggingObject.position;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        button.interactable = false;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObject, eventData.position, eventData.pressEventCamera, out var globalMousePosition))
        {
            draggingObject.position = globalMousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingObject.position = startPosition;
        transform.SetParent(parentAfterDrag);

        button.interactable = true;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        dragging = false;
    }

}
