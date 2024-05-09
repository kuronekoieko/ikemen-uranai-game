using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;

public static class ButtonExtensions
{
    public static void AddListener(this Button self, Func<UniTask> func, AudioID audioID = AudioID.BtnClick_Positive)
    {
        self.onClick.AddListener(async () =>
        {
            AudioManager.Instance.PlayOneShot(audioID);

            if (ScreenManager.Instance == null)
            {
                await func();
                return;
            }

            // naninovelのUIカメラを10に設定しているため
            ScreenManager.Instance.UICamera.depth = 9;
            ScreenManager.Instance.CanvasGroup.blocksRaycasts = false;
            self.enabled = false;
            await func();
            ScreenManager.Instance.UICamera.depth = 11;
            ScreenManager.Instance.CanvasGroup.blocksRaycasts = true;
            self.enabled = true;
        });
    }


}
