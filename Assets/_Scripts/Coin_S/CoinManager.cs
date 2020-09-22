using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aloha.Currency;
using UniRx;
using UnityEngine.UI;
public class CoinManager : SingletonComponent<CoinManager>
{
    [SerializeField] CurrencyManager currencyManager;
    [SerializeField] CurrencyType currencyType;
    [SerializeField] Text ResultCoinText;
    [SerializeField] Text ResultRewardCoinText;
    [SerializeField] List<Text> CoinText = new List<Text>();
    [SerializeField] int TempStandardCoin;
    [SerializeField] int TempStageStandardCoin;
    [SerializeField] int TempStageExternalCoin;
    [SerializeField] int TempExternalCoin;
    ReactiveProperty<int> TotalTempCoin = new ReactiveProperty<int>();
    protected override void Awake()
    {
        currencyManager.Initialize();
    }
    void Start()
    {
        InitCoinText();
        TotalTempCoin.Subscribe(x =>
        {
            ResultCoinText.text = x.ToString();
        });

        currencyManager.Initialize();
    }
    public void StageStandardCoinUp(int Coin)
    {
        TempStageStandardCoin += Coin;
    }
    public void StageExternalCoinUp(int Coin)
    {
        TempStageExternalCoin += Coin;
    }
    public void TempStageCoinSum()
    {
        TempStandardCoin += TempStageStandardCoin;
        TempExternalCoin += TempStageExternalCoin;
        StageReset();
    }
    public void ClearCoinUp()
    {
        TotalTempCoin.Value = TempStandardCoin + TempExternalCoin;
        ResultRewardCoinText.text = ((TempStandardCoin + TempExternalCoin) * 2).ToString();
        currencyManager.Gain(currencyType, TempStandardCoin + TempExternalCoin, "coin");
        FireBase.Instance.GetCoin(TotalTempCoin.Value, 0, 0);
    }
    public void RewardCoinUp()
    {
        currencyManager.Gain(currencyType, TempStandardCoin + TempExternalCoin, "coin");
        FireBase.Instance.GetCoin(0, TempStandardCoin + TempExternalCoin, 0);

    }
    public void StageReset()
    {
        TempStageStandardCoin = 0;
        TempStageExternalCoin = 0;
    }
    public void Reset()
    {
        StageReset();
        TempExternalCoin = 0;
        TempStandardCoin = 0;
        InitCoinText();
    }
    public void InitCoinText()
    {
        for (int i = 0; i < CoinText.Count; i++)
        {
            CoinText[i].text = currencyManager.GetAmount(currencyType).ToString();
        }
    }

}
