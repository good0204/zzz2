using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TargetHit : MonoBehaviour
{
    List<TargetGimmick> _targetGimmicks = new List<TargetGimmick>();
    public bool IsLastArea;
    int LastTargetCount;
    public void Initialize(List<TargetGimmick> target)
    {
        _targetGimmicks = target;
        LastTargetCount = 0;
        for (int i = 0; i < _targetGimmicks.Count; i++)
        {
            if (IsLastArea)
            {
                target[i].targetHit = this;
                target[i].IsLastAreaTarget = true;
            }
            else
            {
                target[i].IsLastAreaTarget = false;
            }
        }
    }
    public bool Clear(bool IsSub)
    {
        bool IsClear = true;
        for (int i = 0; i < _targetGimmicks.Count; i++)
        {
            if (_targetGimmicks[i].CheckClear() == false)
            {
                if (!IsSub)
                    _targetGimmicks[i].StartEmoji("Derision");
                IsClear = false;
            }
        }
        return IsClear;
    }
    public void LastTargetHit(Transform lastTarget)
    {
        if (LastTargetCount == _targetGimmicks.Count)
        {
            StateMachine.Instance.ChangeState("LastTarget", lastTarget);
        }
    }
    public bool CheckLastTarget()
    {
        LastTargetCount++;
        if (LastTargetCount == _targetGimmicks.Count)
        {
            return true;
        }
        return false;
    }
}
