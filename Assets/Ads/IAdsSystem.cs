using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAdsSystem
{
    void ShowBanner();
    void HideBanner();
    
    bool IsRewardedAdsReady(string adId);
    Task<bool> ShowRewardedAds(string adId);
    
    bool IsInterstitialAdsReady(string adId);
    Task ShowInterstitialAds(string adId);
}
