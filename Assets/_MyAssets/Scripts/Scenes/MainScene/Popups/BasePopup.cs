using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class BasePopup : MonoBehaviour
{
    [SerializeField] protected Button negativeButton;
    [SerializeField] protected Button positiveButton;
    CanvasGroup canvasGroup;

    protected Action onClickNegativeButton { get; set; }
    protected Action onClickPositiveButton { get; set; }


    public virtual void OnStart()
    {
        gameObject.SetActive(false);
        if (negativeButton) negativeButton.onClick.AddListener(OnClickNegativeButton);
        if (positiveButton) positiveButton.onClick.AddListener(OnClickPositiveButton);
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

    public void Close()
    {
        OnClose();
    }

    protected virtual void OnClose()
    {
        onClickNegativeButton = null;
        onClickPositiveButton = null;

        canvasGroup.DOFade(0f, 0.2f)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }


    void OnClickNegativeButton()
    {
        onClickNegativeButton?.Invoke();
        OnClose();
    }

    void OnClickPositiveButton()
    {
        onClickPositiveButton?.Invoke();
        OnClose();
    }

    public virtual void Open()
    {
        // 今いる自分の階層の一番下に移動して、一番手前に表示されます。
        transform.SetAsLastSibling();

        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, 0.2f);
    }

}
