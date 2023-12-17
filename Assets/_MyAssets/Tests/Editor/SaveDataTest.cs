using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Linq;

public class SaveDataTest
{

    /*
        [Test]
        public void ConsumePoints()
        {
            SaveData[] tests = new SaveData[] {
                new SaveData(0,0),
                new SaveData(2000,2000),
                new SaveData(2000,5000),
                new SaveData(5000,2000),
                new SaveData(0,2000),
                new SaveData(2000,0),
                new SaveData(1000,0),
             };

            SaveData[] answers = new SaveData[] {
                new SaveData(0,0),
                new SaveData(0,2000),
                new SaveData(0,5000),
                new SaveData(3000,2000),
                new SaveData(0,0),
                new SaveData(0,0),
                new SaveData(0,0),
             };

            List<bool> results = new List<bool>();
            for (int i = 0; i < tests.Length; i++)
            {

                if (i == 0 || i == 5)
                {
                    LogAssert.Expect(LogType.Error, "ポイントが不足しているのに購入できてしまった");
                }


                tests[i].ConsumePoints(2000);
                bool a = tests[i].freePoint == answers[i].freePoint;
                bool b = tests[i].purchasedPoint == answers[i].purchasedPoint;

                if (a == true && b == true)
                {
                    // Debug.Log(i + " : " + tests[i].freePoint + " " + tests[i].purchasedPoint);

                }
                else
                {
                    Debug.LogError(i + " : " + tests[i].freePoint + " " + tests[i].purchasedPoint);
                }

                results.Add(a == true && b == true);
            }
            Assert.That(results.All(_ => true));
        }

    */
}
