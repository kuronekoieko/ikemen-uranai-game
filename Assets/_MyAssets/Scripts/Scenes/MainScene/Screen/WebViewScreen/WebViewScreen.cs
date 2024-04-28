using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class WebViewScreen : BaseScreen
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] RectTransform bodyRT;
    [SerializeField] RectTransform rectTransform;
    Rect rect;

    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
    }

    public void Open(string title, string url)
    {
        base.Open();


        Vector2 pos_TopLeftFromCenter = bodyRT.anchoredPosition + new Vector2(-bodyRT.rect.size.x, bodyRT.rect.size.y) / 2;
        Vector2 pos_TopLeftFromTopLeft = pos_TopLeftFromCenter + new Vector2(rectTransform.rect.size.x, -rectTransform.rect.size.y) / 2;
        pos_TopLeftFromTopLeft.y = -pos_TopLeftFromTopLeft.y;

        rect = new(
            pos_TopLeftFromTopLeft.x * Rate,
            pos_TopLeftFromTopLeft.y * Rate,
            bodyRT.rect.width * Rate,
            bodyRT.rect.height * Rate
        );

        // url = "https://www.google.co.jp/";
        WebView.OpenURL(url, rect);

        titleText.text = title;
    }

    void OnGUI()
    {
        if (!Application.isEditor) return;

        //(new Rect(左上のｘ座標, 左上のｙ座標, 横幅, 縦幅), "テキスト", スタイル)
        GUI.Box(rect, "web view");
    }

    public override UniTask Close()
    {
        WebView.Close();
        base.Close();
        return UniTask.DelayFrame(0);
    }
}
