using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionScreen : BaseScreen
{

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] RectTransform bodyRT;
    [SerializeField] RectTransform dummyRT;
    [SerializeField] RectTransform rectTransform;

    public override void OnStart()
    {
        base.OnStart();
    }

    public void Open(string title, string url)
    {
        base.Open();

        // Debug.Log(bodyRT.sizeDelta);
        //Debug.Log(bodyRT.rect);
        //  Debug.Log(bodyRT.rect.position);
        //  Debug.Log(bodyRT.position);
        //Debug.Log(bodyRT.anchoredPosition);
        // Debug.Log(bodyRT.anchorMax);
        // Debug.Log(bodyRT.anchorMin);

        //Debug.Log(bodyRT.rect.center);

        //  Debug.Log(bodyRT.rect.yMin);
        // Debug.Log(bodyRT.rect.yMax);
        // Debug.Log(bodyRT.rect.xMin);
        // Debug.Log(bodyRT.rect.xMax);
        // Debug.Log(rectTransform.rect.height / 2 - bodyRT.rect.yMax);



        //  Debug.Log(bodyRT.anchoredPosition);
        // Debug.Log(rectTransform.rect.center);
        //Debug.Log(rectTransform.rect.min);

        // Vector2 pos_TopLeftFromCenter = bodyRT.anchoredPosition + bodyRT.rect.size / 2;
        Vector2 pos_TopLeftFromCenter = bodyRT.anchoredPosition + new Vector2(-bodyRT.rect.size.x, bodyRT.rect.size.y) / 2;

        Debug.Log(pos_TopLeftFromCenter);

        Vector2 pos_TopLeftFromTopLeft = pos_TopLeftFromCenter + new Vector2(rectTransform.rect.size.x, -rectTransform.rect.size.y) / 2;
        Debug.Log(pos_TopLeftFromTopLeft);
        pos_TopLeftFromTopLeft.y = -pos_TopLeftFromTopLeft.y;

        // Vector2 bodyPos_BottomLeftFromCenter = bodyRT.anchoredPosition - bodyRT.rect.size / 2;

        //Debug.Log(bodyPos_BottomLeftFromCenter);

        // Vector2 pos_BottomLeftFromBottomLeftFrom = bodyPos_BottomLeftFromCenter + rectTransform.rect.size / 2;
        // Debug.Log(pos_BottomLeftFromBottomLeftFrom);

        // Debug.Log(poFromScreensBottomLeft);
        // yが398になればOK

        // Rect
        // https://docs.unity.cn/ja/2021.3/ScriptReference/Rect.html
        // xMin、yMinは矩形(くけい)の中心からの左上の座標
        // rect.positionは、x,yと同じ


        Rect rect = new
(
    pos_TopLeftFromTopLeft.x * CanvasManager.Instance.Rate,
    pos_TopLeftFromTopLeft.y * CanvasManager.Instance.Rate,
    bodyRT.rect.width * CanvasManager.Instance.Rate,
    bodyRT.rect.height * CanvasManager.Instance.Rate
);

        // rect.x = await FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.test_x);
        // rect.y = await FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.test_y);
        // rect.width = await FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.test_w);
        // rect.height = await FirebaseRemoteConfigManager.GetInt(FirebaseRemoteConfigManager.Key.test_h);
        Debug.Log(rect.ToRectInt());


        // WebView.OpenURL(URLs.Account, rect.ToRectInt());
        url = "https://www.google.co.jp/";
        WebView.OpenURL(url, rect.ToRectInt());

        titleText.text = title;
        // descriptionText.text = description;

        // dummyRT.size = new Vector2(rect.width, rect.height);
        // dummyRT.anchoredPosition = new Vector3(rect.x, rect.y);
    }

    public override void Close()
    {
        WebView.Close();
        base.Close();
    }
}
