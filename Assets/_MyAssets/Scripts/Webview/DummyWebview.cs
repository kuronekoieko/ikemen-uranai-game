using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class DummyWebview : SingletonMonoBehaviour<DummyWebview>
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform canvasRT;

    private void Start()
    {
        Close();
        Debug.Log(rectTransform.anchorMin);
    }

    public void Show(RectInt rect)
    {
        gameObject.SetActive(true);
        // rectTransform.sizeDelta = rect.size;
        //rectTransform.anchoredPosition = rect.position;

        rectTransform.SetRect(rect.ToRect());
        // Debug.Log("DummyWebview: " + rectTransform.rect);

    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
