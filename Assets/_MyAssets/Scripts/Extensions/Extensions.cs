using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public static class Extensions
{

    public static string ToNonNull(this string self)
    {
        if (string.IsNullOrEmpty(self)) return "";
        return self;
    }

    public static void SetLimitedText(this Text self, string originalText, int lineLimit)
    {
        self.verticalOverflow = VerticalWrapMode.Overflow;

        if (string.IsNullOrEmpty(originalText))
        {
            self.text = "";
            return;
        }
        var text = originalText;
        var textLength = originalText.Length;
        //TextGenerationSettingにはTextのRectSizeを渡す
        var setting = self.GetGenerationSettings(new Vector2(self.rectTransform.rect.width, self.rectTransform.rect.height));
        var generator = self.cachedTextGeneratorForLayout;
        while (true)
        {
            //Populate関数を使って一度評価を行う
            generator.Populate(text, setting);
            if (generator.lineCount > lineLimit)
            {
                //指定の行数より長い場合1文字削って試す
                textLength--;
                text = text.Substring(0, textLength) + "...";
            }
            else
            {
                //指定行数に収まったのでTextに文字列を設定する
                self.text = text;
                break;
            }
        }



    }

    public static string GetJoinedKeywords(this string[] self)
    {
        return string.Join("、", self);
    }

    public static string ToStringIncludeEmpty(this Nullable<DateTime> self, string format = "")
    {
        if (self == null)
        {
            return "n/a";
        }
        else
        {
            return self.Value.ToString(format);
        }
    }

    public static string ToDateKey(this DateTime self)
    {
        return self.ToString("yyyy-MM-dd");
    }
}
