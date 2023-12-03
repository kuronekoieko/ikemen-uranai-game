using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;
using System.Linq;

[System.Serializable]
public class Uranaishi
{
    public string id;
    public string name;
    public UranaishiStatus status;
    public int callChargePerMin;
    [TextArea(5, 10)] public string message;
    public string belonging;
    public string twitterURL;
    public string facebookURL;
    public string otherURL;
    public string[] expertises = new string[0];
    public string[] divinations = new string[0];
    public List<DailySchedule> dailySchedules = new List<DailySchedule>();
    public List<Review> reviews = new List<Review>();

    [System.NonSerialized] Sprite _iconSprite;

    public void CheckScheduleMatrix()
    {
        double span = Constant.Instance.reserveDurationMin;
        int days = Constant.Instance.reserveDays;

        DateTime init = DateTime.Today.AddHours(9);
        DateTime last = DateTime.Today.AddDays(1).AddHours(5);

        // 0:00-5:00 前日扱い
        // 5:00-9:00 前日扱い
        // 9:00-0:00 その日扱い
        if (DateTime.Now < init)
        {
            init = init.AddDays(-1);
            last = last.AddDays(-1);
        }

        // 古いスケジュールを削除
        // 念のため、予約可能日を超えた時間も削除
        dailySchedules.RemoveAll(d => d.schedules[0].startSDT.dateTime < init
            || d.schedules[0].startSDT.dateTime > last.AddDays(days));


        for (int j = dailySchedules.Count; j < days; j++)
        {
            DailySchedule dailySchedule = new DailySchedule();

            for (DateTime i = init; i < last; i = i.AddMinutes(span))
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = i.AddDays(j);
                dailySchedule.schedules.Add(schedule);
                // Debug.Log(i + " " + schedule.startSDT.dateTime);
            }
            dailySchedules.Add(dailySchedule);

        }

    }

    public DailySchedule GetDailyScheduleIncludes(DateTime dateTime)
    {
        DailySchedule dailySchedule = dailySchedules
            .Where(d => d.schedules[0].startSDT.dateTime <= dateTime)
            .Where(d => dateTime <= d.schedules.LastOrDefault().startSDT.dateTime.Value.AddMinutes(Constant.Instance.reserveDurationMin))
            .FirstOrDefault();
        return dailySchedule;
    }


    public async void GetIcon(UnityAction<Sprite> onComplete)
    {
        if (_iconSprite)
        {
            onComplete(_iconSprite);
            return;
        }

        if (UIManager.i.isLocalTestData)
        {

            // return;
        }

        await FirebaseStorageManager.i.DownloadFile(this, (sprite) =>
        {
            _iconSprite = sprite;
            onComplete(sprite);
        });
    }

    public string GetStatusDisplayName()
    {
        switch (status)
        {
            case UranaishiStatus.Counseling:
                return "相談中(X人待ち)";
            case UranaishiStatus.Waiting:
                return "今すぐOK";
            case UranaishiStatus.Closed:
                return "本日終了";
            case UranaishiStatus.DatTime:
                DailySchedule dailySchedule = GetDailyScheduleIncludes(DateTime.Now);
                Schedule schedule = dailySchedule.schedules
                    .Where(schedule => schedule.startSDT.IsFutureFromNow())
                    .OrderBy(schedule => schedule.startSDT.dateTime)
                    .FirstOrDefault();
                if (schedule == null) return "本日終了";
                return schedule.startSDT.dateTime.ToStringIncludeEmpty("M/dd HH:mm～");
            default:
                return "";
        }

    }

    public float GetReviewAvr()
    {
        double avr;
        if (reviews.Count() == 0)
        {
            avr = 0;
        }
        else
        {
            avr = reviews.Select(r => r.starCount).Average();
        }
        return (float)avr;
    }

    public Review[] GetOrderedReviews()
    {
        var pickUpReviews = reviews
            .Where(review => review.isPickUp)
            .OrderBy(review => review.writtenDate.dateTime)
            .ToArray();

        var notPickUpReviews = reviews
            .Where(review => !review.isPickUp)
            .OrderBy(review => review.writtenDate.dateTime)
            .ToArray();

        return pickUpReviews.Concat(notPickUpReviews).ToArray();
    }

    public Review GetFirstReview()
    {
        Review[] reviews = GetOrderedReviews();
        if (reviews.Length > 0)
        {
            return reviews[0];
        }
        else
        {
            return null;
        }
    }

    public string GetChareText()
    {
        return callChargePerMin + "pt/1分"; ;
    }

}

[System.Serializable]
public enum UranaishiStatus
{
    Counseling = 0,
    Waiting = 1,
    Closed = 2,
    DatTime = 3
}

[System.Serializable]
public class Schedule
{
    public SerializableDateTime startSDT = new SerializableDateTime();
    public DateTime endDT => startSDT.dateTime.Value.AddMinutes(Constant.Instance.reserveDurationMin);
    public ScheduleStatus scheduleStatus = ScheduleStatus.Free;
    //public string clientName;
}
[System.Serializable]
public class DailySchedule
{
    public List<Schedule> schedules;
    public DailySchedule()
    {
        schedules = new List<Schedule>();
    }
}


[System.Serializable]
public enum ScheduleStatus
{
    Free = 0,
    Reserved = 1,
    Closed = 2,
}


[System.Serializable]
public class SerializableDateTime
{
    [Header("yyyy/MM/dd HH:mm:ss")]
    [SerializeField] string dateTimeStr = "n/a";
    Nullable<DateTime> _dateTime;

    public string GetStr()
    {
        return dateTimeStr;
    }

    public Nullable<DateTime> dateTime
    {
        set
        {
            _dateTime = value;
            dateTimeStr = _dateTime.ToStringIncludeEmpty();
        }
        get
        {
            if (_dateTime != null)
            {
                return _dateTime;
            }

            if (DateTime.TryParse(dateTimeStr, out DateTime a))
            {
                _dateTime = a;
                return _dateTime;
            }
            else
            {
                Debug.LogWarning("日付のパース失敗 " + dateTimeStr);
                return null;
            }
        }

    }

    public bool IsFutureFromNow()
    {
        return dateTime > DateTime.Now;
    }
}

[System.Serializable]
public class Review
{
    [NonSerialized] public Uranaishi uranaishi;
    public int starCount;
    [TextArea(5, 10)] public string text;
    public string reviewerName;
    public int age;
    public Sex sex;
    public bool isPickUp;
    public SerializableDateTime writtenDate;

    public Review(
        int starCount,
        string text)
    {
        this.starCount = starCount;
        this.text = text;
        writtenDate = new SerializableDateTime();
        writtenDate.dateTime = DateTime.Now;
        sex = Sex.None;
    }

    public string GetTitleText()
    {
        string title = "";
        title += string.IsNullOrEmpty(reviewerName) ? "匿名" : reviewerName;
        title += age == 0 ? "" : "・" + age + "代";

        switch (sex)
        {
            case Sex.Man:
                title += "・男性";
                break;
            case Sex.Woman:
                title += "・女性";
                break;
            case Sex.None:

                break;
            default:
                break;
        }
        return title;

    }
}

[System.Serializable]
public enum Sex
{
    Man,
    Woman,
    None,
}
