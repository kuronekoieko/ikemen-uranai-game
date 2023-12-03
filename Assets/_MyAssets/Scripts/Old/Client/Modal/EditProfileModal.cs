using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using DeadMosquito.AndroidGoodies;
using System.Linq;
using UniRx;

public class EditProfileModal : BaseModal
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TMP_InputField firstNameIF;
    [SerializeField] TMP_InputField lastNameIF;
    [SerializeField] Button birthdayButton;
    [SerializeField] TextMeshProUGUI birthdayText;
    [SerializeField] Button selectSexButton;
    [SerializeField] TextMeshProUGUI sexText;
    [SerializeField] Button selectBloodTypeButton;
    [SerializeField] TextMeshProUGUI bloodTypeText;
    [SerializeField] Button saveButton;
    bool isMe;
    Profile profile;
    bool isEdited
    {
        get
        {
            if (profile == null) return false;
            return !profile.Equals(SaveData.i.GetProfile(isMe));
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        saveButton.onClick.AddListener(OnClickSaveButton);
        birthdayButton.onClick.AddListener(OnClickBirthdayButton);
        selectSexButton.onClick.AddListener(OnClickSelectSexButton);
        selectBloodTypeButton.onClick.AddListener(OnClickSelectBloodTypeButton);
        base.modalCommon.xButton.onClick.RemoveAllListeners();
        base.modalCommon.xButton.onClick.AddListener(OnClose);
        firstNameIF.onValueChanged.AddListener((str) =>
        {
            profile.firstName = str;
        });
        lastNameIF.onValueChanged.AddListener((str) =>
        {
            profile.lastName = str;
        });

        profile = null;
        this.ObserveEveryValueChanged(_isEdited => isEdited)
            .Subscribe(_isEdited => saveButton.interactable = _isEdited)
            .AddTo(this.gameObject);
    }

    public void Open(bool isMe)
    {

        base.OpenAnim();
        this.isMe = isMe;

        profile = new Profile(SaveData.i.GetProfile(isMe));

        titleText.text = (isMe ? "自分" : "相手") + "の情報";
        firstNameIF.text = profile.firstName;
        lastNameIF.text = profile.lastName;
        DateTime? birthDay = profile.birthDaySDT.dateTime;

        birthdayText.text = birthDay == null ?
            "選択する" : birthDay.ToStringIncludeEmpty("yyyy年MM月dd日");
        SetSexText();
        SetBloodTypeText();
    }

    void SetSexText()
    {
        string sexName = Utils.GetSexNameDic()[profile.sex];
        sexText.text = sexName == "" ? "未選択" : sexName;
    }

    void SetBloodTypeText()
    {
        string bloodTypeName = Utils.GetBloodTypeNameDic()[profile.bloodType];
        bloodTypeText.text = bloodTypeName == "" ? "未選択" : bloodTypeName;
    }


    void OnClickSaveButton()
    {
        SaveData.i.SetProfile(isMe, profile);
        SaveDataManager.i.Save();
        base.Close();
    }

    void OnClickBirthdayButton()
    {
        NativeDatePicker.ShowDatePicker((dateTime) =>
        {
            DateTime? birthDay = dateTime;
            profile.birthDaySDT.dateTime = birthDay;
            birthdayText.text = birthDay.ToStringIncludeEmpty("yyyy年MM月dd日");
        });

        Debug.Log("日付ピッカー表示");
    }

    void OnClickSelectSexButton()
    {
        List<string> sexOptions = new List<string>(Utils.GetSexNameDic().Values);


        AGAlertDialog.ShowSingleItemChoiceDialog(
            "性別",
            sexOptions.ToArray(),
            0,
            (colorIndex) =>
            {
                profile.sex = (Sex)Enum.ToObject(typeof(Sex), colorIndex);
                SetSexText();
            },
            "選択",
            () =>
            {

            });
    }


    void OnClickSelectBloodTypeButton()
    {
        List<string> bloodTypeOptions = new List<string>(Utils.GetBloodTypeNameDic().Values);


        AGAlertDialog.ShowSingleItemChoiceDialog(
            "血液型",
            bloodTypeOptions.ToArray(),
            0,
            (colorIndex) =>
            {
                profile.bloodType = (BloodType)Enum.ToObject(typeof(BloodType), colorIndex);
                SetBloodTypeText();
            },
            "選択",
            () =>
            {

            });
    }

    void OnClose()
    {
        // 無編集のときに、ポップアップを出さない

        if (!isEdited)
        {
            base.Close();
            return;
        }

        CommonPopup.i.Show(
          "編集を終了しますか？",
          "編集中の内容は反映されません",
          "はい",
          "いいえ"
          , () =>
          {
              base.Close();
          }
      );
    }


}
