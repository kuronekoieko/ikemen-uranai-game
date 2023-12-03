using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuchikomiManager : ObjectPooling<KuchikomiElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void ShowElement(Review[] reviews)
    {
        base.Clear();

        for (int i = 0; i < reviews.Length; i++)
        {
            var element = base.GetInstance();
            element.Show(reviews[i]);
        }
    }

    public void AddElement(Review[] reviews)
    {
        for (int i = 0; i < reviews.Length; i++)
        {
            var element = base.GetInstance();
            element.Show(reviews[i]);
        }
    }
}
