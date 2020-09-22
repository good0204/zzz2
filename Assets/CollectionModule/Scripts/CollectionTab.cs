using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aloha.ItemSystem;
using Aloha.ShopSystem;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CollectionTab : MonoBehaviour
{
    public CollectionItemSelectionHandler CollectionItemSelectionHandler { get; private set; }
    public CollectionPurchaseController CollectionPurchaseController { get; private set; }

    [SerializeField] private string _equipmentSlot;
    [SerializeField] private Inventory _inventory;

    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private HorizontalScrollSnap _scrollSnap;
    [SerializeField] private List<Toggle> _paginations;
    [SerializeField] private List<CollectionPage> _collectionPages;

    private Equipment _equipment;

    public void Initialize(Equipment equipment, CollectionPreviewer collectionPreviewer)
    {
        _equipment = equipment;

        CollectionItemSelectionHandler = new CollectionItemSelectionHandler(_inventory, _toggleGroup,
            _equipmentSlot, equipment, collectionPreviewer);
        CollectionPurchaseController = new CollectionPurchaseController();

        var idx = 0;
        foreach (var collectionPage in _collectionPages)
        {
            var p = idx;
            _scrollSnap.AddChild(collectionPage.gameObject);
            _scrollSnap.OnSelectionPageChangedEvent.AddListener((page) =>
            {
                if (_equipmentSlot != "Trail")
                    if (page == p)
                    {
                        collectionPage.OnFocused();
                    }
                    else collectionPage.OnUnfocused();
            });
            collectionPage.Initialize(CollectionItemSelectionHandler, CollectionPurchaseController);
            idx++;
        }

        var equippedItem = _equipment.GetItem(_equipmentSlot);
        //CollectionItemSelectionHandler.SelectItem(equippedItem);

        for (int i = 0; i < _paginations.Count; i++)
        {
            _paginations[i].gameObject.SetActive(i < _collectionPages.Count);
        }
    }

    void OnEnable()
    {
        if (CollectionItemSelectionHandler == null) return;

        var equipped = _equipment.GetItem(_equipmentSlot);
        CollectionItemSelectionHandler.SelectItem(equipped);

        for (int i = 0; i < _collectionPages.Count; i++)
        {
            if (_collectionPages[i].ContainsItem(equipped))
            {
                _scrollSnap.ChangePage(i);
                break;
            }
        }
    }
}
