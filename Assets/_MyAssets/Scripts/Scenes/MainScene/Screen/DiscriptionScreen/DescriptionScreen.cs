using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionScreen : BaseScreen
{

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;

    public override void OnStart()
    {
        base.OnStart();
    }

    public void Open(string title, string description)
    {
        base.Open();

        titleText.text = title;
        descriptionText.text = description;
    }

    public override void Close()
    {
        base.Close();
    }
}
