using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Ads Manager")]
public class AdsManager : ScriptableObject
{
    public static bool AdsBlocked;

    public event Action<string> OnShowInterstitial;
    public event Action<string> OnShowRewarded;

    private IAdsSystem _adsSystem;
    private DateTime _lastInterstitialTime;

    [SerializeField] private float _interstitialCooltime = 60f;

    public void Initialize(IAdsSystem system)
    {
        _adsSystem = system;
        if (!AdsBlocked) _adsSystem.ShowBanner();

        _lastInterstitialTime = DateTime.UtcNow - TimeSpan.FromMinutes(30);
    }

    public async Task<AdsResult> ShowRewarded(string placementId)
    {
#if UNITY_EDITOR
        return AdsResult.Complete;
#endif

        if (AdsBlocked) return AdsResult.Complete;
        if (!_adsSystem.IsRewardedAdsReady(placementId)) return AdsResult.NotLoaded;
        Debug.Log("Rewarded Ads");
        var result = await _adsSystem.ShowRewardedAds(placementId);
        _lastInterstitialTime = DateTime.UtcNow;
        OnShowRewarded?.Invoke(placementId);
        return result ? AdsResult.Complete : AdsResult.Canceled;
    }

    public async Task<AdsResult> ShowInterstitial(string placementId, int GameCount, GameObject AdBreaker)
    {
        if (AdsBlocked) return AdsResult.Complete;
        if (DateTime.UtcNow - _lastInterstitialTime < TimeSpan.FromSeconds(_interstitialCooltime) || GameCount < 3) return AdsResult.Canceled;
        if (!_adsSystem.IsInterstitialAdsReady(placementId)) return AdsResult.NotLoaded;

        _lastInterstitialTime = DateTime.UtcNow;
        AdBreaker.gameObject.SetActive(true);
        await Task.Delay(2000);
        OnShowInterstitial?.Invoke(placementId);
        await _adsSystem.ShowInterstitialAds(placementId);
        return AdsResult.Complete;
    }
}

public enum AdsResult
{
    NotLoaded, Canceled, Complete
}
