using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class BaseOpenScreenButton : BaseButton, IPointerDownHandler, IPointerUpHandler
{
    readonly float duration = 0.2f;

    public abstract void OnStart();

    public override void OnClick()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Button.interactable == false) return;
        transform.DOScale(Vector3.one * 1.1f, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, duration);
    }


}
