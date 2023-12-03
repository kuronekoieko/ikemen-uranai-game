using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "MyGame/Create " + nameof(Constant), fileName = nameof(Constant))]
public class Constant : SingletonScriptableObject<Constant>
{
    public ReadOnlyValue<string[]> expertises;
    public ReadOnlyValue<string>[] divinations;
    public ReadOnlyValue<List<ChargeInfo>> chargeInfos;
    public int reserveDurationMin;
    public int reserveDays;

    [Button]
    void InitChargeInfos()
    {
        chargeInfos.value.Clear();

        ChargeInfo chargeInfo = new ChargeInfo();
        chargeInfo.point = 1000;
        chargeInfo.cost = 1000;
        chargeInfo.omake = 30;
        chargeInfos.value.Add(chargeInfo);


        for (int i = 0; i < 10; i++)
        {
            ChargeInfo c = new ChargeInfo();
            c.point = chargeInfo.point * (i + 1) * 5;
            c.cost = chargeInfo.cost * (i + 1) * 5;
            c.omake = chargeInfo.omake * (i + 1) * 5;
            chargeInfos.value.Add(c);
        }
    }

}

[System.Serializable]
public class ChargeInfo
{
    public int point;
    public int cost;
    public int omake;
}