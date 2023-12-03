using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class CallingScreen : BaseCallingScreen
{
    [SerializeField] GameObject makingCallObj;
    [SerializeField] GameObject callingObj;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI uranaishiNameText;
    [SerializeField] Button hangupButton;
    [SerializeField] TextMeshProUGUI leftTimeText;
    float passedTimeSec;
    bool isTimer;
    float maxSeconds;
    float usePointTimerSec;
    History history;

    public override void OnStart(CallingManager callingManager)
    {
        base.OnStart(callingManager);
    }

    public override void Open(Uranaishi uranaishi)
    {
        base.Open(uranaishi);
        uranaishiNameText.text = uranaishi.name;
        uranaishi.GetIcon((sprite) =>
        {
            iconImage.sprite = sprite;
        });
        hangupButton.onClick.AddListener(() =>
        {
            base.manager.GetScreen<ConfirmHangUpPopup>().Open(uranaishi);
        });
        makingCallObj.SetActive(true);
        callingObj.SetActive(false);

        // 仮
        DOVirtual.DelayedCall(3, Call);
    }


    private void Update()
    {
        if (!isTimer) return;

        if (passedTimeSec >= maxSeconds)
        {
            // 通話強制終了
            manager.GetScreen<InputReviewScreen>().Open(uranaishi);
            return;
        }


        var leftTimeSpan = new TimeSpan(0, 0, Mathf.CeilToInt(maxSeconds - passedTimeSec));
        var mmss = leftTimeSpan.ToString(@"mm\:ss");

        leftTimeText.text = $"残り 約{mmss}";
        passedTimeSec += Time.deltaTime;

        // nポイント消費するのに何秒いるか
        int usePointUnit = 3;
        float secPerPoint = 60f / (float)uranaishi.callChargePerMin * usePointUnit;
        if (usePointTimerSec > secPerPoint)
        {
            usePointTimerSec = 0;
            // Debug.Log("ポイント更新 " + secPerPoint);

            SaveData.i.ConsumePoints(usePointUnit);

            history.durationSec = Mathf.FloorToInt(passedTimeSec);
            SaveData.i.histories[SaveData.i.histories.Count - 1] = history;
            SaveDataManager.i.Save();
        }

        usePointTimerSec += Time.deltaTime;
        // Debug.Log(leftTimeText.text);

    }

    void Call()
    {
        makingCallObj.SetActive(false);
        callingObj.SetActive(true);
        StartTimer();
        history = new History(DateTime.Now, uranaishi.id);
        SaveData.i.histories.Add(history);
    }

    void StartTimer()
    {
        isTimer = true;
        passedTimeSec = 0;
        usePointTimerSec = 0;
        maxSeconds = (float)SaveData.i.GetSumPoint() / (float)uranaishi.callChargePerMin * 60f;
    }

    public override void Close()
    {
        base.Close();
    }

    public void StopTimer()
    {
        isTimer = false;
    }
}
