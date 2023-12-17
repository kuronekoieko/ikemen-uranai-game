using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HomeHeader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI jemFreeText;
    [SerializeField] TextMeshProUGUI jemChargingText;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI characterExpPerText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image characterExpBarImage;
    [SerializeField] Button chargingScreenButton;


    public void OnStart()
    {

    }
}
