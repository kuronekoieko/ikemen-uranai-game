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
        //Debug.Log(bodyRT.position);
        //Debug.Log(bodyRT.anchoredPosition);
        // Debug.Log(bodyRT.anchorMax);
        // Debug.Log(bodyRT.anchorMin);

        //Debug.Log(bodyRT.rect.center);

        //  Debug.Log(bodyRT.rect.yMin);
        // Debug.Log(bodyRT.rect.yMax);
        // Debug.Log(bodyRT.rect.xMin);
        // Debug.Log(bodyRT.rect.xMax);
        // Debug.Log(rectTransform.rect.height / 2 - bodyRT.rect.yMax);


        Debug.Log(bodyRT.rect.position);
        Debug.Log(rectTransform.rect.position);

        Vector2 vector2 = bodyRT.rect.position - rectTransform.rect.position;
        Debug.Log(vector2);

        // Vector2 bodyPos_BottomLeftFromCenter = bodyRT.anchoredPosition - bodyRT.rect.size / 2;

        // Debug.Log(bodyPos_BottomLeftFromCenter);

        // Vector2 pos_BottomLeftFromBottomLeftFrom = bodyPos_BottomLeftFromCenter + rectTransform.rect.size / 2;
        // Debug.Log(pos_BottomLeftFromBottomLeftFrom);

        // Debug.Log(poFromScreensBottomLeft);
        // yが248になればOK

        // Rect
        // https://docs.unity.cn/ja/2021.3/ScriptReference/Rect.html
        // xMin、yMinは矩形(くけい)の中心からの左上の座標
        // rect.positionは、x,yと同じ


        Rect rect = new
        (
            vector2.x * CanvasManager.Instance.Rate,
            vector2.y * CanvasManager.Instance.Rate,
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
