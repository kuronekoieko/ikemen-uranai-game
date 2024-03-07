using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    BasePopup[] basePopups;
    [SerializeField] CommonPopup commonPopupPrefab;
    List<CommonPopup> commonPopups = new();



    public void OnStart()
    {
        StartPopups();
        commonPopupPrefab.gameObject.SetActive(false);
        HideDummies();
    }


    void StartPopups()
    {
        basePopups = GetComponentsInChildren<BasePopup>(true);
        foreach (var basePopup in basePopups)
        {
            basePopup.OnStart();
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
