using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{

    [SerializeField] ToggleGroup bannerToggleGroup;
    [SerializeField] Transform pagesTf;
    [SerializeField] Transform modalsTf;
    [SerializeField] HeaderManager headerManager;
    [SerializeField] UranaishiTestData uranaishiTestData;
    [SerializeField] LikePopup likePopup;
    public Uranaishi[] uranaishiAry { get; set; }
    public static UIManager i;
    public int activePage { get; set; } = 0;
    public bool isLocalTestData { get; } = false;
    BaseModal[] baseModals;
    BannerToggleController[] bannerToggleControllers;

    async void Start()
    {
        i = this;
        SaveDataManager.i.LoadSaveData();
        FirebaseDatabaseManager.Instance.Initialize();
        FirebaseStorageManager.Instance.Initialize();

        if (isLocalTestData)
        {
            uranaishiAry = uranaishiTestData.uranaishis;
        }
        else
        {
            Debug.Log("ユーザーデータ取得開始");
            uranaishiAry = await FirebaseDatabaseManager.Instance.GetUranaishiAry(10);
            Debug.Log("ユーザーデータ取得終了");
        }

        StartPages();
        StartModals();
        StartBannerToggles();
        headerManager.OnStart();
        likePopup.OnStart();
        CommonPopup.i.OnStart();

        Application.targetFrameRate = 60;
    }

    void StartPages()
    {
        BasePageManager[] basePageManagers = pagesTf.GetComponentsInChildren<BasePageManager>(true);
        foreach (var basePageManager in basePageManagers)
        {
            basePageManager.OnStart();
        }
    }

    void StartModals()
    {
        baseModals = modalsTf.GetComponentsInChildren<BaseModal>(true);
        foreach (var baseModal in baseModals)
        {
            baseModal.OnStart();
        }
    }


    void StartBannerToggles()
    {
        bannerToggleControllers = bannerToggleGroup.GetComponentsInChildren<BannerToggleController>(true);

        foreach (var bannerToggleController in bannerToggleControllers)
        {
            bannerToggleController.OnStart(activePage);
        }
    }

    public void CloseAllModals(int transPage)
    {
        foreach (var baseModal in baseModals)
        {
            baseModal.Clear();
        }

        foreach (var bannerToggleController in bannerToggleControllers)
        {
            if (bannerToggleController.page == transPage)
            {
                bannerToggleController.toggle.isOn = true;
            }
        }
    }

    public T GetModal<T>() where T : BaseModal
    {
        T subClass = baseModals.Select(_ => _ as T).FirstOrDefault(_ => _);
        // Debug.Log(subClass.GetType());
        return subClass;
    }
}
