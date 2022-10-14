using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UITriggerHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public event Action<PointerEventData> OnBeginEvent;
    public event Action<PointerEventData> OnDragEvent;
    public event Action<PointerEventData> OnEndEvent;
    public event Action<PointerEventData> OnPressEvent;
    private PointerEventData currentData;
    private bool IsStartClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        currentData = eventData;
        OnBeginEvent?.Invoke(eventData);
        IsStartClick = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
        currentData = eventData;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndEvent?.Invoke(eventData);
        IsStartClick = false;
    }

    public void Update()
    {
        if (IsStartClick)
        {
            OnPressEvent?.Invoke(currentData);
        }
    }

}
