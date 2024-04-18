using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Cysharp.Threading.Tasks;

public class ScreenManager : MonoBehaviour
{
    BaseScreen[] baseScreens;
    public static ScreenManager Instance;

    public void OnStart()
    {
        Instance = this;
        StartScreens();

        // 画面の開閉の間の1フレームで、open判定になるため
        this.ObserveEveryValueChanged(isOpenHome => IsOpenHome())
        .ThrottleFrame(5)
        .Where(_ => IsOpenHome())
        .Subscribe(isOpenHome =>
        {
            AudioManager.Instance.Play(AudioID.Home);
        })
        .AddTo(gameObject);
    }

    void StartScreens()
    {
        baseScreens = GetComponentsInChildren<BaseScreen>(true);
        foreach (var baseModal in baseScreens)
        {
            baseModal.OnStart();
        }
    }

    public T Get<T>() where T : BaseScreen
    {
        T subClass = baseScreens.Select(_ => _ as T).FirstOrDefault(_ => _);
        return subClass;
    }

    bool IsOpenHome()
    {
        if (baseScreens == null) return false;
        foreach (var baseScreen in baseScreens)
        {
            if (baseScreen.gameObject.activeSelf) return false;
        }
        return true;
    }
}