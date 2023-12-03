using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : ObjectPooling<HistoryElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void ShowElement(History[] histories)
    {
        base.Clear();

        for (int i = 0; i < histories.Length; i++)
        {
            var element = base.GetInstance();
            element.Show(histories[i]);
        }
    }

    public void AddElement(History[] histories)
    {
        for (int i = 0; i < histories.Length; i++)
        {
            var element = base.GetInstance();
            element.Show(histories[i]);
        }
    }
}
