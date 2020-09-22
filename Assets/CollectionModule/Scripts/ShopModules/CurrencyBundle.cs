using System.Collections;
using System.Collections.Generic;
using Aloha.Currency;
using Aloha.ShopSystem;
using UnityEngine;

public class CurrencyBundle : IBundleModule
{
    public ICurrencyType CurrencyType => _currencyType;
    public string Name => _name;
    public int Amount => _amount;
    
    private ICurrencyType _currencyType;
    private ICurrencyManager _currencyManager;
    private int _amount;
    private string _name;
    
    public CurrencyBundle(string name, ICurrencyType currencyType, int amount, ICurrencyManager currencyManager)
    {
        _name = name;
        _amount = amount;
        _currencyManager = currencyManager;
        _currencyType = currencyType;
    }
    
    public bool IsAvailable()
    {
        return true;
    }

    public void Unbox()
    {
        _currencyManager.Gain(_currencyType, _amount, _name);
    }
}
