using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioCalculator : Singleton<AspectRatioCalculator>
{

    public Vector2Int GetAspectRatio(int w, int h)
    {
        int gcd = Gcd(w, h);
        Vector2Int aspectRatio = new Vector2Int();
        aspectRatio.x = w / gcd;
        aspectRatio.y = h / gcd;
        return aspectRatio;
    }

    /// <summary>
    /// https://qiita.com/gushwell/items/e9614b4ac2bea3fc6486
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int Gcd(int a, int b)
    {
        if (a < b)
            // 引数を入替えて自分を呼び出す
            return Gcd(b, a);
        while (b != 0)
        {
            var remainder = a % b;
            a = b;
            b = remainder;
        }
        return a;
    }
}
