using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Linq;
using System;
using DataBase;

public class HoroscopeScreenTest
{
    [Test]
    public async void ConsumePoints()
    {
        await CSVManager.InitializeAsync();

        List<DateTime> testDatas = CSVManager.Constellations.Select(c => c.StartDT.Value).ToList();
        testDatas.AddRange(CSVManager.Constellations.Select(c => c.EndDT.Value).ToList());

        List<bool> results = new();
        SaveData saveData = new();

        foreach (var testData in CSVManager.Constellations)
        {
            // Debug.Log("==================================");
            var constellation = saveData.GetConstellation(testData.StartDT);
            bool success = constellation != null && constellation.name == testData.name;
            results.Add(success);
            if (success)
            {
                //  Debug.Log("成功 " + testData.name);
            }
            else
            {
                Debug.LogError("失敗" + testData.name);
            }

            //  Debug.Log("==================================");

            constellation = saveData.GetConstellation(testData.EndDT);
            success = constellation != null && constellation.name == testData.name;
            results.Add(success);
            if (success)
            {
                // Debug.Log("成功 " + testData.name);
            }
            else
            {
                Debug.LogError("失敗" + testData.name);
            }
        }

        Assert.That(results.All(success => success));


    }

}
