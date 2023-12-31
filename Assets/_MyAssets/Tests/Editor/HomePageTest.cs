using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Linq;
using System;
using DataBase;


public class HomePageTest
{


    [Test]
    public static void Test()
    {
        List<TestData> testDatas = new();
        int openHour = 20;
        int closeHour = 0;

        testDatas.Add(new()
        {
            now = new DateTime(2023, 12, 1).AddHours(openHour - 1).AddMinutes(30),
            answer = false,
        });
        testDatas.Add(new()
        {
            now = new DateTime(2023, 12, 1).AddHours(openHour),
            answer = true,
        });
        testDatas.Add(new()
        {
            now = new DateTime(2023, 12, 1).AddHours(openHour).AddMinutes(30),
            answer = true,
        });
        testDatas.Add(new()
        {
            now = new DateTime(2023, 12, 1).AddHours(closeHour - 1).AddMinutes(30),
            answer = true,
        });
        testDatas.Add(new()
        {
            now = new DateTime(2023, 12, 1).AddHours(closeHour),
            answer = false,
        });
        testDatas.Add(new()
        {
            now = new DateTime(2023, 12, 1).AddHours(closeHour).AddMinutes(30),
            answer = false,
        });


        List<bool> results = new();

        foreach (var testData in testDatas)
        {
            var a = HomePage.IsOpenTomorrowHoroscope(
                now: testData.now,
                openHour: openHour,
                closeHour: closeHour
            );
            var success = a == testData.answer;
            results.Add(success);
            if (success)
            {
                //  Debug.Log("成功 " + testData.name);
            }
            else
            {
                Debug.LogError("失敗");
                DebugUtils.LogJson(testData);
            }
        }
        Assert.That(results.All(success => success));
    }

    class TestData
    {
        public DateTime now;
        public bool answer;
    }

}
