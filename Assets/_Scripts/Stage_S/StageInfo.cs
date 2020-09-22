using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo : MonoBehaviour
{
    public int StageNum;
    public int StageId;
    public bool IsBonus = false;
    public int Numberofbullets;
    public List<TargetGimmick> targetGimmicks = null;

    public List<TargetGimmick> InitializeTarget()
    {
        return targetGimmicks;
    }
}
