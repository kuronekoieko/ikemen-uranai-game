using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ScheduleModal : BaseModal
{
    [SerializeField] ScheduleSelectManager scheduleSelectManager;
    [SerializeField] TextMeshProUGUI monthText;
    [SerializeField] TextMeshProUGUI[] dayTexts;

    public override void OnStart()
    {
        base.OnStart();
        scheduleSelectManager.OnStart();
    }

    public void Open(Uranaishi uranaishi)
    {
        base.OpenAnim(ModalType.Horizontal);

        //  Debug.Log(uranaishi.reserves.);

        monthText.text = DateTime.Today.ToString("M月");

        for (int i = 0; i < dayTexts.Length; i++)
        {
            dayTexts[i].text = DateTime.Today.AddDays(i).ToString("dd\n(ddd)");
        }

        /* List<DateTime[]> dateTimeMatrix = new List<DateTime[]>();

                double span = Constant.Instance.reserveDurationMin;
                DateTime init = DateTime.Today.AddHours(9);
                DateTime last = DateTime.Today.AddDays(1).AddHours(5);


                for (DateTime i = init; i < last; i = i.AddMinutes(span))
                {
                    // Debug.Log(i);
                    DateTime[] dateTimes = new DateTime[3];
                    for (int j = 0; j < dateTimes.Length; j++)
                    {
                        dateTimes[j] = i.AddDays(j);
                    }
                    dateTimeMatrix.Add(dateTimes);
                }*/

        int column = 3;


        List<Schedule[]> scheduleMatrix = new List<Schedule[]>();

        for (int j = 0; j < uranaishi.dailySchedules[0].schedules.Count; j++)
        {
            Schedule[] schedulesOfSameTime = new Schedule[column];

            for (int i = 0; i < schedulesOfSameTime.Length; i++)
            {
                schedulesOfSameTime[i] = uranaishi.dailySchedules[i].schedules[j];

            }
            scheduleMatrix.Add(schedulesOfSameTime);
        }




        scheduleSelectManager.ShowElement(scheduleMatrix, uranaishi);

    }
}
