using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Utils
{

    public static Review[] OrderForKuchikomi(Uranaishi[] uranaishis)
    {
        var priorityDic = new Dictionary<UranaishiStatus, int>
        {
            {UranaishiStatus.Waiting,100},
            {UranaishiStatus.Counseling,90},
            {UranaishiStatus.DatTime,10},
            {UranaishiStatus.Closed,0},
        };


        var groups = uranaishis
        .SelectMany(uranaishi => uranaishi.reviews)
        .Where(review => review.isPickUp)
        .GroupBy(review => review.uranaishi.status)
        .OrderByDescending(g => priorityDic[g.Key]);

        Review[] reviews = new Review[0];
        foreach (var group in groups)
        {
            IOrderedEnumerable<Review> a = null;
            switch (group.Key)
            {
                case UranaishiStatus.Counseling:
                    a = group.OrderBy(i => Guid.NewGuid());
                    break;
                case UranaishiStatus.Waiting:
                    a = group.OrderBy(i => Guid.NewGuid());
                    break;
                case UranaishiStatus.Closed:
                    a = group.OrderByDescending(i => i.writtenDate.dateTime);
                    break;
                case UranaishiStatus.DatTime:
                    a = group.OrderByDescending(i => i.writtenDate.dateTime);
                    break;
                default:
                    a = group.OrderByDescending(i => i.writtenDate.dateTime);
                    break;
            }
            if (a != null) reviews = reviews.Concat(a).ToArray();
        }
        return reviews;
    }

    public static Dictionary<Sex, string> GetSexNameDic()
    {
        var dic = new Dictionary<Sex, string>();
        dic.Add(Sex.Man, "男性");
        dic.Add(Sex.Woman, "女性");
        dic.Add(Sex.None, "");
        return dic;
    }

    public static Dictionary<BloodType, string> GetBloodTypeNameDic()
    {
        var dic = new Dictionary<BloodType, string>();
        dic.Add(BloodType.A, "A型");
        dic.Add(BloodType.B, "B型");
        dic.Add(BloodType.AB, "AB型");
        dic.Add(BloodType.O, "O型");
        dic.Add(BloodType.Unknown, "未選択");
        return dic;

    }

}
