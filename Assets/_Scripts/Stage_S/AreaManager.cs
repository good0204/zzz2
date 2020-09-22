using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Aloha.Save;
using System.Linq;
public class AreaManager : MonoBehaviour, ISaveable
{
    public Action<string> StartPopup;
    public Action<int> ChangeObjectColor;
    public Action<int> TotalStageOfArea;
    public Action<bool, int> AreaNum;
    public Action<bool> BonusClear;
    public Action<bool> CanLaunch;
    public Action Areaclear;

    [SerializeField] List<AreaInfo> areaInfos = null;
    [SerializeField] List<AreaInfo> bonusAreaInfos = null;
    public List<StageInfo> RandomStagesInfos = new List<StageInfo>();
    public List<StageInfo> RandomBonusStagesInfos = new List<StageInfo>();
    [SerializeField] StageManager stageManager;
    AreaInfo CurrentArea;
    MapGenerator mapGenerator = null;
    [SerializeField] List<int> SaveRandomIndex = new List<int>() { 0, 0, 0, 0 };

    [SerializeField] int ClearedTotalAreaCount;
    [SerializeField] int ClearedBonusCount;
    [SerializeField] int ClearedStandCount;
    [SerializeField] int TotalArea;
    public string Key => "AreaManager";
    public bool Isbonus;
    private void Start()
    {
        SaveManager.Load(this);

        TotalArea = (areaInfos.Count + bonusAreaInfos.Count) - 1;

        BonusAreaCheck();
        for (int i = 0; i < bonusAreaInfos.Count; i++)
        {
            for (int j = 0; j < bonusAreaInfos[i].StageInfos.Count; j++)
                bonusAreaInfos[i].StageInfos[j].IsBonus = true;
        }
        for (int i = 0; i < bonusAreaInfos.Count; i++)
        {
            RandomBonusStagesInfos[i].IsBonus = true;
        }

        if (ClearedTotalAreaCount < TotalArea)
        {
            if (!Isbonus)
                CurrentArea = areaInfos[ClearedStandCount];
            else
                CurrentArea = bonusAreaInfos[ClearedBonusCount];
        }
        else
        {
            LoadRandomStage();
        }

        mapGenerator = gameObject.AddComponent<MapGenerator>();
        stageManager.mapGenerator = mapGenerator;
        StartArea();
        BindEvent();
    }
    void BindEvent()
    {
        stageManager._areaClear += AreaClear;
    }
    public int GetCurrentAreaNum()
    {
        return CurrentArea.AreaNumber;
    }
    public int GetCurrentAreaOrder()
    {
        return ClearedTotalAreaCount;
    }
    void BonusAreaCheck()
    {
        Isbonus = (ClearedTotalAreaCount + 1) % 4 == 0 ? true : false;
    }
    public int FireBaseBoolCheck()
    {
        return Isbonus ? 1 : 0;
    }
    public void StartArea()
    {
        StartPopup?.Invoke("StartPopup");
        List<StageInfo> Stages = new List<StageInfo>();
        for (int i = 0; i < CurrentArea.StageInfos.Count; i++)
        {
            Stages.Add(CurrentArea.StageInfos[i]);
        }
        stageManager.Initiallie(mapGenerator.MapGenerate(Stages));
        TotalStageOfArea?.Invoke(CurrentArea.StageInfos.Count);

        if (!Isbonus)
        {
            AreaNum?.Invoke(false, ClearedStandCount);
        }
        else
        {
            AreaNum?.Invoke(true, ClearedBonusCount);
        }
        ChangeObjectColor?.Invoke(ClearedTotalAreaCount % 4);

    }
    public void AreaClear()
    {
        ClearedTotalAreaCount++;

        if (!Isbonus)
        {
            ClearedStandCount++;
        }
        else
        {
            ClearedBonusCount++;
        }
        SaveManager.Save(this);
        Areaclear?.Invoke();
        EffectManager.Instance.AreaClearEffectPlay();
        SoundManager.Instance.Play("crowdCheer");
        CoinManager.Instance.ClearCoinUp();
    }
    public void CheckBonusClear()
    {
        if (Isbonus)
        {
            BonusClear?.Invoke(true);
        }
        else
        {
            BonusClear?.Invoke(false);
        }
    }
    public void NextArea()
    {
        stageManager.Reset();
        SelectArea();
        StartArea();
    }
    public void AreaRestart()
    {
        CanLaunch?.Invoke(false);
        stageManager.Reset();
        StartArea();
        StateMachine.Instance.ChangeState("Start", null);
    }

    public void Retry()
    {
        StartArea();
    }

    void SelectArea()
    {
        BonusAreaCheck();

        if (ClearedTotalAreaCount < TotalArea)
        {
            if (!Isbonus)
                CurrentArea = areaInfos[ClearedStandCount];
            else
                CurrentArea = bonusAreaInfos[ClearedBonusCount];
        }
        else
        {
            AreaInfo tempAreaInfo = new AreaInfo();
            List<int> Randomindexlist = new List<int>();
            int index = 0;
            if (!Isbonus)
            {
                for (int i = 0; i < 4; i++)
                {
                    index = UnityEngine.Random.Range(6, RandomStagesInfos.Count);
                    StageInfo TempStage = RandomStagesInfos[index];
                    if (tempAreaInfo.StageInfos.Contains(TempStage))
                    {
                        while (tempAreaInfo.StageInfos.Contains(TempStage))
                        {
                            index = UnityEngine.Random.Range(0, RandomStagesInfos.Count);
                            TempStage = RandomStagesInfos[UnityEngine.Random.Range(0, RandomStagesInfos.Count)];
                        }
                    }
                    TempStage.StageNum = i;
                    tempAreaInfo.AreaNumber = ClearedStandCount;
                    tempAreaInfo.StageInfos.Add(TempStage);
                    Randomindexlist.Add(index);
                }
            }
            else
            {
                index = UnityEngine.Random.Range(0, RandomBonusStagesInfos.Count);
                StageInfo TempStage = RandomBonusStagesInfos[index];
                TempStage.StageNum = 0;
                tempAreaInfo.AreaNumber = ClearedBonusCount;
                tempAreaInfo.StageInfos.Add(TempStage);
                Randomindexlist.Add(index);
            }
            CurrentArea = tempAreaInfo;
            SaveRandomIndex = Randomindexlist;
        }
        SaveManager.Save(this);
    }
    public void DebugModeChangeArea(int ClearedStandCount)
    {
        this.ClearedStandCount = ClearedStandCount;
        if (!Isbonus)
            CurrentArea = areaInfos[ClearedStandCount];
        else
            CurrentArea = bonusAreaInfos[ClearedBonusCount];
    }
    void LoadRandomStage()
    {
        AreaInfo SaveAreaInfo = new AreaInfo();
        if (!Isbonus)
        {
            for (int i = 0; i < 4; i++)
            {
                StageInfo SaveStage = RandomStagesInfos[SaveRandomIndex[i]];
                if (SaveAreaInfo.StageInfos.Contains(SaveStage))
                {
                    while (SaveAreaInfo.StageInfos.Contains(SaveStage))
                    {
                        SaveStage = RandomStagesInfos[UnityEngine.Random.Range(0, RandomStagesInfos.Count)];
                    }
                }
                SaveStage.StageNum = i;
                SaveAreaInfo.AreaNumber = ClearedStandCount;
                SaveAreaInfo.StageInfos.Add(SaveStage);
            }
        }
        else
        {
            StageInfo SaveStage = RandomBonusStagesInfos[SaveRandomIndex[0]];
            SaveStage.StageNum = 0;
            SaveAreaInfo.AreaNumber = ClearedBonusCount;
            SaveAreaInfo.StageInfos.Add(SaveStage);
        }
        CurrentArea = SaveAreaInfo;
    }
    private void OnDisable()
    {
        SaveManager.Save(this);
    }
    public void Load(Dictionary<string, object> Loadcomponent)
    {
        ClearedTotalAreaCount = Convert.ToInt32(Loadcomponent["ClearedTotalAreaCount"]);
        ClearedBonusCount = Convert.ToInt32(Loadcomponent["ClearedBonusCount"]);
        ClearedStandCount = Convert.ToInt32(Loadcomponent["ClearedStandCount"]);
        SaveRandomIndex = new List<int>(Loadcomponent["SaveRandomIndex"] as List<int>);
    }
    public Dictionary<string, object> GetSaveData()
    {
        Dictionary<string, object> Save = new Dictionary<string, object>();
        Save["ClearedTotalAreaCount"] = ClearedTotalAreaCount;
        Save["ClearedBonusCount"] = ClearedBonusCount;
        Save["ClearedStandCount"] = ClearedStandCount;
        Save["SaveRandomIndex"] = SaveRandomIndex;
        return Save;
    }
}
