using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputProfileScreen : SingletonMonoBehaviour<InputProfileScreen>
{

    [SerializeField] ScrollSelector monthScrollSelector;
    [SerializeField] ScrollSelector dayScrollSelector;

    public void OnStart()
    {
        gameObject.SetActive(false);


        var months = new List<string>();
        for (int i = 1; i <= 12; i++)
        {
            months.Add(i.ToString());
        }

        var days = new List<string>();
        for (int i = 1; i <= 31; i++)
        {
            days.Add(i.ToString());
        }

        monthScrollSelector.OnStart(months, 0);
        dayScrollSelector.OnStart(days, 0);
    }

    public void Open()
    {
        gameObject.SetActive(true);


    }
}
