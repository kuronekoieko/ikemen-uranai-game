using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        if (closeButton) closeButton.AddListener(Close, AudioID.BtnClick_Negative);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        if (scrollRect) scrollRect.verticalNormalizedPosition = 1f;
    }

    public virtual UniTask Close()
    {
        gameObject.SetActive(false);
        return UniTask.DelayFrame(0);
    }
}
