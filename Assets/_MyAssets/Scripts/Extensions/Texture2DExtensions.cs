using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Texture2DExtensions
{
    public static Sprite ToSprite(this Texture2D self)
    {
        return Sprite.Create(self, new Rect(0, 0, self.width, self.height), Vector2.zero);
    }


    public static string ToBase64String(this Texture2D self)
    {

        // 【Unity】Texture '...' is not readable, the texture memory can not be accessed from scripts. というエラーの対処法
        // https://www.hanachiru-blog.com/entry/2019/09/17/224404
        // Unityで画面をキャプチャーしようとした際に「ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame.」
        // https://uwanosora22.hatenablog.com/entry/2016/07/17/035101
        // UnityでGoogle Maps Viewを利用したときの "Unsupported texture format" の対応方法
        // https://qiita.com/NorsteinBekkler/items/335cc0833122b37bb049

        byte[] jpg = self.EncodeToJPG();
        var encode = Convert.ToBase64String(jpg);
        return encode;
    }

    public static Texture2D Base64ToTexture2D(string base64)
    {
        var t = new Texture2D(1, 1)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        t.LoadImage(Convert.FromBase64String(base64));
        return t;

    }


}
