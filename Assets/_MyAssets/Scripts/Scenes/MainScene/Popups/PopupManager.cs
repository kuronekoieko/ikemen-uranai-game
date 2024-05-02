using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    BasePopup[] basePopups;
    CommonPopup commonPopupPrefab;
    readonly List<CommonPopup> commonPopups = new();



    public async UniTask OnStart()
    {
        await StartPopupsAsync();
        commonPopupPrefab.gameObject.SetActive(false);
        HideDummies();
    }


    async UniTask StartPopupsAsync()
    {
        var prefabs = await AddressablesLoader.LoadAllAsync<GameObject>("Popups");
        foreach (var prefab in prefabs)
        {
            Instantiate(prefab, transform);
        }

        basePopups = GetComponentsInChildren<BasePopup>(true);
        foreach (var basePopup in basePopups)
        {
            basePopup.OnStart();
            // https://l-s-d.sakura.ne.jp/2016/04/check_derive_sub_class/
            if (!basePopup.GetType().IsSubclassOf(typeof(CommonPopup)))
            {
                if (basePopup.TryGetComponent(out CommonPopup commonPopup))
                {
                    this.commonPopupPrefab = commonPopup;
                }
            }
        }
    }

    public T GetPopup<T>() where T : BasePopup
    {
        T subClass = basePopups.Select(_ => _ as T).FirstOrDefault(_ => _);
        return subClass;
    }

    public T CreatePopup<T>() where T : BasePopup
    {
        T popup = Instantiate(GetPopup<T>(), transform);
        popup.OnStart();
        return popup;
    }



    void HideDummies()
    {
        CommonPopup[] dummies = GetComponentsInChildren<CommonPopup>();
        foreach (var dummy in dummies)
        {
            dummy.gameObject.SetActive(false);
        }
    }

    public void Clear()
    {
        foreach (var item in commonPopups)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ResetOrder()
    {
        // CommonPopupが増えるため
        Canvas[] canvases = GetComponentsInChildren<Canvas>(true);
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].sortingOrder = i;
        }
    }

    public CommonPopup GetCommonPopup()
    {
        CommonPopup instance = null;
        foreach (var item in commonPopups)
        {
            if (item.gameObject.activeSelf == false)
            {
                instance = item;
                break;
            }
        }
        if (instance == null)
        {
            instance = Instantiate(commonPopupPrefab, transform);
            instance.OnStart();
            commonPopups.Add(instance);
        }
        instance.gameObject.SetActive(true);
        return instance;
    }
}
