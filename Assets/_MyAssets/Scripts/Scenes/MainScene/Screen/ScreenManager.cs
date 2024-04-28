using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Cysharp.Threading.Tasks;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] Camera uiCamera;

    BaseScreen[] baseScreens;
    public static ScreenManager Instance;

    public async UniTask OnStart()
    {
        Instance = this;
        await StartScreens();

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

    async UniTask StartScreens()
    {

        var baseScreenPrefabs = await AssetBundleLoader.LoadAllAsync<GameObject>("Screens");

        foreach (var baseScreenPrefab in baseScreenPrefabs)
        {
            Instantiate(baseScreenPrefab, transform);
        }

        baseScreens = GetComponentsInChildren<BaseScreen>(true);
        foreach (var baseScreen in baseScreens)
        {
            baseScreen.OnStart();
            baseScreen.SetCamera(uiCamera);
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

    public void ResetOrder()
    {
        var a = baseScreens.OrderBy(baseScreen => baseScreen.transform.GetSiblingIndex()).ToArray();
        for (int i = 0; i < a.Length; i++)
        {
            a[i].Canvas.sortingOrder = i;
        }
    }
}