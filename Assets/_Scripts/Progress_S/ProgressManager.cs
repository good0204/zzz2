using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aloha.Save;
using Aloha.ItemSystem;
public class ProgressManager : MonoBehaviour, ISaveable
{

    public string Key => "Progress";
    float progresspercent;
    int GetedprogressSkinId;
    public bool IsFirst = true;


    private void Start()
    {
        if (!SaveManager.Load(this))
        {
            progresspercent = 0;
            IsFirst = true;
        }
    }
    public float ProgressUp()
    {
        if (IsFirst == true)
        {
            progresspercent += 25;
        }
        else
        {
            progresspercent += 20;
        }
        SaveManager.Save(this);
        if (progresspercent >= 100)
        {
            progresspercent = 0;
            IsFirst = false;
            return 100f;
        }
        return progresspercent;
    }
    public float CurrentProgress()
    {
        return progresspercent;
    }

    private void OnDisable()
    {
        SaveManager.Save(this);
    }
    public void Load(Dictionary<string, object> Loadcomponent)
    {
        progresspercent = (float)Loadcomponent["progresspercent"];
        IsFirst = (bool)Loadcomponent["IsFirst"];
    }
    public Dictionary<string, object> GetSaveData()
    {
        Dictionary<string, object> Save = new Dictionary<string, object>();
        Save["progresspercent"] = progresspercent;
        Save["IsFirst"] = IsFirst;
        return Save;
    }
}
