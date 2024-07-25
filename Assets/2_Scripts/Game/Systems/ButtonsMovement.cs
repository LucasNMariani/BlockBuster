using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    DragController dragController;
    [SerializeField] bool isRightButton;

    private void Start()
    {
        dragController = FindObjectOfType<DragController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isRightButton) dragController.OnLeftButtonDown();
        else dragController.OnRightButtonDown();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isRightButton) dragController.OnLeftButtonUp();
        else dragController.OnRightButtonUp();
    }
}
