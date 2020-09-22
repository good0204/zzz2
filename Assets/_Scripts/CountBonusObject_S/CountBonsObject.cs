using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountBonsObject : SingletonComponent<CountBonsObject>
{
    int BonusObejctCount;

    public void CountUp()
    {
        BonusObejctCount++;
    }
    public void ReSet()
    {
        BonusObejctCount = 0;
    }
    public int GetBonusObejctCount()
    {
        return BonusObejctCount;
    }
}
