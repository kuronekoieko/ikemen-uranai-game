using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyDatas : MonoBehaviour
{
    [SerializeField] UranaishiTestData uranaishiTestData;

    async void Start()
    {
        uranaishiTestData.uranaishis = await FirebaseDatabaseManager.i.GetUranaishiAry(10);

    }


}
