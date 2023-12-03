using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class UranaishiProfile_Schedule : BaseUranaishiProfile
{
    [SerializeField] TitleContentTexts titleContentTextOrigin;
    List<TitleContentTexts> titleContentTexts = new List<TitleContentTexts>();
    public override void OnStart()
    {
        titleContentTexts.Add(titleContentTextOrigin);
        for (int i = 0; i < 4; i++)
        {
            titleContentTexts.Add(Instantiate(titleContentTextOrigin, transform));
        }

    }

    public override void OnOpen(Uranaishi uranaishi)
    {
        for (int i = 0; i < titleContentTexts.Count; i++)
        {
            titleContentTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < uranaishi.dailySchedules.Count; i++)
        {
            var schedules = uranaishi.dailySchedules[i].schedules;

            var freeDateTimes = schedules
                .Where(schedule => schedule.startSDT.IsFutureFromNow())
                .Where(schedule => schedule.scheduleStatus == ScheduleStatus.Free)
                .OrderBy(schedule => schedule.startSDT.dateTime)
                .Select(schedule => schedule.startSDT.dateTime)
                .ToArray();


            Period period = null;
            List<Period> periods = new List<Period>();
            foreach (var startDT in freeDateTimes)
            {

                if (period == null || period.endDT != startDT)
                {
                    period = new Period(
                        startDT: startDT,
                        endDT: startDT.Value.AddMinutes(Constant.Instance.reserveDurationMin)
                    );
                    periods.Add(period);
                    continue;
                }

                period.endDT = period.endDT.Value.AddMinutes(Constant.Instance.reserveDurationMin);
            }


            string text = "";
            string dateText = "";
            foreach (var p in periods)
            {
                text += p.startDT.ToStringIncludeEmpty("HH:mm")
                + "～" + p.endDT.ToStringIncludeEmpty("HH:mm") + "\n";
                dateText = schedules[0].startSDT.dateTime.ToStringIncludeEmpty("M月d日(ddd)");
            }
            text = text.TrimEnd('\n');
            titleContentTexts[i].gameObject.SetActive(true);

            titleContentTexts[i].SetTexts(dateText, text);
        }

    }



    class Period
    {
        public DateTime? startDT;
        public DateTime? endDT;

        public Period(DateTime? startDT, DateTime? endDT)
        {
            this.startDT = startDT;
            this.endDT = endDT;
        }
    }




}

