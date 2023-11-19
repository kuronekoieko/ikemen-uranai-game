using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEngine.TestTools;
using System;

public class UranaishiTest
{
    /* /// <summary>
        /// スケジュールが元々0個の場合
        /// </summary>
        [Test]
        public void CheckSchedules_0()
        {
            Uranaishi uranaishi = new Uranaishi();
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            uranaishi.CheckSchedules(reserveDurationMin, days);
            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == ScheduleStatus.Free);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));
        }

        /// <summary>
        /// スケジュールが10日前からのしか入っていなくて、全部予約済みの場合
        /// </summary>
        [Test]
        public void CheckSchedules_1()
        {
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            Uranaishi uranaishi = new Uranaishi();

            DateTime startDT = DateTime.Today.AddDays(-10);

            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < sumCount; i++)
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = startDT;
                schedule.scheduleStatus = ScheduleStatus.Reserved;
                startDT = startDT.AddMinutes(reserveDurationMin);
                schedules.Add(schedule);
            }
            uranaishi.schedules = schedules;

            uranaishi.CheckSchedules(reserveDurationMin, days);


            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == ScheduleStatus.Free);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));

        }

        /// <summary>
        /// スケジュールが10日後の場合
        /// </summary>
        [Test]
        public void CheckSchedules_2()
        {
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            Uranaishi uranaishi = new Uranaishi();

            DateTime startDT = DateTime.Today.AddDays(10);

            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < sumCount; i++)
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = startDT;
                schedule.scheduleStatus = ScheduleStatus.Reserved;
                startDT = startDT.AddMinutes(reserveDurationMin);
                schedules.Add(schedule);
            }
            uranaishi.schedules = schedules;


            uranaishi.CheckSchedules(reserveDurationMin, days);


            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == ScheduleStatus.Free);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));

        }

        /// <summary>
        /// 一日前から3日間のスケジュールがある場合
        /// </summary>
        [Test]
        public void CheckSchedules_3()
        {
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            Uranaishi uranaishi = new Uranaishi();

            DateTime startDT = DateTime.Today.AddDays(-1);

            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < sumCount; i++)
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = startDT;
                schedule.scheduleStatus = ScheduleStatus.Free;
                startDT = startDT.AddMinutes(reserveDurationMin);
                schedules.Add(schedule);
            }
            uranaishi.schedules = schedules;


            uranaishi.CheckSchedules(reserveDurationMin, days);


            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == ScheduleStatus.Free);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));

        }

        /// <summary>
        /// 前日から3日間がすべて予約済みになってる場合
        /// </summary>
        [Test]
        public void CheckSchedules_4()
        {
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            Uranaishi uranaishi = new Uranaishi();

            DateTime startDT = DateTime.Today.AddDays(-1);

            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < sumCount; i++)
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = startDT;
                schedule.scheduleStatus = ScheduleStatus.Reserved;
                startDT = startDT.AddMinutes(reserveDurationMin);
                schedules.Add(schedule);
            }
            uranaishi.schedules = schedules;


            uranaishi.CheckSchedules(reserveDurationMin, days);


            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                bool reserved = uranaishi.schedules[i].startSDT.dateTime < DateTime.Today.AddDays(3);
                ScheduleStatus scheduleStatus = reserved ? ScheduleStatus.Reserved : ScheduleStatus.Free;
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == scheduleStatus);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));

        }

        /// <summary>
        /// 今日から4日間がすべて予約済みの場合
        /// </summary>
        [Test]
        public void CheckSchedules_5()
        {
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            Uranaishi uranaishi = new Uranaishi();

            DateTime startDT = DateTime.Today;

            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < sumCount; i++)
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = startDT;
                schedule.scheduleStatus = ScheduleStatus.Reserved;
                startDT = startDT.AddMinutes(reserveDurationMin);
                schedules.Add(schedule);
            }
            uranaishi.schedules = schedules;

            uranaishi.CheckSchedules(reserveDurationMin, days);


            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == ScheduleStatus.Reserved);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));

        }

        /// <summary>
        /// datetimeにnullが入っている場合
        /// </summary>
        [Test]
        public void CheckSchedules_6()
        {
            int reserveDurationMin = Constant.Instance.reserveDurationMin;
            int days = Constant.Instance.reserveDays;
            int sumCount = 60 / reserveDurationMin * 24 * days;

            Uranaishi uranaishi = new Uranaishi();

            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < sumCount; i++)
            {
                Schedule schedule = new Schedule();
                schedule.startSDT.dateTime = null;
                schedule.scheduleStatus = ScheduleStatus.Reserved;
                schedules.Add(schedule);
            }
            uranaishi.schedules = schedules;

            LogAssert.ignoreFailingMessages = true;
            //LogAssert.Expect(LogType.Error, "日付のパース失敗 n/a");
            uranaishi.CheckSchedules(reserveDurationMin, days);
            //LogAssert.NoUnexpectedReceived();


            for (int i = 0; i < uranaishi.schedules.Count; i++)
            {
                Debug.Log(uranaishi.schedules[i].startSDT.dateTime + " " + uranaishi.schedules[i].scheduleStatus);
                Assert.That(uranaishi.schedules[i].startSDT.dateTime == DateTime.Today.AddMinutes(reserveDurationMin * i));
                Assert.That(uranaishi.schedules[i].scheduleStatus == ScheduleStatus.Free);
            }
            Assert.That(uranaishi.schedules.Count == sumCount);
            Assert.That(uranaishi.schedules[uranaishi.schedules.Count - 1].startSDT.dateTime == DateTime.Today.AddDays(days).AddMinutes(-reserveDurationMin));

        }*/


}

public class SerializableDateTimeTest
{
    [Test]
    public void DateTime_0()
    {
        var serializableDateTime = new SerializableDateTime();
        LogAssert.Expect(LogType.Error, "日付のパース失敗 " + "n/a");
        Assert.That(serializableDateTime.dateTime == null);
        Assert.That(serializableDateTime.GetStr() == "n/a");
        LogAssert.Expect(LogType.Error, "日付のパース失敗 " + "n/a");
        Assert.That(serializableDateTime.IsFutureFromNow() == false);
    }

    [Test]
    public void DateTime_1()
    {
        var serializableDateTime = new SerializableDateTime();
        serializableDateTime.dateTime = DateTime.MinValue;
        Assert.That(serializableDateTime.dateTime == DateTime.MinValue);
        Assert.That(serializableDateTime.GetStr() == DateTime.MinValue.ToString());
        Assert.That(serializableDateTime.IsFutureFromNow() == false);
    }

    [Test]
    public void DateTime_2()
    {
        var serializableDateTime = new SerializableDateTime();
        var test = new DateTime();
        serializableDateTime.dateTime = test;
        Assert.That(serializableDateTime.dateTime == test);
        Assert.That(serializableDateTime.GetStr() == test.ToString());
        Assert.That(serializableDateTime.IsFutureFromNow() == false);
    }

    [Test]
    public void DateTime_3()
    {
        var serializableDateTime = new SerializableDateTime();
        serializableDateTime.dateTime = null;
        LogAssert.Expect(LogType.Error, "日付のパース失敗 " + "n/a");
        Assert.That(serializableDateTime.dateTime == null);
        Assert.That(serializableDateTime.GetStr() == "n/a");
        LogAssert.Expect(LogType.Error, "日付のパース失敗 " + "n/a");
        Assert.That(serializableDateTime.IsFutureFromNow() == false);
    }

    [Test]
    public void DateTime_4()
    {
        var serializableDateTime = new SerializableDateTime();
        serializableDateTime.dateTime = DateTime.Now;
        Assert.That(serializableDateTime.dateTime == DateTime.Now);
        Assert.That(serializableDateTime.GetStr() == DateTime.Now.ToString());
        Assert.That(serializableDateTime.IsFutureFromNow() == false);
    }

    [Test]
    public void DateTime_5()
    {
        var serializableDateTime = new SerializableDateTime();
        serializableDateTime.dateTime = DateTime.MaxValue;
        Assert.That(serializableDateTime.dateTime == DateTime.MaxValue);
        Assert.That(serializableDateTime.GetStr() == DateTime.MaxValue.ToString());
        Assert.That(serializableDateTime.IsFutureFromNow() == true);
    }

    [Test]
    public void ToStringIncludeEmpty_0()
    {
        DateTime? dateTime = null;
        Assert.That(dateTime.ToStringIncludeEmpty() == "n/a");
    }

    [Test]
    public void ToStringIncludeEmpty_1()
    {
        DateTime newDT = new DateTime();
        DateTime? dateTime = newDT;
        Assert.That(dateTime.ToStringIncludeEmpty() == newDT.ToString());
    }

    [Test]
    public void ToStringIncludeEmpty_2()
    {
        DateTime newDT = DateTime.Now;
        DateTime? dateTime = newDT;
        Assert.That(dateTime.ToStringIncludeEmpty() == newDT.ToString());
    }

    [Test]
    public void ToStringIncludeEmpty_3()
    {
        DateTime? dateTime = null;
        Assert.That(dateTime.ToStringIncludeEmpty("M月d日(ddd)") == "n/a");
    }

    [Test]
    public void ToStringIncludeEmpty_4()
    {
        DateTime newDT = new DateTime();
        DateTime? dateTime = newDT;
        Assert.That(dateTime.ToStringIncludeEmpty("M月d日(ddd)") == newDT.ToString("M月d日(ddd)"));
    }

    [Test]
    public void ToStringIncludeEmpty_5()
    {
        DateTime newDT = DateTime.Now;
        DateTime? dateTime = newDT;
        Assert.That(dateTime.ToStringIncludeEmpty("M月d日(ddd)") == newDT.ToString("M月d日(ddd)"));
    }
}


public class ReviewTest
{

    [Test]
    public void GetTitleText_0()
    {
        Review review = new Review(1, "");

        string title = review.GetTitleText();
        // Debug.Log(review.reviewerName);
        Assert.That(title == "匿名");
    }

    [Test]
    public void GetTitleText_1()
    {
        Review review = new Review(1, "");
        review.reviewerName = "よしこ";
        review.age = 50;
        review.sex = Sex.Woman;

        string title = review.GetTitleText();
        Assert.That(title == "よしこ・50代・女性");
    }

    [Test]
    public void GetTitleText_2()
    {
        Review review = new Review(1, "");
        review.reviewerName = "";
        review.age = 20;
        review.sex = Sex.Man;

        string title = review.GetTitleText();
        Assert.That(title == "匿名・20代・男性");
    }

    [Test]
    public void GetTitleText_3()
    {
        Review review = new Review(1, "");
        review.reviewerName = "よしお";
        review.age = 0;
        review.sex = Sex.Man;

        string title = review.GetTitleText();
        Assert.That(title == "よしお・男性");
    }


    [Test]
    public void GetTitleText_4()
    {
        Review review = new Review(1, "");
        review.reviewerName = "よしお";
        review.age = 20;
        review.sex = Sex.None;

        string title = review.GetTitleText();
        Assert.That(title == "よしお・20代");
    }

    [Test]
    public void GetTitleText_5()
    {
        Review review = new Review(1, "");
        review.reviewerName = "";
        review.age = 0;
        review.sex = Sex.Man;

        string title = review.GetTitleText();
        Assert.That(title == "匿名・男性");
    }

    [Test]
    public void GetTitleText_6()
    {
        Review review = new Review(1, "");
        review.reviewerName = "よしお";
        review.age = 0;
        review.sex = Sex.None;

        string title = review.GetTitleText();
        Assert.That(title == "よしお");
    }

    [Test]
    public void GetTitleText_7()
    {
        Review review = new Review(1, "");
        review.reviewerName = "";
        review.age = 20;
        review.sex = Sex.None;

        string title = review.GetTitleText();
        Assert.That(title == "匿名・20代");
    }
}
