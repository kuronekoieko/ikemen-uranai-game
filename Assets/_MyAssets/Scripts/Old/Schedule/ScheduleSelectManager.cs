using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ScheduleSelectManager : ObjectPooling<RowElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void ShowElement(List<Schedule[]> scheduleMatrix, Uranaishi uranaishi)
    {
        base.Clear();
        for (int i = 0; i < scheduleMatrix.Count; i++)
        {
            var element = base.GetInstance();
            // element.Show(histories[i]);
            element.Show(scheduleMatrix[i], uranaishi);
        }
    }

    public void AddElement(History[] histories)
    {
        for (int i = 0; i < histories.Length; i++)
        {
            var element = base.GetInstance();
            // element.Show(histories[i]);
        }
    }
}
