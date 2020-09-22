using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aloha.ShopSystem;
using UnityEngine;

public class RewardedAdPayment : IPaymentModule
{
    private AdsManager _adsManager;
    private readonly string _placement;
    
    public RewardedAdPayment(string placement, AdsManager adsManager)
    {
        _adsManager = adsManager;
        _placement = placement;
    }

    public bool IsAvailable()
    {
        return true;
    }

    public async Task<bool> TryProgressPayment()
    {
        var result =  await _adsManager.ShowRewarded(_placement);
        if (result == AdsResult.Complete) return true;
        return false;
    }
}
