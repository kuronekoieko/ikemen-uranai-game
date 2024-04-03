using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;

public static class ButtonExtensions
{
    public static void AddListener(this Button self, Func<UniTask> func)
    {
        self.onClick.AddListener(async () =>
        {
            if (CanvasManager.Instance == null)
            {
                await func();
                return;
            }

            // naninovelのUIカメラを10に設定しているため
            CanvasManager.Instance.Canvas.worldCamera.depth = 9;
            CanvasManager.Instance.GraphicRaycaster.enabled = false;
            self.enabled = false;
            await func();
            CanvasManager.Instance.Canvas.worldCamera.depth = 11;
            CanvasManager.Instance.GraphicRaycaster.enabled = true;
            self.enabled = true;
        });
    }
}
