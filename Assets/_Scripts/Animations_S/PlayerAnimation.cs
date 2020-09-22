using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAnimation : MonoBehaviour
{
    float theta;
    public Action<int> _playerMove;
    public Action<string> ClearPopup;
    public Action BonusCheck;
    [SerializeField] Animator playeranimator;
    int CurrentStage;
    public void ShotAnimation(Vector3[] f, int direction, float radian)
    {
        playeranimator.SetTrigger("Shot");
    }
    public void PlayerRotation(float PointX)
    {
        theta = Mathf.LerpAngle(transform.eulerAngles.y, PointX * 300, Time.deltaTime * 50);
        if (theta >= 45f && theta <= 180)
        {
            theta = 45f;
        }
        else if (theta >= 180 && theta <= 315)
        {
            theta = 315f;
        }
        transform.eulerAngles = new Vector3(0, theta, 0);
    }
    public void PlayerRun(int Stage)
    {
        CurrentStage = Stage;
        playeranimator.SetTrigger("Run");
        RunEvent();
    }
    public void PlayerIdle()
    {
        playeranimator.SetTrigger("Idle");
    }
    public void RunEvent()
    {
        _playerMove?.Invoke(CurrentStage);
    }
    public void ClearEvent()
    {
        StateMachine.Instance.ChangeState("Clear", null);

        playeranimator.SetTrigger("Clear" + UnityEngine.Random.Range(1, 5));
    }
    public void ClearAnimationEnd()
    {
        ClearPopup?.Invoke("ClearPopup");
        StateMachine.Instance.ChangeState("Start", null);
        BonusCheck?.Invoke();
        PlayerIdle();
    }

}
