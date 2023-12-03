using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class SelectDateTimeButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] GameObject availableObj;
    [SerializeField] GameObject notAvailableObj;

    Uranaishi uranaishi;
    DateTime dateTime;

    public void OnInstantiate()
    {
        button.onClick.AddListener(OnClickSelectButton);
    }

    public void OnOpen(Schedule schedule, Uranaishi uranaishi)
    {
        DateTime dateTime = schedule.startSDT.dateTime.Value;
        this.uranaishi = uranaishi;
        this.dateTime = dateTime;

        bool isFree = schedule.startSDT.dateTime != null
            && schedule.scheduleStatus == ScheduleStatus.Free
            && schedule.startSDT.IsFutureFromNow();

        availableObj.SetActive(isFree);
        notAvailableObj.SetActive(!isFree);
        button.enabled = isFree;
    }

    void OnClickSelectButton()
    {
        UIManager.i.GetModal<ReserveModal>().Open(dateTime, uranaishi);
    }
}
