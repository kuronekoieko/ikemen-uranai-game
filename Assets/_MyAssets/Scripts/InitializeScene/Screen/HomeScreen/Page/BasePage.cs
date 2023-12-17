using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BasePage : MonoBehaviour
{
    public virtual void OnStart()
    {
        gameObject.SetActive(false);
    }
    public virtual async void Open()
    {
        var basePage = PageManager.Instance.GetActive();
        basePage?.Close();
        gameObject.SetActive(true);
    }

    public void Close()
    {

        gameObject.SetActive(false);
        OnClose();
    }

    protected abstract void OnClose();
}
