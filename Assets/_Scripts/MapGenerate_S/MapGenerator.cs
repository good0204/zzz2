using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    float StageZ = 35f;
    List<StageInfo> Stages;
    List<StageInfo> StagesPrefab;
    public List<StageInfo> MapGenerate(List<StageInfo> stages)
    {
        StagesPrefab = new List<StageInfo>();
        Stages = new List<StageInfo>();
        StagesPrefab = stages;
        for (int i = 0; i < stages.Count; i++)
        {
            if (i == 0)
                Stages.Add(Instantiate(stages[i]));
            else
            {
                Stages.Add(Instantiate(stages[i], Stages[i - 1].transform.position + new Vector3(0, 0, StageZ), Quaternion.identity));
            }
        }
        return Stages;
    }
    public StageInfo MapGenerate(int StageNum, Vector3 pos)
    {
        if (StageNum == 0)
            return Stages[StageNum] = Instantiate(StagesPrefab[StageNum]);
        else
        {
            return Stages[StageNum] = Instantiate(StagesPrefab[StageNum], pos, Quaternion.identity);
        }

    }
}