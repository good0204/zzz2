using System;
using System.Collections.Generic;
using Aloha.ItemSystem;
using UniRx;
using UnityEngine;

public class CollectionPreviewer : MonoBehaviour
{
    public event Action<string, IItem> OnPreviewingItemChanged;

    private Dictionary<string, IItem> _previewingItems = new Dictionary<string, IItem>();
    [SerializeField] GameObject GunPreview;
    [SerializeField] BulletPreviewAnimation Trailpreview;
    private void Start()
    {
        GunPreview.SetActive(true);
    }
    public void RegisterPreviewSelectionHandler(string slot, CollectionItemSelectionHandler itemSelectionHandler)
    {
        itemSelectionHandler.PreviewingItem
            .Subscribe(item =>
            {
                SetPreviewingItem(slot, item);
                if (slot == "Trail")
                {
                    Trailpreview.EndAni();
                    Trailpreview.StartAni();
                }
            });
        itemSelectionHandler.PreviewAction += ChangePreview;
    }

    private void SetPreviewingItem(string slot, IItem item)
    {
        _previewingItems[slot] = item;
        OnPreviewingItemChanged?.Invoke(slot, item);

    }
    private void ChangePreview(int Id)
    {
        if (Id > 100 && Id < 110)
        {
            Trailpreview.gameObject.SetActive(false);
            GunPreview.SetActive(true);
        }
        else if (Id > 200 && Id < 210)
        {

            Trailpreview.gameObject.SetActive(true);
            GunPreview.SetActive(false);
        }
    }
}
