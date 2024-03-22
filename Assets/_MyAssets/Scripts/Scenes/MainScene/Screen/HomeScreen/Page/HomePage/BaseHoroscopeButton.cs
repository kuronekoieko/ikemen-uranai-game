using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using SaveDataObjects;


public abstract class BaseHoroscopeButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    protected string Key => DateTime.Today.ToDateKey();

    public virtual void OnStart()
    {
        button.AddListener(OnClick);
    }

    public virtual void OnOpen()
    {

    }

    protected async UniTask<HoroscopeHistory> GetHoroscopeHistory()
    {
        await UniTask.WaitUntil(() => SaveDataManager.SaveData != null);
        await UniTask.WaitUntil(() => SaveDataManager.SaveData.horoscopeHistories != null);

        SaveDataManager.SaveData.horoscopeHistories.TryGetValue(Key, out HoroscopeHistory horoscopeHistory);
        return horoscopeHistory;
    }

    protected void Anim()
    {
        transform
            .DOPunchScale(Vector3.one * 0.1f, 2f, 3, 0.1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .OnKill(() =>
            {
                transform.localScale = Vector3.one;
            });
    }

    protected abstract UniTask OnClick();

}
