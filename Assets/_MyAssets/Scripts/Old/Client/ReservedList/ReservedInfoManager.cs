using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservedInfoManager : ObjectPooling<ReservedInfoElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void ShowElement(Reserve[] reserves)
    {
        base.Clear();

        for (int i = 0; i < reserves.Length; i++)
        {
            var element = base.GetInstance();
            element.Show(reserves[i]);
        }
    }

    public void AddElement(Reserve[] reserves)
    {
        for (int i = 0; i < reserves.Length; i++)
        {
            var element = base.GetInstance();
            element.Show(reserves[i]);
        }
    }
}
