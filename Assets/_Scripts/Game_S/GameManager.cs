using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] DrawLine drawLine;
    [SerializeField] BulletLauncher bulletLauncher;
    [SerializeField] MouseGameController mouseGameController;
    [SerializeField] StageManager stageManager;
    [SerializeField] AreaManager areaManager;
    [SerializeField] PopupManager popupManager;
    [SerializeField] ButtonController buttonController;
    [SerializeField] CameraController cameraController;
    [SerializeField] StageInfoText stageInfoText;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerTransform playerTransform;
    [SerializeField] CoinManager coinManager;
    [SerializeField] ObjectColor objectColor;
    [SerializeField] AdController adController;
    [SerializeField] SkinManager skinManager;
    [SerializeField] DebugMode debugMode;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        BineEvents();
    }
    void BineEvents()
    {
        mouseGameController.AdjectLine += drawLine.LineAdject;
        mouseGameController.AdjectLine += playerAnimation.PlayerRotation;
        mouseGameController.Firstposition += playerTransform.FirstTouchPosition;
        mouseGameController.Launch += bulletLauncher.Launch;
        mouseGameController.Launch += playerAnimation.ShotAnimation;

        mouseGameController.Bulletshine += stageInfoText.BulletShine;
        stageManager._canLaunch += mouseGameController.CanLaunch;
        stageManager._stageClear += bulletLauncher.MoveLaucher;
        stageManager._stageClear += playerAnimation.PlayerRun;
        playerAnimation._playerMove += playerTransform.PlayerMove;
        bulletLauncher._checkGameResult += stageManager.checkGameResult;
        stageManager.InjectbulletCount += bulletLauncher.Initailize;
        playerTransform._idle += playerAnimation.PlayerIdle;
        playerTransform._canLaunch += mouseGameController.CanLaunch;
        areaManager.ChangeObjectColor += objectColor.ChangeObjectColor;

        areaManager.Areaclear += bulletLauncher.CheckRemainingbullets;
        areaManager.Areaclear += playerAnimation.ClearEvent;
        areaManager.Areaclear += playerTransform.ClearTurn;
        areaManager.AreaNum += stageInfoText.ChangeAreaNum;
        playerTransform.SubResultcheck += bulletLauncher.CheckSubResult;

        mouseGameController.TopButtonsActive += buttonController.TopButtonsActive;
        //bulletLauncher.Followbullet += cameraController.FollowBullet;

        //팝업
        areaManager.BonusClear += popupManager.ClearPopup;
        playerAnimation.BonusCheck += areaManager.CheckBonusClear;


        playerAnimation.ClearPopup += popupManager.On;
        stageManager._failPopup += popupManager.On;
        areaManager.StartPopup += popupManager.On;

        //버튼
        buttonController.InputRetry += Reset;
        buttonController.InputRetry += stageManager.Retry;
        buttonController.InputRetry += areaManager.Retry;
        buttonController.InputRetry += adController.Interstitial;


        buttonController.Canlaunch += mouseGameController.CanLaunch;

        buttonController.InputNext += Reset;
        buttonController.InputNext += areaManager.NextArea;
        buttonController.InputNext += adController.Interstitial;

        buttonController.InputCoinx2 += adController.GetCoinx2Reward;
        buttonController.InputTrailGet += adController.GetTrailReward;
        buttonController.InputContinue += adController.GetContinueReward;

        buttonController.LoseIt += skinManager.LoseIt;

        buttonController.InputFirstTrailGet += skinManager.GetFirstprogressSkin;

        //Debug모드
        buttonController.DebugMode += debugMode.ChangeArea;
        buttonController.DebugMode += areaManager.AreaRestart;
        buttonController.DebugMode += Reset;


        //스테이지 리스타트
        buttonController.InputStageReset += stageManager.StageRestart;
        buttonController.InputStageReset += coinManager.StageReset;

        //에리어 리스타트
        areaManager.CanLaunch += mouseGameController.CanLaunch;
        buttonController.InputAreaReset += areaManager.AreaRestart;
        buttonController.InputAreaReset += Reset;
        buttonController.InputAreaReset += adController.Interstitial;

        //스테이지텍스트
        stageManager._changeStage += stageInfoText.ChangeStageText;
        areaManager.TotalStageOfArea += stageInfoText.ChageTotalStage;
        bulletLauncher._chageBulletCount += stageInfoText.ChangeBulletImage;
        bulletLauncher._changeBulletBG += stageInfoText.ChangeBGHeight;

        //continue광고
        adController.ActionContinueReward += stageManager.StageRestart;
        adController.ActionContinueReward += stageManager.RewardRestart;
        adController.ActionContinueReward += mouseGameController.StageContinue;
        adController.ActionContinueReward += coinManager.StageReset;

        //Trail광고 
        adController.ActionTrailReward += skinManager.ProgressGetTrailSkin;

        //코인2배광고
        adController.ActionCoinRewardx2 += coinManager.RewardCoinUp;

        // 다음스테이지
        adController.NextArea += Reset;
        adController.NextArea += areaManager.NextArea;
    }
    private void Reset()
    {
        coinManager.Reset();
        bulletLauncher.ReSet();
        playerTransform.ReSet();
        EffectManager.Instance.StopClearEffect();
        FireBase.Instance.ReSet();

        //StateMachine.Instance.ChangeState("Start");
    }
}
