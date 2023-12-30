using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.PlayerLoop;
using Cysharp.Threading.Tasks;
//using Codice.Client.Common;
using MainScene;

public class ScrollSelector : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI originText;
    ScrollRect scrollRect;
    readonly List<TextMeshProUGUI> texts = new();
    public int SelectedIndex => GetNearIndex();

    public async void OnStart(List<string> datas, int startIndex)
    {
        scrollRect = GetComponent<ScrollRect>();

        originText.text = datas[0];
        texts.Add(originText);

        for (int i = 1; i < datas.Count; i++)
        {
            var text = Instantiate(originText, scrollRect.content);
            text.text = datas[i];
            texts.Add(text);
        }
        await UniTask.DelayFrame(1);
        var normalizedPos = ScrollToCore(startIndex, 0.5f);
        scrollRect.verticalNormalizedPosition = normalizedPos;
        Initialize.Instance.OnUpdate += OnUpdate;
    }

    public void ShowDay(int daysInMonth)
    {
        for (int i = 1; i < texts.Count; i++)
        {
            texts[i].gameObject.SetActive(i < daysInMonth);
        }
    }

    private void OnUpdate()
    {
        if (scrollRect == null) return;
        // Debug.Log(scrollRect.verticalNormalizedPosition);
        float v = Mathf.Abs(scrollRect.velocity.y);

        if (!Input.GetMouseButton(0) && v < 300f)
        {
            var selectedIndex = GetNearIndex();
            var normalizedPos = ScrollToCore(selectedIndex, 0.5f);
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, normalizedPos, 3f * Time.deltaTime);
        }
    }

    int GetNearIndex()
    {
        int length = texts.Count;
        float y = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
        // 0は下、1は上
        // それぞれ要素の真ん中
        // 一要素の長さd=1/(length-1)
        // (num-1)*d-d/2<1-y<(num-1)*d+d/2
        // を解く
        int num = Mathf.CeilToInt((length - 1) * (1 - y) + 1 / 2f);
        //Debug.Log(num);

        return Mathf.Clamp(num - 1, 0, length - 1);
    }

    // https://qiita.com/Shinoda_Naoki/items/346d349b7b81affe99d8
    private float ScrollToCore(int selectedIndex, float align)
    {
        var targetRect = texts[selectedIndex].rectTransform;
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
