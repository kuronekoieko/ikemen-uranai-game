using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseScreen : MonoBehaviour
{
    protected ScrollRect scrollRect;
    [SerializeField] Button closeButton;


    public virtual void OnStart()
    {
        gameObject.SetActive(false);
        scrollRect = GetComponentInChildren<ScrollRect>();
        if (closeButton) closeButton.onClick.AddListener(Close);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        if (scrollRect) scrollRect.verticalNormalizedPosition = 1f;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
