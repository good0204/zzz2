using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Firebase.Analytics;
using Aloha.Currency;
using Aloha.Save;
public class FireBase : SingletonComponent<FireBase>, ISaveable
{
    public enum CurrentState { Next, Restart, Continue }
    private CurrentState _currentState = CurrentState.Next;
    private int Game_Start = 1;
    private int Game_End = 1;
    public float playtime;
    public bool IsGameStart;
    public float Ts;
    string today;
    public int totalIn = 0;
    public int totalNotShowIn = 0;
    int totalCoinReward = 0;
    int totalCoinRewardx2 = 0;
    int totalTrailReward = 0;
    int totalContinueReward = 0;
    public int ToTalPlayCount;
    [SerializeField] SkinManager skinManager;
    [SerializeField] CurrencyManager currencyManager;
    [SerializeField] CurrencyType currencyType;
    [SerializeField] StageManager stageManager;
    [SerializeField] AreaManager areaManager;
    [SerializeField] BulletLauncher bulletLauncher;
    int StageResetCount;
    string advertisingId;
    public string Key { get { return "FireBase"; } }

    // Start is called before the first frame update
    public void CheangeEnum(string State)
    {
        if (State == "Next")
        {
            _currentState = CurrentState.Next;
        }
        else if (State == "Restart")
        {
            _currentState = CurrentState.Restart;
        }
        else
        {
            _currentState = CurrentState.Continue;
        }

    }
    protected override void Awake()
    {
        if (!SaveManager.Load(this))
        {
            Ts = 0;
            ToTalPlayCount = 0;
            totalCoinReward = 0;
            totalTrailReward = 0;
            totalCoinRewardx2 = 0;
        }
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");

        FirebaseAnalytics.SetSessionTimeoutDuration(TimeSpan.FromSeconds(60f));

        if (!PlayerPrefs.HasKey("first_version"))
        {
            PlayerPrefs.SetString("first_version", Application.version);
        }
        var firstVersion = PlayerPrefs.GetString("first_version", Application.version);
        FirebaseAnalytics.SetUserProperty("first_version", firstVersion);

        Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnabled, string error) =>
        {
            Debug.Log("advertisingId " + advertisingId + " " + trackingEnabled + " " + error);
            if (trackingEnabled)
            {
                FirebaseAnalytics.SetUserProperty("ad_id", advertisingId);
                this.advertisingId = advertisingId;
                Debug.Log("FirebaseEventLogger :: Ad id set");
            }
        });
    }
    private void Start()
    {
        // var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        // if (dependencyStatus == Firebase.DependencyStatus.Available)
        // {
        // }
        // else
        // {
        //     UnityEngine.Debug.LogError(System.String.Format(
        //         "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //     // Firebase Unity SDK is not safe to use here.
        // }

    }
    private void Update()
    {
        Ts += Time.deltaTime;
        if (IsGameStart)
        {
            playtime += Time.deltaTime;
        }
    }

    public void GameStart()
    {
        ToTalPlayCount++;
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        IsGameStart = true;
        Parameter[] GameStartParameters = {
            new Parameter("Game_Start",Game_Start),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()+1),
            new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()+1),
            new Parameter("Stage",stageManager.GetCurrentStage()+1),
            new Parameter("MapId",stageManager.GetCurrentStageId()),
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("coin_currency", currencyManager.GetAmount(currencyType)),
            new Parameter("date", today),
            new Parameter("Version", Application.version)
            };
        FirebaseAnalytics.LogEvent("play_start", GameStartParameters);
        SaveManager.Save(this);
        // string a = string.Format("EventName = play_start  LevelNum = {0} ,MapId  ={1} , play_count = {2} , coin_currency = {3} , Stage = {4}, LevelOrder = {5}", areaManager.GetCurrentAreaNum() + 1, stageManager.GetCurrentStageId(), ToTalPlayCount, currencyManager.GetAmount(currencyType), stageManager.GetCurrentStage() + 1, areaManager.GetCurrentAreaOrder() + 1);
        // Debug.Log(a);
    }
    private void OnDisable()
    {
        SaveManager.Save(this);
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveManager.Save(this);
        }
    }
    public void GameEnd(int Fail, int Clear)
    {
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        Parameter[] GameOverParameters = {
            new Parameter("date",today),
            new Parameter("play_count",ToTalPlayCount),
            new Parameter("play_time", playtime),
            new Parameter("play_ts", Ts),
            new Parameter("Game_End", Game_End),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()+1),
            new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()+1),
            new Parameter("IsBonus", areaManager.FireBaseBoolCheck()),
            new Parameter("Stage",stageManager.GetCurrentStage() + 1),
            new Parameter("MapId",stageManager.GetCurrentStageId()),
            new Parameter("StartType", _currentState.ToString()),
            new Parameter("remained_bullet",bulletLauncher.GetNumberofbullets()),
            new Parameter("Fail", Fail),
            new Parameter("Clear", Clear),
            new Parameter("TrailId", skinManager.CurrentTrailSkinId),
            new Parameter("GunId", skinManager.CurrentGunSkinId),
            new Parameter("Version", Application.version),
            new Parameter("BonusObejctCount", CountBonsObject.Instance.GetBonusObejctCount())
    };
        FirebaseAnalytics.LogEvent("play_end", GameOverParameters);
        // string a = string.Format("EventName = play_end TrailId = {0} ,GunId = {1} , play_count = {2} , play_time = {3} , Fail = {4} , Clear = {5} ,remained_bullet = {6}, StartType = {7} , MapId = {8} , LevelNum = {9} , Stage = {10}, IsBonus = {12}, BonusObjectCount = {13} "
        // , skinManager.CurrentTrailSkinId, skinManager.CurrentGunSkinId, ToTalPlayCount, playtime, Fail, Clear, bulletLauncher.GetNumberofbullets(), _currentState.ToString(), stageManager.GetCurrentStageId(), areaManager.GetCurrentAreaNum() + 1, stageManager.GetCurrentStage() + 1, areaManager.GetCurrentAreaOrder() + 1, areaManager.FireBaseBoolCheck(), CountBonsObject.Instance.GetBonusObejctCount());
        // Debug.Log(a);

        ReSet();
        SaveManager.Save(this);
    }
    public void GetCoin(int coin_play, int coin_x2, int coin_200)
    {
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        if (coin_play != 0)
        {
            Parameter[] GetCoinParameters = {
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()),
            new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()),
            new Parameter("coin_play",coin_play),
            new Parameter("coin_currency",currencyManager.GetAmount(currencyType)),
            new Parameter("date",today),
            new Parameter("Version", Application.version)

        };
            FirebaseAnalytics.LogEvent("coin_currency", GetCoinParameters);
        }
        else if (coin_x2 != 0)
        {
            Parameter[] GetCoinParameters = {
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()),
             new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()),
            new Parameter("coin_x2",coin_x2),
            new Parameter("coin_currency",currencyManager.GetAmount(currencyType)),
            new Parameter("date",today),
            new Parameter("Version", Application.version)
            };
            FirebaseAnalytics.LogEvent("coin_currency", GetCoinParameters);
            // string a = string.Format("EventName = coin_currency  AllClear = {0} ,  ={1} , play_count = {2} , coin_x2 = {3} , coin_currency = {4}"
            // , AllClear, , ToTalPlayCount, coin_x2, currencyManager.GetAmount(currencyType));
            //     Debug.Log(a);
        }
        else
        {
            Parameter[] GetCoinParameters = {
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()),
             new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()),
            new Parameter("coin_200",coin_200),
            new Parameter("coin_currency",currencyManager.GetAmount(currencyType)),
            new Parameter("date",today),
            new Parameter("Version", Application.version)
            };
            // FirebaseAnalytics.LogEvent("coin_currency", GetCoinParameters);
            // string a = string.Format("EventName = coin_currency  ,level  ={1} , play_count = {2} , coin_200 = {3} , coin_currency = {4}"
            // // , areaManager.GetCurrentAreaNum(), ToTalPlayCount, coin_200, currencyManager.GetAmount(currencyType));

            // Debug.Log(a);
        }

    }
    public void StageResetButtonClick()
    {
        StageResetCount++;
        Parameter[] ButtonClick =
        {
            new Parameter("LevelNum", areaManager.GetCurrentAreaNum() + 1),
             new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()+1),
            new Parameter("Stage", stageManager.GetCurrentStage() + 1),
             new Parameter("MapId", stageManager.GetCurrentStageId()),
            new Parameter("ButtonclickCount", StageResetCount),
        };
        FirebaseAnalytics.LogEvent("StageReset", ButtonClick);

        // string a = string.Format("EventName = StageReset  LevelNum = {0} , LevelOrder ={4} , Stage ={1} , MapId = {2} , ButtonclickCount = {3},"
        //    , areaManager.GetCurrentAreaNum() + 1, stageManager.GetCurrentStage() + 1, stageManager.GetCurrentStageId(), StageResetCount, areaManager.GetCurrentAreaOrder() + 1);
        // Debug.Log(a);
    }
    public void UseCoin(int Id, int use_amount)
    {
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        int useamount = use_amount - 20;
        Parameter[] UseCoinParameters = {
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("use_amount",useamount),
            new Parameter("coin_currency",currencyManager.GetAmount(currencyType)),
            new Parameter("date",today),
            new Parameter("Version", Application.version)
            };
        FirebaseAnalytics.LogEvent("coin_currency", UseCoinParameters);

        //     string a = string.Format("EventName = coin_currency  play_count = {0} , use_amount ={1} , coin_currency = {2} "
        //       , ToTalPlayCount, useamount, currencyManager.GetAmount(currencyType));
        //     Debug.Log(a);

    }
    public void Skin(int SkinId, int Buy_coin, int Buy_ads, int lose)
    {
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        Parameter[] GetSkinParameters = {
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("SkinId",SkinId),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()),
            new Parameter("Stage",stageManager.GetCurrentStage()),
            new Parameter("Buy_coin",Buy_coin),
            new Parameter("Buy_ads",Buy_ads),
            new Parameter("lose",lose),
            new Parameter("date",today),
            new Parameter("Version", Application.version)
    };
        FirebaseAnalytics.LogEvent("skin", GetSkinParameters);
        // string a = string.Format("EventName = skin  AllClear = {0} ,  ={1} , play_count = {2} , SkinName = {3} , Buy_coin = {4} , Buy_ads = {5} , Lose = {6}"
        // , AllClear, , ToTalPlayCount, SkinName, Buy_coin, Buy_ads, lose);
        //  Debug.Log(a);
    }
    public void Interstitial()
    {
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        totalIn++;
        Parameter[] InterstitialParameters = {
            new Parameter("date",today),
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("LevelNum",areaManager.GetCurrentAreaNum()),
                 new Parameter("LevelOrder", areaManager.GetCurrentAreaOrder()),
            new Parameter("Stage",stageManager.GetCurrentStage()),
            new Parameter("IS",1),
            new Parameter("IS_total",totalIn),
            new Parameter("Version", Application.version)
            };
        FirebaseAnalytics.LogEvent("ads_IS", InterstitialParameters);
        // string a = string.Format("EventName = ads_IS  play_count = {0} ,LevelNum = {1}  , LevelOrder = {4}, Stage = {2} , totalIn = {3} "
        //   , ToTalPlayCount, areaManager.GetCurrentAreaNum(), stageManager.GetCurrentStage() + 1, totalIn, areaManager.GetCurrentAreaOrder());
        // Debug.Log(a);
    }
    public void Reward(string RewardName)
    {
        today = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");
        int coinreward = 0;
        int trailreward = 0;
        int coinrewardx2 = 0;
        int conti = 0;

        if (RewardName == "trail")
        {
            totalTrailReward++;
            trailreward = 1;

            Parameter[] RewardParameters = {
            new Parameter("date",today),
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("skin",trailreward),
            new Parameter("RV_Skintotal",totalTrailReward),
            new Parameter("Re_total", totalTrailReward+totalCoinReward+totalCoinRewardx2),
             new Parameter("Version", Application.version)
            };
            FirebaseAnalytics.LogEvent("ads_RV", RewardParameters);

            // string a = string.Format("EventName = ads_RV  play_count = {0} , skin ={1} , RV_Skintotal = {2} , Re_total = {3}"
            // , ToTalPlayCount, trailreward, totalTrailReward, totalTrailReward + totalCoinReward + totalCoinRewardx2 + conti);
            // Debug.Log(a);
        }
        else if (RewardName == "coin200")
        {
            coinreward = 1;
            totalCoinReward++;
            Parameter[] RewardParameters = {
             new Parameter("date",today),
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("coin200",coinreward),
            new Parameter("RV_Cointotal",totalCoinReward),
            new Parameter("Version", Application.version),
            new Parameter("Re_total", totalTrailReward+totalCoinReward+totalCoinRewardx2)
            };
            FirebaseAnalytics.LogEvent("ads_RV", RewardParameters);

            // string a = string.Format("EventName = ads_RV  play_count = {0} , coin200 ={1} , RV_Cointotal = {2} , Re_total = {3}"
            //  , ToTalPlayCount, coinreward, totalCoinReward, totalTrailReward + totalCoinReward + totalCoinRewardx2 + conti);
            // Debug.Log(a);
        }
        else if (RewardName == "coinx2")
        {
            coinrewardx2 = 1;
            totalCoinRewardx2++;
            Parameter[] RewardParameters = {
            new Parameter("date",today),
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("coinx2",coinrewardx2),
            new Parameter("RV_Coinx2total",totalCoinRewardx2),
            new Parameter("Version", Application.version),
            new Parameter("Re_total", totalTrailReward+totalCoinReward+totalCoinRewardx2)
            };
            FirebaseAnalytics.LogEvent("ads_RV", RewardParameters);

            // string a = string.Format("EventName = ads_RV  play_count = {0} , coinx2 ={1} , RV_Coinx2total = {2} , Re_total = {3}"
            // , ToTalPlayCount, coinrewardx2, totalCoinRewardx2, totalTrailReward + totalCoinReward + totalCoinRewardx2 + conti);
            // Debug.Log(a);
        }
        else if (RewardName == "continue")
        {
            conti = 1;
            totalContinueReward++;
            Parameter[] RewardParameters = {
            new Parameter("date",today),
            new Parameter("play_count", ToTalPlayCount),
            new Parameter("continue",conti),
            new Parameter("RV_Continuetotal",totalContinueReward),
            new Parameter("Version", Application.version),
            new Parameter("Re_total", totalTrailReward+totalCoinReward+totalCoinRewardx2)
            };
            FirebaseAnalytics.LogEvent("ads_RV", RewardParameters);

            // string a = string.Format("EventName = ads_RV  play_count = {0} , continue ={1} , RV_Continuetotal = {2} , Re_total = {3}"
            // , ToTalPlayCount, conti, totalContinueReward, totalTrailReward + totalCoinReward + totalCoinRewardx2 + conti);
            // Debug.Log(a);
        }

    }
    public void CrossMarketing()
    {
        Parameter[] CorssMarketing = {
            new Parameter("target_app_id","kr.co.alohacorp.Aloha1010"),
            new Parameter("target_app_name", "Block Puzzle 1010!")
            };
        FirebaseAnalytics.LogEvent("cross", CorssMarketing);
    }

    public void ReSet()
    {
        playtime = 0;
        StageResetCount = 0;

        CountBonsObject.Instance.ReSet();
        IsGameStart = false;

    }
    public void ResetState()
    {
        _currentState = CurrentState.Next;
    }

    public void Load(Dictionary<string, object> Loadcomponent)
    {
        Ts = (float)Loadcomponent["TS"];
        totalIn = (int)Loadcomponent["totalIn"];
        totalNotShowIn = (int)Loadcomponent["totalNotShowIn"];
        totalCoinReward = (int)Loadcomponent["totalCoinReward"];
        totalTrailReward = (int)Loadcomponent["totalTrailReward"];
        ToTalPlayCount = (int)Loadcomponent["ToTalPlayCount"];
        totalCoinRewardx2 = (int)Loadcomponent["totalCoinRewardx2"];
        totalContinueReward = (int)Loadcomponent["totalContinueReward"];
    }
    public Dictionary<string, object> GetSaveData()
    {
        Dictionary<string, object> Save = new Dictionary<string, object>();
        Save["TS"] = Ts;
        Save["totalCoinReward"] = totalCoinReward;
        Save["totalTrailReward"] = totalTrailReward;
        Save["ToTalPlayCount"] = ToTalPlayCount;
        Save["totalCoinRewardx2"] = totalCoinRewardx2;
        Save["totalIn"] = totalIn;
        Save["totalNotShowIn"] = totalNotShowIn;
        Save["totalContinueReward"] = totalContinueReward;
        return Save;
    }

}

