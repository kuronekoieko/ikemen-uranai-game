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
    Tweener tweener;
    public virtual void OnStart()
    {
        button.AddListener(OnClick);
    }

    public virtual void OnOpen()
    {

    }

    protected HoroscopeHistory GetHoroscopeHistory()
    {
        if (SaveDataManager.SaveData == null) return null;
        if (SaveDataManager.SaveData.horoscopeHistories == null) return null;

        SaveDataManager.SaveData.horoscopeHistories.TryGetValue(Key, out HoroscopeHistory horoscopeHistory);
        return horoscopeHistory;
    }

    protected void Anim()
    {
        // アニメーション実行中に再度実行するとどんどんデカくなる→killで対応できる
        // ホームボタンを押すたびにアニメーション動くと変
        if (tweener != null && tweener.IsPlaying())
        {
            return;
        }

        Kill();
        tweener = transform
            .DOPunchScale(Vector3.one * 0.1f, 2f, 3, 0.1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .OnKill(() =>
            {
                transform.localScale = Vector3.one;
            });
    }

    protected void Kill()
    {
        // こっちだとスケールもどらないっぽい？
        // button.transform.DOKill(true);
        // transform.DOKill(true);
        tweener?.Kill(true);
        transform.localScale = Vector3.one;
    }

    protected abstract UniTask OnClick();

}
