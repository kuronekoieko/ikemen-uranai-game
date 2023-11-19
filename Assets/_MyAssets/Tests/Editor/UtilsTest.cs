using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System;

public class UtilsTest
{
    [Test]
    public void OrderForKuchikomi_0()
    {


        // Review[] reviews = Utils.OrderForKuchikomi();


        LogAssert.Expect(LogType.Error, "日付のパース失敗 " + "n/a");
        // Assert.That(serializableDateTime.IsFutureFromNow() == false);
    }
}
