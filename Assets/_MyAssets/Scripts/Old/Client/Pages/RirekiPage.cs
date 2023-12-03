using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirekiPage : BasePageManager
{
    public override void OnStart()
    {
        base.SetPageAction(2);

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
}
