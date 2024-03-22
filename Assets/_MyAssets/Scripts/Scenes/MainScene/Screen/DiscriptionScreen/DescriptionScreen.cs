using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Codice.CM.SEIDInfo;

public class DescriptionScreen : BaseScreen
{

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] RectTransform bodyRT;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GUIStyle dummyTextStyle;
    Rect rect;

    public override void OnStart()
    {
        base.OnStart();
    }

    public void Open(string title, string url)
    {
        base.Open();


        Vector2 pos_TopLeftFromCenter = bodyRT.anchoredPosition + new Vector2(-bodyRT.rect.size.x, bodyRT.rect.size.y) / 2;
        Vector2 pos_TopLeftFromTopLeft = pos_TopLeftFromCenter + new Vector2(rectTransform.rect.size.x, -rectTransform.rect.size.y) / 2;
        pos_TopLeftFromTopLeft.y = -pos_TopLeftFromTopLeft.y;

        rect = new(
            pos_TopLeftFromTopLeft.x * CanvasManager.Instance.Rate,
            pos_TopLeftFromTopLeft.y * CanvasManager.Instance.Rate,
            bodyRT.rect.width * CanvasManager.Instance.Rate,
            bodyRT.rect.height * CanvasManager.Instance.Rate
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

    public override void Close()
    {
        WebView.Close();
        base.Close();
    }
}
