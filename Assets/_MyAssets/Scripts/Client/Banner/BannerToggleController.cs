using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerToggleController : MonoBehaviour
{
    public Toggle toggle { get; private set; }
    [SerializeField] int _page;
    public int page => _page;

    public void OnStart(int initPage)
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ToggleValueChanged);
        toggle.isOn = (initPage == _page);
    }

    void ToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            UIManager.i.activePage = _page;
        }
    }
}
