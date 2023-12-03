using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.PlayerLoop;
//using Codice.Client.Common;

public class ScrollSelector : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] TextMeshProUGUI originText;
    ScrollRect scrollRect;
    List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    int selectedIndex;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        string[] elements = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", };

        originText.text = elements[0];
        texts.Add(originText);

        for (int i = 1; i < elements.Length; i++)
        {
            var text = Instantiate(originText, scrollRect.content);
            text.text = elements[i];
            texts.Add(text);
        }
        scrollRect.verticalNormalizedPosition = 1;
    }


    public void OnEndDrag(PointerEventData data)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {


    }

    private void Update()
    {

        Debug.Log(scrollRect.verticalNormalizedPosition);
        float v = Mathf.Abs(scrollRect.velocity.y);

        if (!Input.GetMouseButton(0) && v < 300f)
        {
            var normalizedPos = ScrollToCore(0.5f);
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, normalizedPos, 5f * Time.deltaTime);
        }
    }

    // https://qiita.com/Shinoda_Naoki/items/346d349b7b81affe99d8
    private float ScrollToCore(float align)
    {
        int length = texts.Count;
        float y = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
        int num = Mathf.CeilToInt((length - 1) * (1 - y) + 1 / 2f);
        selectedIndex = Mathf.Clamp(num - 1, 0, length - 1);
        GameObject go = texts[selectedIndex].gameObject;

        //Debug.Log(num);


        var targetRect = go.transform.GetComponent<RectTransform>();
        var contentHeight = scrollRect.content.rect.height;
        var viewportHeight = scrollRect.viewport.rect.height;
        // スクロール不要
        if (contentHeight < viewportHeight) return 0f;

        // ローカル座標が contentHeight の上辺を0として負の値で格納されてる
        // これは現在のレイアウト特有なのかもしれないので、要確認
        var targetPos = contentHeight + GetPosY(targetRect) + targetRect.rect.height * align;
        var gap = viewportHeight * align; // 上端〜下端あわせのための調整量
        var normalizedPos = (targetPos - gap) / (contentHeight - viewportHeight);

        normalizedPos = Mathf.Clamp01(normalizedPos);
        // scrollRect.verticalNormalizedPosition = normalizedPos;
        return normalizedPos;
    }




    private float GetPosY(RectTransform transform)
    {
        return transform.localPosition.y + transform.rect.y; //pivotによるズレをrect.yで補正
    }



}
