using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusObject : TargetGimmick
{
    int bonuscoin = 50;
    private void Start()
    {
        base.IsBonus = true;
        base.BonusCoin = bonuscoin;
    }
}
