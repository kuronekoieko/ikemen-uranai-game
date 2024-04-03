using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoadingPopup : BasePopup
{
    public async UniTask ShowFixedTime(float duration)
    {
        Open();
        await UniTaskUtils.DelaySecond(duration);
        Close();
    }

}
