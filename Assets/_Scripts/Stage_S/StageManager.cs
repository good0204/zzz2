using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
public class StageManager : MonoBehaviour
{
    public Action _areaClear;
    public Action<bool> _canLaunch;
    public Action<int> _stageClear;
    public Action<int> _changeStage;
    public Action<string> _failPopup;
    public Action<int> InjectbulletCount;
    public StageInfo CurrentStage;
    StageInfo NextStage;
    List<TargetHit> targetHits = null;
    List<StageInfo> StagesinArea = new List<StageInfo>();
    [HideInInspector] public MapGenerator mapGenerator;

    public void Initiallie(List<StageInfo> Stages)
    {
        targetHits = new List<TargetHit>();
        this.StagesinArea = Stages;
        this.CurrentStage = StagesinArea[0];
        for (int i = 0; i < StagesinArea.Count; i++)
        {
            targetHits.Add(StagesinArea[i].GetComponent<TargetHit>());
        }
        targetHits[targetHits.Count - 1].IsLastArea = true;
        targetHits[CurrentStage.StageNum].Initialize(CurrentStage.InitializeTarget());
        StageStart();
    }
    public int GetCurrentStage()
    {
        return CurrentStage.StageNum;
    }
    public int GetCurrentStageId()
    {
        return CurrentStage.StageId;
    }
    public void StageStart()
    {
        InjectbulletCount?.Invoke(CurrentStage.Numberofbullets);
        _changeStage?.Invoke(CurrentStage.StageNum);
        if (CurrentStage.StageNum != StagesinArea.Count - 1)
        {
            NextStage = StagesinArea[CurrentStage.StageNum + 1];
        }

    }
    void NextStageStart()
    {
        CurrentStage = NextStage;
        targetHits[CurrentStage.StageNum].Initialize(CurrentStage.InitializeTarget());
        StageStart();

        FireBase.Instance.GameStart();
    }
    public void RewardRestart()
    {
        InjectbulletCount?.Invoke(CurrentStage.Numberofbullets + 1);
    }
    public void Reset()
    {
        for (int i = 0; i < StagesinArea.Count; i++)
        {
            Destroy(StagesinArea[i].gameObject);
        }
    }
    public void Retry()
    {
        Reset();
        StateMachine.Instance.ChangeState("Start", null);
    }
    public void checkGameResult(bool _checkGameFail, bool IsSub)
    {

        bool Isclear = targetHits[CurrentStage.StageNum].Clear(IsSub);
        if (Isclear)
        {
            if (!IsSub)
            {
                CoinManager.Instance.TempStageCoinSum();
            }
            _canLaunch?.Invoke(false);
            if (CurrentStage.StageNum == StagesinArea.Count - 1)
            {
                FireBase.Instance.GameEnd(0, 1);
                _areaClear?.Invoke();
                FireBase.Instance.ResetState();
            }
            else
            {
                FireBase.Instance.GameEnd(0, 1);
                SoundManager.Instance.Play("Gauge");
                EffectManager.Instance.StageClearEffectPlay();
                _stageClear?.Invoke((NextStage.StageNum + 1));
                NextStageStart();
            }
            return;
        }
        else if (_checkGameFail && !Isclear)
        {
            _canLaunch?.Invoke(false);
            if (CurrentStage.IsBonus)
            {
                FireBase.Instance.GameEnd(0, 1);
                _areaClear?.Invoke();
            }
            else
            {
                _failPopup?.Invoke("FailPopup");
                FireBase.Instance.GameEnd(1, 0);
            }
            return;
        }
        _canLaunch?.Invoke(true);
    }
    public void StageRestart()
    {
        int stagenum = CurrentStage.StageNum;

        Vector3 CurrentStagePosition = CurrentStage.transform.position;
        Destroy(CurrentStage.gameObject);

        StagesinArea[stagenum] = mapGenerator.MapGenerate(stagenum, CurrentStagePosition);
        CurrentStage = StagesinArea[stagenum];
        targetHits[stagenum] = StagesinArea[CurrentStage.StageNum].GetComponent<TargetHit>();
        if (StagesinArea.Count - 1 == stagenum)
        {
            targetHits[stagenum].IsLastArea = true;
        }
        targetHits[stagenum].Initialize(CurrentStage.InitializeTarget());
    }
}
