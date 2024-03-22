using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    #region position

    public static void SetPosX(this RectTransform self, float x)
    {
        self.position = new Vector3(x, self.position.y, self.position.z);
    }

    public static void SetPosY(this RectTransform self, float y)
    {
        self.position = new Vector3(self.position.x, y, self.position.z);
    }

    public static void SetPosZ(this RectTransform self, float z)
    {
        self.position = new Vector3(self.position.x, self.position.y, z);
    }

    public static void AddPosX(this RectTransform self, float x)
    {
        self.position = new Vector3(self.position.x + x, self.position.y, self.position.z);
    }

    public static void AddPosY(this RectTransform self, float y)
    {
        self.position = new Vector3(self.position.x, self.position.y + y, self.position.z);
    }

    public static void AddPosZ(this RectTransform self, float z)
    {
        self.position = new Vector3(self.position.x, self.position.y, self.position.z + z);
    }

    #endregion

    #region anchoredPosition

    public static void SetAnchoredPositionX(this RectTransform self, float x)
    {
        self.anchoredPosition = new Vector2(x, self.anchoredPosition.y);
    }

    public static void SetAnchoredPositionY(this RectTransform self, float y)
    {
        self.anchoredPosition = new Vector2(self.anchoredPosition.x, y);
    }

    public static void AddAnchoredPositionX(this RectTransform self, float x)
    {
        self.anchoredPosition = new Vector2(self.anchoredPosition.x + x, self.anchoredPosition.y);
    }

    public static void AddAnchoredPositionY(this RectTransform self, float y)
    {
        self.anchoredPosition = new Vector2(self.anchoredPosition.x, self.anchoredPosition.y + y);
    }

    #endregion

    #region anchoredPosition3D

    public static void SetAnchoredPosition3DX(this RectTransform self, float x)
    {
        self.anchoredPosition3D = new Vector3(x, self.anchoredPosition3D.y, self.anchoredPosition3D.z);
    }

    public static void SetAnchoredPosition3DY(this RectTransform self, float y)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, y, self.anchoredPosition3D.z);
    }

    public static void SetAnchoredPosition3DZ(this RectTransform self, float z)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, self.anchoredPosition3D.y, z);
    }

    public static void AddAnchoredPosition3DX(this RectTransform self, float x)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x + x, self.anchoredPosition3D.y, self.anchoredPosition3D.z);
    }

    public static void AddAnchoredPosition3DY(this RectTransform self, float y)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, self.anchoredPosition3D.y + y, self.anchoredPosition3D.z);
    }

    public static void AddAnchoredPosition3DZ(this RectTransform self, float z)
    {
        self.anchoredPosition3D = new Vector3(self.anchoredPosition3D.x, self.anchoredPosition3D.y, self.anchoredPosition3D.z + z);
    }

    #endregion

    public static RectInt ToRectInt(this Rect self)
    {
        RectInt rectInt = new(
            Mathf.CeilToInt(self.x),
            Mathf.CeilToInt(self.y),
            Mathf.CeilToInt(self.width),
            Mathf.CeilToInt(self.height)
        );
        return rectInt;
    }

    public static Rect ToRect(this RectInt self)
    {
        Rect rect = new(
            self.x,
            self.y,
            self.width,
            self.height
        );
        return rect;
    }

    /// <summary>
    /// Rect構造体の情報を元に領域を決定する
    /// https://nekojara.city/unity-rect-transform-corners#Rect%E6%A7%8B%E9%80%A0%E4%BD%93%E3%81%A7%E6%8C%87%E5%AE%9A%E3%81%99%E3%82%8B/// 
    /// </summary>
    public static void SetRect(this RectTransform rt, in Rect rect)
    {
        // アンカー領域を左下に設定する
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;

        // 親の左下を原点に、指定された矩形の位置を設定する
        rt.offsetMin = rect.min;
        rt.offsetMax = rect.max;
    }
}
