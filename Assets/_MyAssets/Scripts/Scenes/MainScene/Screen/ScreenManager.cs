using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Cysharp.Threading.Tasks;

public class ScreenManager : SingletonMonoBehaviour<ScreenManager>
{
    [SerializeField] Camera uiCamera;
    public Camera UICamera => uiCamera;
    [SerializeField] CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup => canvasGroup;

    BaseScreen[] baseScreens;
    HomeScreen homeScreen;


    public async UniTask OnStart()
    {
        await StartScreens();

        // 画面の開閉の間の1フレームで、open判定になるため
        this.ObserveEveryValueChanged(isOpenHome => IsOpenHome())
        .ThrottleFrame(5)
        .Where(_ => IsOpenHome())
        .Subscribe(isOpenHome =>
        {
            Get<HomeScreen>().Open();
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
            baseScreen.OnStart(uiCamera);
            if (baseScreen.TryGetComponent(out HomeScreen homeScreen))
            {
                this.homeScreen = homeScreen;
            }
        }

        homeScreen.Open();
    }

    public T Get<T>() where T : BaseScreen
    {
        T subClass = baseScreens.Select(_ => _ as T).FirstOrDefault(_ => _);
        return subClass;
    }

    bool IsOpenHome()
    {
        if (baseScreens == null) return false;
        if (homeScreen == null) return false;
        foreach (var baseScreen in baseScreens)
        {
            if (baseScreen == homeScreen) continue;
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