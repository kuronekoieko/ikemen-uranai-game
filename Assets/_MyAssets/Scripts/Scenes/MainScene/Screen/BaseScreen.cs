using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseScreen : MonoBehaviour
{
    protected ScrollRect scrollRect;

    public virtual void OnStart()
    {
        gameObject.SetActive(false);
        scrollRect = GetComponentInChildren<ScrollRect>();
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
