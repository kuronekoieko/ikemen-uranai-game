using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class BasePageManager : MonoBehaviour
{
    int thisPage;

    /// <summary>
    /// OnOpenとOnCloseのイベント発火タイミングを設定する
    /// </summary>
    /// <param name="thisPage">セットする画面のenumを入れてください</param>
    protected void SetPageAction(int thisPage)
    {
        this.thisPage = thisPage;

        this.ObserveEveryValueChanged(Page => UIManager.i.activePage)
            .Where(Page => Page == thisPage)
            .Subscribe(Page => OnOpen())
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(Page => UIManager.i.activePage)
            .Buffer(2, 1)
            .Where(Page => Page[0] == thisPage)
            .Where(Page => Page[1] != thisPage)
            .Subscribe(Page => OnClose())
            .AddTo(this.gameObject);

        gameObject.SetActive(IsThisPage);
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
    protected abstract void OnOpen();
    protected abstract void OnClose();
    public bool IsThisPage => UIManager.i.activePage == thisPage;
}
