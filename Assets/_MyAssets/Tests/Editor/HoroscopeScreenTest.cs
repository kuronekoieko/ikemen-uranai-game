using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Linq;
using System;
using DataBase;

public class HoroscopeScreenTest : MonoBehaviour
{
    [Test]
    public async void ConsumePoints()
    {
        await CSVManager.Instance.InitializeAsync();

        List<DateTime> testDatas = CSVManager.Instance.Constellations.Select(c => c.StartDT).ToList();
        testDatas.AddRange(CSVManager.Instance.Constellations.Select(c => c.EndDT).ToList());

        List<bool> results = new();
        SaveData saveData = new();

        foreach (var testData in CSVManager.Instance.Constellations)
        {
            Debug.Log("==================================");
            var constellation = saveData.GetConstellation(testData.StartDT);
            bool success = constellation != null && constellation.name == testData.name;
            results.Add(success);
            if (success)
            {
                Debug.Log("成功 " + testData.name);
            }
            else
            {
                Debug.LogError("失敗" + testData.name);
            }

            Debug.Log("==================================");

            constellation = saveData.GetConstellation(testData.EndDT);
            success = constellation != null && constellation.name == testData.name;
            results.Add(success);
            if (success)
            {
                Debug.Log("成功 " + testData.name);
            }
            else
            {
                Debug.LogError("失敗" + testData.name);
            }
        }

        Assert.That(results.All(success => success));


    }

}
