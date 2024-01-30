/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine.Purchasing.Security;

public class PurchaseResult
{
    public PurchaseProcessingResult purchaseProcessingResult;
    public PurchaseFailureReason purchaseFailureReason;
    public Result result = Result.Waiting;
    public enum Result
    {
        Waiting,
        Processing,
        Failed,
    }
}

public class IAPManager : IStoreListener
{
    /// <summary>
    /// 商品情報
    /// </summary>
    public class ProductInfo
    {
        public string ID { private set; get; }
        public ProductType Type { private set; get; }

        public ProductInfo(string _id, ProductType _type) => (ID, Type) = (_id, _type);
    }

    private IStoreController m_storeController;
    private IExtensionProvider m_extensionProvider;
    public static IAPManager i => _i;
    static IAPManager _i = new IAPManager();

    public bool IsInitialized => m_storeController != null && m_extensionProvider != null;
    public ProductInfo[] _products = new ProductInfo[] {
        new IAPManager.ProductInfo(
            "com.superchatai.com.sub",
            UnityEngine.Purchasing.ProductType.Subscription
        )
    };

    private IAppleExtensions appleExtensions;
    PurchaseResult purchaseResult;



    /// <summary>
    /// 初期化
    /// </summary>
    public async UniTask Initialize()
    {
        if (!IsInitialized)
        {
            var m_builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var product in _products)
            {
                m_builder.AddProduct(product.ID, product.Type);
            }
            Debug.Log("課金初期化 開始");
            UnityPurchasing.Initialize(this, m_builder);
        }

        await UniTask.WaitUntil(() => IsInitialized);
    }

    /// <summary>
    /// 初期化 成功
    /// </summary>
    public void OnInitialized(IStoreController _controller, IExtensionProvider _extensions)
    {
        m_storeController = _controller;
        m_extensionProvider = _extensions;
        this.appleExtensions = _extensions.GetExtension<IAppleExtensions>();

        Debug.Log("課金初期化成功");
    }

    /// <summary>
    /// 初期化 失敗
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason _error)
    {
        // 失敗時のエラー処理を実装
        Debug.Log(_error);
        Debug.Log("課金初期化失敗");
    }

    public void OnInitializeFailed(InitializationFailureReason a, string b)
    {
        Debug.Log(a);

    }

    /// <summary>
    /// 購入成功時
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs _purchaseEvent)
    {
        ValidateReceipt(_purchaseEvent.purchasedProduct.receipt);
        //bool validPurchase = Validate(_purchaseEvent.purchasedProduct.receipt);
        Debug.Log(_purchaseEvent.purchasedProduct);
        purchaseResult.result = PurchaseResult.Result.Processing;
        // 購入が成功した場合
        purchaseResult.purchaseProcessingResult = PurchaseProcessingResult.Complete;
        // 購入した商品情報は_purchaseEvent.purchasedProductから取得可能
        return PurchaseProcessingResult.Complete;
    }

    private IEnumerator ValidateReceipt(string receipt)
    {
        var request = UnityWebRequest.Post("https://your-server.com/verify_receipt", receipt);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            // レシート検証が成功した場合の処理
        }
    }


    /// <summary>
    /// 購入処理 失敗
    /// </summary>
    public void OnPurchaseFailed(Product _product, PurchaseFailureReason _failureReason)
    {
        // 失敗時のエラー処理を実装
        Debug.Log("PurchaseFailureReason: " + _failureReason);
        purchaseResult.result = PurchaseResult.Result.Failed;
        purchaseResult.purchaseFailureReason = _failureReason;
    }

    /// <summary>
    /// 商品購入
    /// </summary>
    public async UniTask<PurchaseResult> Purchase(int index)
    {
        await UniTask.WaitUntil(() => IsInitialized);
        purchaseResult = new PurchaseResult();

        string productID = _products[index].ID;

        m_storeController.InitiatePurchase(productID);

        await UniTask.WaitUntil(() => purchaseResult.result != PurchaseResult.Result.Waiting);

        return purchaseResult;
    }

    public async UniTask<string> GetProductPrice(int index)
    {
        await UniTask.WaitUntil(() => IsInitialized);

        Product product = m_storeController.products.WithID(_products[index].ID);

        // 商品が見つかった場合、価格を取得
        if (product != null && product.metadata != null)
        {
            return product.metadata.localizedPriceString;
        }
        return "";
    }

    public async UniTask<bool> IsPurchased(int index)
    {
        await UniTask.WaitUntil(() => IsInitialized);

        string productID = _products[index].ID;

        Product product = m_storeController.products.WithID(productID);
        if (product != null && product.hasReceipt)
        {
            Debug.Log(productID + " is purchased.");
            return true;
        }
        else
        {
            Debug.Log(productID + " is not purchased.");
            return false;
        }
    }

    public async UniTask<bool> RestorePurchases()
    {
        await UniTask.WaitUntil(() => IsInitialized);

        bool isComplete = false;
        bool isSuccess = false;

        m_extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, s) =>
        {
            if (result)
            {
                Debug.Log("Restore successful.");
                isSuccess = true;
            }
            else
            {
                Debug.Log("Restore failed.");
                isSuccess = false;
            }
            isComplete = true;
        });
        await UniTask.WaitUntil(() => isComplete);
        return isSuccess;

    }

    public async UniTask<bool> IsSubscribed()
    {
        await UniTask.WaitUntil(() => IsInitialized);

        // string[] ids = ConstantsSO.Instance.resources.Select(r => r.productId).ToArray();
        //bool isSubscribed = ids.Any(id => IAPManager.i.CheckIfPurchased(id));
        bool isSubscribed = await IsPurchased(0);

        // return false;
        return isSubscribed;
    }

    [System.Serializable]
    private class Receipt
    {
        public string Payload = string.Empty;
    }
    [System.Serializable]
    private class Payload
    {
        public string json = string.Empty;
        public string signature = string.Empty;
    }
    [System.Serializable]
    private class Json
    {
        public string purchaseToken = string.Empty;
        public string productId = string.Empty;
        public string orderId = string.Empty;
    }
}
*/