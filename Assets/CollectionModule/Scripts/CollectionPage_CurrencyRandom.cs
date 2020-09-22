using System.Collections.Generic;
using System.Linq;
using Aloha.Currency;
using Aloha.ItemSystem;
using Aloha.ShopSystem;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPage_CurrencyRandom : CollectionPage
{
    [SerializeField] private Button _progressPurchaseButton;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private SkinManager skinManager;

    [Header("Product Create")]
    [SerializeField] private int _productId;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private CurrencyType _currency;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private List<int> _itemIds;
    [SerializeField] private List<int> _leveledPrices;

    [Header("Reward Currency")]
    [SerializeField] private int _rewardProductId;
    [SerializeField] private int _rewardAmount;
    [SerializeField] private AdsManager _adsManager;

    private Product _product;
    private Product _rewardProduct;

    private CollectionPurchaseController _purchaseController;

    void Start()
    {
        if (_progressPurchaseButton != null)
            _progressPurchaseButton.onClick.AddListener(ProgressPurchase);

    }

    protected override void OnFocusedCallback()
    {
    }
    public override void Initialize(CollectionItemSelectionHandler collectionItemSelectionHandler,
        CollectionPurchaseController purchaseController)
    {
        _purchaseController = purchaseController;
        var idx = 0;
        foreach (var itemId in _itemIds)
        {
            var collectionItem = _collectionItems[idx++];
            var item = _itemManager.GetItemById<IItem>(itemId);
            collectionItem.Initialize(item, true, new ReactiveProperty<string>(item.Name));
            collectionItemSelectionHandler.RegisterItemSelector(collectionItem);
        }

        _product = new Product(_productId, int.MaxValue, $"currency_random_{_productId}");
        var payment = new LeveledCurrencyPayment($"payment_{name}", _currency, _currencyManager, _leveledPrices.ToArray());
        var acquisition = new LumpSum();
        var bundle = new PickUpBundle(_itemIds.Select(id => (id, 1, 1)).ToList(), _inventory, _itemManager);
        _product.SetPayment(payment)
            .SetAcquisition(acquisition)
            .SetBundle(bundle);

        payment.Price.Subscribe(price => _priceText.text = price.ToString());

        _progressPurchaseButton.gameObject.SetActive(skinManager.CheckPurchaseButtons());
        _rewardButton.gameObject.SetActive(skinManager.CheckPurchaseButtons());
        bundle.OnUnboxed += (result) =>
        {
            FireBase.Instance.UseCoin(result.Picked.Item1,
               payment.Price.Value);
            FireBase.Instance.Skin(result.Picked.Item1, payment.Price.Value, 0, 0);
            CoinManager.Instance.InitCoinText();
            _progressPurchaseButton.gameObject.SetActive(skinManager.CheckPurchaseButtons());
            _rewardButton.gameObject.SetActive(skinManager.CheckPurchaseButtons());

        };

        _rewardProduct = new Product(_rewardProductId, int.MaxValue, $"currency_reward_{_rewardProductId}");
        var rewardPayment = new RewardedAdPayment("rewarded", _adsManager);
        var rewardAcquisition = new LumpSum();
        var rewardBundle = new CurrencyBundle($"currency_reward_{_rewardProductId}", _currency, _rewardAmount, _currencyManager);
        _rewardProduct.SetPayment(rewardPayment)
            .SetAcquisition(rewardAcquisition)
            .SetBundle(rewardBundle);
        if (_rewardButton != null)
        {
            _rewardButton.onClick.AddListener(() => _purchaseController.ProgressPurchase(_rewardProduct));
        }
    }

    private async void ProgressPurchase()
    {
        await _purchaseController.ProgressPurchase(_product);

    }
}
