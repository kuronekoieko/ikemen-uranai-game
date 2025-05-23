using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class LoadingScreen : BaseScreen
{
    [SerializeField] Image progressBarImage;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] TextMeshProUGUI hintTitleText;
    [SerializeField] TextMeshProUGUI hintContentText;
    [SerializeField] TextMeshProUGUI uidText;


    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
    }

    public override void Open()
    {
        base.Open();

        SetProgress(0);

        // テキストサイズはauto sizeで

        var hint = CSVManager.Hints.GetRandom();
        if (hint == null)
        {
            //hintTitleText.text = "カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法カードのレベルアップ方法";
            hintTitleText.text = "カードのレベルアップ方法";
            // hintTitleText.LimitLineCount(lineCountMax: 1, fontSizeMin: 40);
            // hintContentText.text = "カードを重複して手に入れるか、\n成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。成長の鍵を手に入れることでレベルを上げることができます。";
            hintContentText.text = "カードを重複して手に入れるか、\n成長の鍵を手に入れることでレベルを上げることができます。";
            // hintContentText.LimitLineCount(lineCountMax: 3, fontSizeMin: 30);
            uidText.text = "UserID:  123456";
        }
        else
        {
            hintTitleText.text = hint.title;
            // hintTitleText.LimitLineCount(lineCountMax: 1, fontSizeMin: 40);
            hintContentText.text = hint.description;
            // hintContentText.LimitLineCount(lineCountMax: 3, fontSizeMin: 30);
            uidText.text = "UserID:  " + SaveDataManager.SaveData.userId;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="progress">0-1</param>
    public void SetProgress(float progress)
    {
        progressBarImage.fillAmount = Mathf.Clamp01(progress);
        progressText.text = (progressBarImage.fillAmount * 100).ToString("F1") + "%";
    }

    public async UniTask ProgressTimer(float timeOut)
    {
        float frameCount = 0;
        while (true)
        {
            float time = Time.fixedDeltaTime * frameCount;
            float progress = time / timeOut;// テスト3秒
            if (progress > 1f) break;
            SetProgress(progress);
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
            frameCount += 1f;
        }
    }


    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }
}
