using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button, IPointerDownHandler, IPointerUpHandler
{
    public Action onPointerDown;
    public Action onPointerUp;
    bool isButtonDown;

    protected async UniTask Task()
    {
        await UniTask.WaitUntil(() => isButtonDown == false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (interactable == false) return;
        onPointerDown?.Invoke();
        isButtonDown = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (interactable == false) return;
        onPointerUp?.Invoke();
        isButtonDown = false;
    }


}
