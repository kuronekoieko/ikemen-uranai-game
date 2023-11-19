using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class KuchikomiPage : BasePageManager
{
    [SerializeField] KuchikomiManager kuchikomiManager;
    public override void OnStart()
    {
        base.SetPageAction(3);
        kuchikomiManager.OnStart();
        Order();
    }

    public override void OnUpdate()
    {
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
    }


    void Order()
    {
        Review[] reviews = Utils.OrderForKuchikomi(UIManager.i.uranaishiAry);
        kuchikomiManager.AddElement(reviews);

    }

}
