using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class MyUniTaskExtensions
{
    public async static UniTask DelaySecond(this UniTask self, float second, CancellationTokenSource _tokenSource = null)
    {
        float f = second * 1000;
        if (_tokenSource == null)
        {
            await UniTask.Delay((int)f);
        }
        else
        {
            try
            {
                await UniTask.Delay((int)f, cancellationToken: _tokenSource.Token);
            }
            catch (System.Exception)
            {

            }
        }
    }

    public async static UniTask<bool> TimeOutSeconds(this UniTask self, float second)
    {
        float f = second * 1000;
        UniTask timeOut = UniTask.Delay((int)f);
        int index = await UniTask.WhenAny(self, timeOut);
        bool isTimeout = index == 1;
        return isTimeout;
    }

    public async static UniTask<(bool isTimeout, T result)> TimeOutSeconds<T>(this UniTask<T> self, float second)
    {
        float f = second * 1000;
        UniTask timeOut = UniTask.Delay((int)f);
        (bool hasResultLeft, T result) = await UniTask.WhenAny(self, timeOut);
        bool isTimeout = hasResultLeft == false;
        return (isTimeout, result);
    }
}



