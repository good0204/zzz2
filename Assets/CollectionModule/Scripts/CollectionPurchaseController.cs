using System;
using System.Threading.Tasks;
using Aloha.ShopSystem;
using UnityEngine;
public class CollectionPurchaseController
{
    public event Action<Product> OnProductPurchased;
    public Action<int, int, int> FirebaseEvent;
    public Action<int> FirebaseAdEvent;
    private PurchaseController _purchaseController = new PurchaseController();
    private bool _purchaseLock;

    public async Task<PurchaseController.Result> ProgressPurchase(Product product)
    {
        if (_purchaseLock) return PurchaseController.Result.PaymentFailed;
        _purchaseLock = true;
        var result = await _purchaseController.ProgressPurchase(product);
        OnProductPurchased?.Invoke(product);
        if (product.Id == 1)
        {
            FireBase.Instance.GetCoin(0, 0, 200);
            FireBase.Instance.Reward("coin200");
            CoinManager.Instance.InitCoinText();
        }
        _purchaseLock = false;
        return result;
    }
}
