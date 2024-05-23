using System.Collections;
using System.Collections.Generic;
using SaveDataObjects;
using UnityEngine;

public class PartnerPool : ObjectPooling<PartnerElement>
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public void Show(PartnerProfile[] ary)
    {
        foreach (var item in ary)
        {
            var instance = GetInstance();
            instance.Show(item);
        }
    }

    public void Add(PartnerProfile partnerProfile)
    {
        var instance = GetInstance();
        instance.Show(partnerProfile);
    }
}
