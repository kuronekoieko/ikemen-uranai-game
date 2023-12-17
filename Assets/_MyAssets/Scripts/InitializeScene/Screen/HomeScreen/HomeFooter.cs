using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HomeFooter : MonoBehaviour
{
    [SerializeField] Button homeButton;
    [SerializeField] Button charactersButton;
    [SerializeField] Button gachaButton;
    [SerializeField] Button horoscopeButton;
    [SerializeField] Button storyButton;

    public void OnStart()
    {
        homeButton.onClick.AddListener(OnClickHomeButton);
        charactersButton.onClick.AddListener(OnClickHomeButton);
        gachaButton.onClick.AddListener(OnClickHomeButton);
        horoscopeButton.onClick.AddListener(OnClickHomeButton);
        storyButton.onClick.AddListener(OnClickHomeButton);
    }

    void OnClickHomeButton()
    {

    }
    


}
