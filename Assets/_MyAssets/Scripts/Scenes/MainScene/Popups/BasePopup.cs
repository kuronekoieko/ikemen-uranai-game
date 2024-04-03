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
    //CanvasGroup canvasGroup;

    // protected Action onClickNegativeButton { get; set; }
    // protected Action onClickPositiveButton { get; set; }
    // protected float animDuration = 0.2f;
    protected bool isClosed = false;

    public virtual void OnStart()
    {
        gameObject.SetActive(false);
        if (negativeButton) negativeButton.AddListener(async () =>
        {
            OnClickNegativeButton();
            await UniTask.DelayFrame(0);
        });
        if (positiveButton) positiveButton.AddListener(async () =>
        {
            OnClickPositiveButton();
            await UniTask.DelayFrame(0);
        });
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
        Debug.Log("OnClose");
        // onClickNegativeButton = null;
        // onClickPositiveButton = null;

        //  await canvasGroup.DOFade(0f, animDuration).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        isClosed = true;
    }


    void OnClickNegativeButton()
    {
        // onClickNegativeButton?.Invoke();
        OnClose();
    }

    void OnClickPositiveButton()
    {
        // onClickPositiveButton?.Invoke();
        OnClose();
    }

    public virtual void Open()
    {
        isClosed = false;

        // 今いる自分の階層の一番下に移動して、一番手前に表示されます。
        transform.SetAsLastSibling();

        gameObject.SetActive(true);
        // canvasGroup.alpha = 0;
        // await canvasGroup.DOFade(1f, animDuration).AsyncWaitForCompletion();
    }

}
