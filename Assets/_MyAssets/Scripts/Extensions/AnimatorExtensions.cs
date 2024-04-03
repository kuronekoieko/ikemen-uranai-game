using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class AnimatorExtensions
{
    public async static UniTask SetTriggerAsync(this Animator self, string stateName, string triggerName)
    {
        self.SetTrigger(triggerName);
        //animator.Play("CloseWindow");

        await UniTask.WaitUntil(() =>
            self.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            self.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        ); // animationが終わるまで遅延

    }

    public async static UniTask PlayAsync(this Animator self, string stateName)
    {
        // https://kan-kikuchi.hatenablog.com/entry/Animator_Replay
        self.Play(stateName, 0, 0);
        await UniTask.WaitUntil(() => self.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f); // animationが終わるまで遅延
    }
}
