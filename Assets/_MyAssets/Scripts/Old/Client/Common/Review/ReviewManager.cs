using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : ObjectPooling<ReviewElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void ShowElements(Review[] reviews)
    {
        base.Clear();

        for (int i = 0; i < reviews.Length; i++)
        {
            var reviewElement = base.GetInstance();
            reviewElement.ShowData(reviews[i]);
        }
    }
}
