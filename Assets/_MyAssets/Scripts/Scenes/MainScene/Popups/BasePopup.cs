using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class BasePopup : MonoBehaviour
{
    [SerializeField] protected Button negativeButton;
    [SerializeField] protected Button positiveButton;
    CanvasGroup canvasGroup;

    protected Action onClickNegativeButton { get; set; }
    protected Action onClickPositiveButton { get; set; }
    protected float animDuration = 0.2f;


    public virtual void OnStart()
    {
        gameObject.SetActive(false);
        if (negativeButton) negativeButton.AddListener(() =>
        {
            OnClickNegativeButton();
            return UniTask.DelayFrame(0);
        });
        if (positiveButton) positiveButton.AddListener(() =>
        {
            OnClickPositiveButton();
            return UniTask.DelayFrame(0);
        });
        tag = "Popup";
        canvasGroup = GetComponent<CanvasGroup>();
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

    public async UniTask Close()
    {
        await OnClose();
    }

    protected virtual async UniTask OnClose()
    {
        onClickNegativeButton = null;
        onClickPositiveButton = null;

        await canvasGroup.DOFade(0f, animDuration).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }


    void OnClickNegativeButton()
    {
        onClickNegativeButton?.Invoke();
        OnClose().Forget();
    }

    void OnClickPositiveButton()
    {
        onClickPositiveButton?.Invoke();
        OnClose().Forget();
    }

    public virtual async UniTask Open()
    {
        // 今いる自分の階層の一番下に移動して、一番手前に表示されます。
        transform.SetAsLastSibling();

        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        await canvasGroup.DOFade(1f, animDuration).AsyncWaitForCompletion();
    }

}
