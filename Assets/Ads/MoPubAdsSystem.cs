using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

public class MoPubAdsSystem : MonoBehaviour, IAdsSystem
{
    [SerializeField] private AdsManager _adsManager;

    private string ADUNIT_BANNER = "3b0c3dd70c1245619c8eaf1ca45c3ba2";
    private string ADUNIT_INTERSTITIAL = "2300c743d29746068d213155716a8f81";
    private string ADUNIT_REWARDED_REVIVE = "a0d12365b28947f8b8d4f8504995fb2f";

    private Subject<Unit> _onInterstitialShown = new Subject<Unit>();
    private Subject<Unit> _onRewardedVideoFailed = new Subject<Unit>();
    private Subject<Unit> _onRewardedVideoRewarded = new Subject<Unit>();

    public void Initialize(string adUnitId)
    {
        if (Application.installMode != ApplicationInstallMode.Store)
        {
            OverridePlacementsByTest();
        }
        //OverridePlacementsByTest();
        MoPub.LoadBannerPluginsForAdUnits(new[] { ADUNIT_BANNER });
        MoPub.LoadInterstitialPluginsForAdUnits(new[] { ADUNIT_INTERSTITIAL });
        MoPub.LoadRewardedVideoPluginsForAdUnits(new[] { ADUNIT_REWARDED_REVIVE });

        _adsManager.Initialize(this);

        SetupInterstitial();
        SetupRewarded();
    }

    private void OverridePlacementsByTest()
    {
        ADUNIT_BANNER = "b195f8dd8ded45fe847ad89ed1d016da";
        ADUNIT_INTERSTITIAL = "24534e1901884e398f1253216226017e";
        ADUNIT_REWARDED_REVIVE = "920b6145fb1546cf8b5cf2ac34638bb7";
    }

    private void SetupInterstitial()
    {
        MoPub.RequestInterstitialAd(ADUNIT_INTERSTITIAL);
        MoPubManager.OnInterstitialShownEvent += _ => MoPub.RequestInterstitialAd(ADUNIT_INTERSTITIAL);
        MoPubManager.OnInterstitialShownEvent += _ => _onInterstitialShown.OnNext(default);
    }

    private void SetupRewarded()
    {
        MoPub.RequestRewardedVideo(ADUNIT_REWARDED_REVIVE);
        MoPubManager.OnRewardedVideoClosedEvent += _ => MoPub.RequestRewardedVideo(ADUNIT_REWARDED_REVIVE);
        MoPubManager.OnRewardedVideoFailedEvent += (s, s1) => _onRewardedVideoFailed.OnNext(default);
        MoPubManager.OnRewardedVideoReceivedRewardEvent += (_, __, ___) => _onRewardedVideoRewarded.OnNext(default);
    }

    public void ShowBanner()
    {
        MoPub.RequestBanner(ADUNIT_BANNER, MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.ScreenWidthHeight90);
    }

    public void HideBanner()
    {
        MoPub.ShowBanner(ADUNIT_BANNER, false);
    }

    public bool IsRewardedAdsReady(string adId)
    {
        return MoPub.HasRewardedVideo(ADUNIT_REWARDED_REVIVE);
    }

    public async Task<bool> ShowRewardedAds(string adId)
    {
        MoPub.ShowRewardedVideo(ADUNIT_REWARDED_REVIVE);
        var taskCompletionSource = new TaskCompletionSource<bool>();

        var canceledObserver = _onRewardedVideoFailed
            .Subscribe(_ => taskCompletionSource.SetResult(false));
        var completeObserver = _onRewardedVideoRewarded
            .Subscribe(_ => taskCompletionSource.SetResult(true));

#if UNITY_EDITOR
        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ => _onRewardedVideoRewarded.OnNext(default));
#endif

        var result = await taskCompletionSource.Task;

        canceledObserver.Dispose();
        completeObserver.Dispose();

        return result;
    }

    public bool IsInterstitialAdsReady(string adId)
    {
        return MoPub.IsInterstitialReady(ADUNIT_INTERSTITIAL);
    }

    public async Task ShowInterstitialAds(string adId)
    {
        MoPub.ShowInterstitialAd(ADUNIT_INTERSTITIAL);

        var taskCompletionSource = new TaskCompletionSource<bool>();
#if UNITY_EDITOR
        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ => _onInterstitialShown.OnNext(Unit.Default));
#endif
        _onInterstitialShown.First().Subscribe(_ => taskCompletionSource.SetResult(true));

        await taskCompletionSource.Task;
    }
}
