using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputReviewScreen : BaseCallingScreen
{
    [SerializeField] InputReviewStarManager inputReviewStarManager;
    [SerializeField] Button sendButton;
    [SerializeField] TMP_InputField reviewTextIF;
    [SerializeField] TMP_InputField giftIF;
    [SerializeField] TextMeshProUGUI myPointText;
    [SerializeField] Button purchaseButton;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI uranaishiNameText;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TextMeshProUGUI characterLimitText;
    int giftPoint;

    public override void OnStart(CallingManager callingManager)
    {
        base.OnStart(callingManager);
        inputReviewStarManager.OnStart();
        sendButton.onClick.AddListener(() =>
        {
            SendData();
        });
        purchaseButton.onClick.AddListener(() =>
        {
            UIManager.i.GetModal<PurchaseModal>().Open();
            UIManager.i.GetModal<PurchaseModal>().onClose += OnReturn;
        });

        reviewTextIF.characterLimit = 400;
        characterLimitText.text = 0 + "/" + reviewTextIF.characterLimit;
        reviewTextIF.onValueChanged.AddListener((text) =>
        {
            characterLimitText.text = text.Length + "/" + reviewTextIF.characterLimit;
            if (text.Length == reviewTextIF.characterLimit)
            {
                characterLimitText.color = Color.red;
            }
            else
            {
                characterLimitText.color = Color.black;
            }
        });

        giftIF.contentType = TMP_InputField.ContentType.IntegerNumber;
        giftIF.onValueChanged.AddListener((text) =>
        {
            if (int.TryParse(text, out giftPoint))
            {
                giftPoint = Mathf.Clamp(giftPoint, 0, SaveData.i.GetSumPoint());
            }
            giftIF.text = giftPoint.ToString();
        });
    }

    public override void Open(Uranaishi uranaishi)
    {
        base.Open(uranaishi);
        //スクロールの一番上に行くように
        scrollRect.verticalNormalizedPosition = 1;
        myPointText.text = $"所持ポイント : {SaveData.i.GetSumPoint()}pt";
        inputReviewStarManager.ChangeView(4);
        uranaishiNameText.text = uranaishi.name;
        uranaishi.GetIcon((sprite) =>
        {
            iconImage.sprite = sprite;
        });
    }

    public override void Close()
    {
        base.Close();
    }

    async void SendData()
    {
        Review review = new Review
        (
            starCount: inputReviewStarManager.starCount,
            text: reviewTextIF.text
        );
        // 他のパラメーターの入力はどうするか？



        uranaishi.reviews.Add(review);

        // TODO: 占い師が同時に操作してると上書きされる可能性があるので、レビューだけ書き換えたい
        await FirebaseDatabaseManager.i.SetUserData(uranaishi);

        SaveData.i.ConsumePoints(giftPoint);
        SaveDataManager.i.Save();





        if (Application.isEditor)
        {
            CommonPopup.i.Show(
                title: "評価を投稿します。\nよろしいですか？",
                message: "",
                positive: "はい",
                negative: "いいえ",
                onClickPositiveButton: CloseAllModals
            );
        }
        else
        {
            CBNativeDialog.Instance.Show(
                title: "",
                message: "評価を投稿します。\nよろしいですか？",
                positiveButtonTitle: "はい",
                positiveButtonAction: () => { CloseAllModals(); },
                negativeButtonTitle: "いいえ",
                negativeButtonAction: () => { Debug.Log("CANCEL"); });
        }

    }

    void CloseAllModals()
    {
        base.manager.Close();
        UIManager.i.CloseAllModals(1);
    }

    void OnReturn()
    {
        myPointText.text = $"所持ポイント : {SaveData.i.GetSumPoint()}pt";
    }
}
