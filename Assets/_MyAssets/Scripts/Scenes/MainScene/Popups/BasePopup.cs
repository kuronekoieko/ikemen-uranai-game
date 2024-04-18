using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class BasePopup : MonoBehaviour
{
    [SerializeField] protected CustomButton negativeButton;
    [SerializeField] protected CustomButton positiveButton;


    public virtual void OnStart()
    {
        gameObject.SetActive(false);

        if (negativeButton) negativeButton.AddListener(async () =>
        {
            OnClickNegativeButton();
            await UniTask.DelayFrame(0);
        }, AudioID.BtnClick_Negative);

        if (positiveButton) positiveButton.AddListener(async () =>
        {
            OnClickPositiveButton();
            await UniTask.DelayFrame(0);
        }, AudioID.BtnClick_Negative);

        tag = "Popup";
        // canvasGroup = GetComponent<CanvasGroup>();
        AddImage();
    }

    void AddImage()
    {
        // RaycastAllを反応させるため
        if (!TryGetComponent<Image>(out var image))
        {
            image = gameObject.AddComponent<Image>();
            image.color = Color.clear;
        }
    }

    public void Close()
    {
        OnClose();
    }

    protected virtual void OnClose()
    {
        gameObject.SetActive(false);
    }


    void OnClickNegativeButton()
    {
        OnClose();
    }

    void OnClickPositiveButton()
    {
        OnClose();
    }

    public virtual void Open()
    {
        // 今いる自分の階層の一番下に移動して、一番手前に表示されます。
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
    }

}
