using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class ButtonController : MonoBehaviour
{
    public Action InputRetry;
    public Action InputNext;
    public Action InputStageReset;
    public Action InputAreaReset;
    public Action<bool> Canlaunch;
    public Action InputCoinx2;
    public Action InputTrailGet;
    public Action InputContinue;
    public Action LoseIt;
    public Action InputFirstTrailGet;
    public Action DebugMode;

    [SerializeField] PopupManager popupManager;
    [SerializeField] AreaManager areaManager;
    [SerializeField] List<Button> NextButton = new List<Button>();
    [SerializeField] List<Button> ReTryButton = new List<Button>();
    [SerializeField] Button AreaResetButton;
    [SerializeField] Button StageResetButton;
    [SerializeField] Button Coinx2Button;
    [SerializeField] Button TrailGetButton;
    [SerializeField] Button ContinueButton;
    [SerializeField] Button FirstTrailButton;
    [SerializeField] Button DebugModeButton;
    [SerializeField] EventTrigger StartButton = null;

    void Start()
    {
        InitNextButton();
        InitRetryButton();
        InitStartButton();
        InitStageResetButton();
        InitAreaResetButton();
        InitCoinx2RewardButton();
        InitTrailRewardButton();
        InitContinueRewardButton();
        InitFirstTrailSKinButton();
        InitDebugModeButton();
    }
    void InitNextButton()
    {
        for (int i = 0; i < NextButton.Count; i++)
        {
            NextButton[i].onClick.AddListener(() => FireBase.Instance.CheangeEnum("Next"));
            NextButton[i].onClick.AddListener(() => popupManager.Off());
            NextButton[i].onClick.AddListener(() => InputNext?.Invoke());
            if (i == NextButton.Count - 1)
            {
                NextButton[i].onClick.AddListener(() => LoseIt?.Invoke());
            }

        }
    }
    void InitRetryButton()
    {
        for (int i = 0; i < ReTryButton.Count; i++)
        {
            ReTryButton[i].onClick.AddListener(() => FireBase.Instance.CheangeEnum("Restart"));
            ReTryButton[i].onClick.AddListener(() => popupManager.Off());
            ReTryButton[i].onClick.AddListener(() => InputRetry?.Invoke());
        }
    }
    void InitStartButton()
    {
        EventTrigger.Entry Start = new EventTrigger.Entry();
        Start.eventID = EventTriggerType.PointerDown;
        Start.callback.AddListener((x) =>
        {
            popupManager.Off();
            popupManager.On("InGamePopup");
            StateMachine.Instance.ChangeState("Stage1", null);
            Canlaunch?.Invoke(true);
            FireBase.Instance.GameStart();
        });
        StartButton.triggers.Add(Start);
    }
    void InitStageResetButton()
    {
        StageResetButton.onClick.AddListener(() => InputStageReset?.Invoke());
        StageResetButton.onClick.AddListener(() => FireBase.Instance.StageResetButtonClick());
    }
    void InitAreaResetButton()
    {
        AreaResetButton.onClick.AddListener(() => FireBase.Instance.CheangeEnum("Restart"));
        AreaResetButton.onClick.AddListener(() => InputAreaReset?.Invoke());
    }
    void InitCoinx2RewardButton()
    {
        //Coinx2Button.onClick.AddListener(() => popupManager.Off());
        Coinx2Button.onClick.AddListener(() => InputCoinx2?.Invoke());
    }
    void InitTrailRewardButton()
    {
        TrailGetButton.onClick.AddListener(() => popupManager.Off());
        TrailGetButton.onClick.AddListener(() => InputTrailGet?.Invoke());
    }
    void InitContinueRewardButton()
    {
        ContinueButton.onClick.AddListener(() => popupManager.Off());
        ContinueButton.onClick.AddListener(() => InputContinue?.Invoke());
    }
    void InitFirstTrailSKinButton()
    {
        FirstTrailButton.onClick.AddListener(() => popupManager.Off());
        FirstTrailButton.onClick.AddListener(() => InputNext?.Invoke());
        FirstTrailButton.onClick.AddListener(() => InputFirstTrailGet?.Invoke());
    }
    void InitDebugModeButton()
    {
        DebugModeButton.onClick.AddListener(() => DebugMode?.Invoke());
    }
    public void TopButtonsActive(bool Isactive)
    {
        if (!areaManager.Isbonus)
        {
            AreaResetButton.gameObject.SetActive(Isactive);
            StageResetButton.gameObject.SetActive(Isactive);
        }
    }



}
