using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class OkiniiriPage : BasePageManager
{

    [SerializeField] UranaishiButtonManager uranaishiButtonManager;
    [SerializeField] HistoryManager historyManager;

    public override void OnStart()
    {
        base.SetPageAction(1);
        uranaishiButtonManager.OnStart();
        historyManager.OnStart();

        ShowLikedList();

        this.ObserveEveryValueChanged(listCount => SaveData.i.likedUranaishiIdList.Count)
            .Where(listCount => UIManager.i.activePage == 1)
            .Subscribe(listCount => ShowLikedList())
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(count => SaveData.i.histories.Count)
            .Subscribe(contains => OnChangedHistories())
            .AddTo(this.gameObject);
    }

    public override void OnUpdate()
    {
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        ShowLikedList();
    }


    void ShowLikedList()
    {
        List<Uranaishi> likedList = new List<Uranaishi>();

        foreach (var uranaishi in UIManager.i.uranaishiAry)
        {
            foreach (var likedUranaishiId in SaveData.i.likedUranaishiIdList)
            {
                if (uranaishi.id == likedUranaishiId)
                {
                    likedList.Add(uranaishi);
                }
            }
        }

        uranaishiButtonManager.ShowButtons(likedList.ToArray());
    }

    void OnChangedHistories()
    {
        History[] histories = SaveData.i.histories
        .OrderByDescending(h => h.startCallingSDT.dateTime)
        .ToArray();
        historyManager.ShowElement(histories);
    }
}
