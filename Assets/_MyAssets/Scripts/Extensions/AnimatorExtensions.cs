using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class AnimatorExtensions
{
    public async static UniTask SetTriggerAsync(this Animator self, string triggerName)
    {
        self.SetTrigger(triggerName);
        await UniTask.WaitUntil(() => self.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    public async static UniTask PlayAsync(this Animator self, string stateName, int layer = 0)
    {
        // https://kan-kikuchi.hatenablog.com/entry/Animator_Replay
        self.Play(stateName, layer, 0);
        await UniTask.WaitUntil(() => self.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f); // animationが終わるまで遅延
    }
}
