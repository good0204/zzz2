using System.Threading.Tasks;
using Aloha.Currency;
using Aloha.ShopSystem;
using UniRx;
using UnityEngine;

public class LeveledCurrencyPayment : IPaymentModule
{
    public ICurrencyType CurrencyType => _currencyType;

    public IReadOnlyReactiveProperty<int> Price =>
        _level
            .Select(lv => Mathf.Clamp(lv, 0, _leveledPrices.Length - 1))
            .Select(lv => _leveledPrices[lv])
            .ToReadOnlyReactiveProperty();

    private string _id;
    private RecordIntProperty _level;
    private int[] _leveledPrices;
    private ICurrencyType _currencyType;
    private ICurrencyManager _currencyManager;

    public LeveledCurrencyPayment(string id, ICurrencyType currencyType, ICurrencyManager currencyManager, int[] leveledPrices)
    {
        _level = new RecordIntProperty(id, 0);
        _leveledPrices = leveledPrices;
        _currencyType = currencyType;
        _currencyManager = currencyManager;
    }

    public bool IsAvailable()
    {
        return _currencyManager.HaveGreaterOrEqual(_currencyType, Price.Value);
    }

    public async Task<bool> TryProgressPayment()
    {
        if (!IsAvailable())
        {
            return false;
        }
        _currencyManager.TryUse(_currencyType, Price.Value, _id);
        _level.Value++;
        return true;
    }
}
