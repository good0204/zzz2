using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aloha.Save;
using Aloha.ItemSystem;
using UniRx;
using System;
using DG.Tweening;
using System.Linq;
public class SkinManager : MonoBehaviour, ISaveable
{
    public string Key => "Skin";
    [SerializeField] Equipment equipment;
    [SerializeField] ItemManager itemManager;
    [SerializeField] Inventory inventory;
    [SerializeField] TrailSkin trailSkin;
    [SerializeField] GunSkin gunSkin;
    [SerializeField] Material GunSkinmaterial;
    [SerializeField] TrailRenderer[] trailRenderers;
    [SerializeField] Transform CollectionPreview;
    int CurrentProgressSkinId;
    List<IItem> ProgressSkins = new List<IItem>();

    public int CurrentTrailSkinId;
    public int CurrentGunSkinId;
    public bool GetFirstTrail;

    private void Awake()
    {
        inventory.Initialize();
        GetProgressSkins();

        gunSkin.Initialize(GunSkinmaterial, itemManager);
        trailSkin.Initialize(trailRenderers, itemManager);
        SaveManager.Load(this);
        if (!SaveManager.Load(equipment))
        {
            inventory.Obtain(gunSkin.GetSkin(101));
            equipment.EquipItem("GunSkin", gunSkin.GetSkin(101));
            inventory.Obtain(trailSkin.GetSkin(201));
            equipment.EquipItem("Trail", trailSkin.GetSkin(201));
            CurrentProgressSkinId = 202;
        }
        equipment.GetReactiveItem("GunSkin").Subscribe(x =>
        {
            gunSkin.InitSkin(x.Id);
            CurrentGunSkinId = x.Id;
            SaveManager.Save(equipment);
        });
        equipment.GetReactiveItem("Trail").Subscribe(x =>
        {
            trailSkin.InitSkin(x.Id);
            CurrentTrailSkinId = x.Id;
            SaveManager.Save(equipment);
        });
        CollectionPreview.DOLocalRotate(new Vector3(0, 0, 360), 2.5f, RotateMode.FastBeyond360).SetAutoKill(false).SetEase(Ease.Linear)
                                    .SetLoops(-1);
    }
    public void ProgressGetTrailSkin()
    {
        for (int i = 0; i < ProgressSkins.Count; i++)
        {
            if (ProgressSkins[i].Id == CurrentProgressSkinId)
            {
                inventory.Obtain(trailSkin.GetSkin(CurrentProgressSkinId));
                equipment.EquipItem("Trail", trailSkin.GetSkin(CurrentProgressSkinId));
                FireBase.Instance.Skin(CurrentProgressSkinId - 1, 0, 1, 0);
            }
        }
        CurrentProgressSkinId++;
        SaveManager.Save(this);
    }
    public void GetFirstprogressSkin()
    {
        inventory.Obtain(trailSkin.GetSkin(202));
        equipment.EquipItem("Trail", trailSkin.GetSkin(202));
        GetFirstTrail = true;
        CurrentProgressSkinId++;
        SaveManager.Save(this);
    }
    public int SkinToGetThisTime()
    {
        for (int i = 0; i < ProgressSkins.Count; i++)
        {
            if (inventory.GetStock(ProgressSkins[i]) < 1)
            {

                if (ProgressSkins[i].Id == CurrentProgressSkinId)
                {
                    if (CurrentProgressSkinId > 208)
                    {
                        CurrentProgressSkinId = 202;
                    }
                    return ProgressSkins[i].Id;
                }
            }
        }
        return 0;
    }
    public bool CheckPurchaseButtons()
    {
        for (int i = 0; i < gunSkin._gun.Count; i++)
        {
            if (inventory.GetStock(gunSkin._gun[i]) < 1)
            {
                return true;
            }
        }
        return false;
    }
    public void LoseIt()
    {
        FireBase.Instance.Skin(CurrentProgressSkinId, 0, 0, 1);
        CurrentProgressSkinId++;

        SaveManager.Save(this);
    }
    void GetProgressSkins()
    {
        ProgressSkins = trailSkin.ProgressSkins();
    }
    public bool CheckAllGetProgressSkins()
    {
        for (int i = 0; i < ProgressSkins.Count; i++)
        {
            if (inventory.GetStock(ProgressSkins[i]) < 1)
            {
                return false;
            }
        }
        return true;
    }
    private void OnDisable()
    {
        //SaveManager.Save(this);
        //SaveManager.Save(equipment);
    }
    private void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus)
        {
            SaveManager.Save(this);
        }
    }
    public void Load(Dictionary<string, object> Loadcomponent)
    {
        CurrentGunSkinId = Convert.ToInt32(Loadcomponent["CurrentGunSkinId"]);
        CurrentTrailSkinId = Convert.ToInt32(Loadcomponent["CurrentTrailSkinId"]);
        CurrentProgressSkinId = Convert.ToInt32(Loadcomponent["CurrentProgressSkinId"]);
        GetFirstTrail = (bool)Loadcomponent["GetFirstTrail"];
    }

    public Dictionary<string, object> GetSaveData()
    {
        Dictionary<string, object> Save = new Dictionary<string, object>();
        Save["CurrentGunSkinId"] = CurrentGunSkinId;
        Save["CurrentTrailSkinId"] = CurrentTrailSkinId;
        Save["CurrentProgressSkinId"] = CurrentProgressSkinId;
        Save["GetFirstTrail"] = GetFirstTrail;
        return Save;

    }
}
