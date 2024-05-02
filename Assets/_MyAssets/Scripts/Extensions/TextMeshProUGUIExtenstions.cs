using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public static class TextMeshProUGUIExtenstions
{
    public static void LimitLineCount(this TextMeshProUGUI self, int lineCountMax, int fontSizeMin)
    {

        var textInfo = self.GetTextInfo(self.text);

        //Debug.Log(" ================");


        // 指定したテキストを入れた時に何行に改行されるかを取得する。
        // Debug.Log(self.fontSize + " " + textInfo.lineCount);

        while (textInfo.lineCount > lineCountMax)
        {
            self.fontSize--;
            textInfo = self.GetTextInfo(self.text);
            // Debug.Log(self.fontSize + " " + textInfo.lineCount);
            if (self.fontSize < fontSizeMin) break;
        }
        string leader = "...";

        while (textInfo.lineCount > lineCountMax)
        {
            self.text = self.text.Substring(0, self.text.Length - 1 - leader.Length) + leader;
            textInfo = self.GetTextInfo(self.text);
            // Debug.Log(self.text.Length + " " + textInfo.lineCount);
        }
        Debug.Log(self.text.Length + " " + textInfo.lineCount);

    }
}
