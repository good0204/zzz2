using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
public class AdController : MonoBehaviour
{
    public Action ActionTrailReward;
    public Action ActionContinueReward;
    public Action ActionCoinRewardx2;
    public Action NextArea;
    [SerializeField] GameObject interstitialImg;
    [SerializeField] AdsManager adsManager;
    [SerializeField] AreaManager areaManager;
    [SerializeField] RewardCoinEffect rewardCoinEffect;
    public async void GetCoinx2Reward()
    {
        var Isget = await adsManager.ShowRewarded("a0d12365b28947f8b8d4f8504995fb2f");
        if (Isget == AdsResult.Complete)
        {
            var effect = await rewardCoinEffect.AnimationPlay();
            ActionCoinRewardx2.Invoke();
            FireBase.Instance.Reward("coinx2");
        }

        PopupManager.Instance.Off();
        NextArea?.Invoke();

    }
    public async void GetContinueReward()
    {
        var Isget = await adsManager.ShowRewarded("a0d12365b28947f8b8d4f8504995fb2f");
        if (Isget == AdsResult.Complete)
        {
            FireBase.Instance.CheangeEnum("continue");
            FireBase.Instance.Reward("continue");
            FireBase.Instance.GameStart();
        }
        ActionContinueReward?.Invoke();
    }
    public async void GetTrailReward()
    {
        var Isget = await adsManager.ShowRewarded("a0d12365b28947f8b8d4f8504995fb2f");
        if (Isget == AdsResult.Complete)
        {
            ActionTrailReward?.Invoke();

            FireBase.Instance.Reward("trail");
        }
        NextArea?.Invoke();
    }

    public async void Interstitial()
    {
        var Isget = await adsManager.ShowInterstitial("2300c743d29746068d213155716a8f81", areaManager.GetCurrentAreaNum(), interstitialImg);
        if (Isget == AdsResult.Complete)
        {
            FireBase.Instance.Interstitial();
        }

        interstitialImg.SetActive(false);
    }
}
