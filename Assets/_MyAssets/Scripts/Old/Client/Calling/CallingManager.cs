using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CallingManager : MonoBehaviour
{
    BaseCallingScreen[] baseCallingScreens;
    [SerializeField] PurchasePointsPopup purchasePointsPopup;
    Uranaishi uranaishi;
    bool isEnoughPoint => SaveData.i.GetSumPoint() > uranaishi.callChargePerMin * 3;

    public void OnStart()
    {
        baseCallingScreens = GetComponentsInChildren<BaseCallingScreen>(true);
        foreach (var item in baseCallingScreens)
        {
            item.OnStart(this);
        }
        purchasePointsPopup.OnStart();
    }

    public void Open(Uranaishi uranaishi)
    {
        gameObject.SetActive(true);
        this.uranaishi = uranaishi;
        foreach (var item in baseCallingScreens)
        {
            item.Close();
        }

        //isEnoughPoint = true;
        if (isEnoughPoint)
        {
            GetScreen<ConfirmCallingPopup>().Open(uranaishi);
        }
        else
        {
            purchasePointsPopup.onReturnFromPurchase = OnReturnFromPurchase;
            purchasePointsPopup.Open(uranaishi, 3);
        }
    }

    void OnReturnFromPurchase()
    {
        //isEnoughPoint = true;
        if (isEnoughPoint)
        {
            purchasePointsPopup.Close();
            GetScreen<ConfirmCallingPopup>().Open(uranaishi);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        foreach (var item in baseCallingScreens)
        {
            item.Close();
        }
    }

    public T GetScreen<T>() where T : BaseCallingScreen
    {
        // https://techblog.kayac.com/unity_advent_calendar_2018_20
        // T subClass = baseCallingScreens.Select(_ => _ as T).FirstOrDefault(_ => _);
        foreach (var item in baseCallingScreens)
        {
            T subClass = item as T;
            if (subClass) return subClass;
        }
        // Debug.Log(subClass.GetType());
        return null;
    }
}
