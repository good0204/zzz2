using System.Collections.Generic;
using Aloha.ItemSystem;
using UniRx;
using UnityEngine.UI;
using System;
public class CollectionItemSelectionHandler
{
    public IReadOnlyReactiveProperty<IItem> PreviewingItem => _previewingItem;
    public IReadOnlyReactiveProperty<CollectionItem> SelectedCollectionItem => _selectedCollectionItem;
    public Action<int> PreviewAction;

    private ReactiveProperty<IItem> _equippedItem = new ReactiveProperty<IItem>();
    private ReactiveProperty<IItem> _previewingItem = new ReactiveProperty<IItem>();
    private ReactiveProperty<CollectionItem> _selectedCollectionItem = new ReactiveProperty<CollectionItem>();

    private Inventory _inventory;
    private ToggleGroup _toggleGroup;

    private List<CollectionItem> _collectionItems = new List<CollectionItem>();
    private Equipment _equipment;

    public CollectionItemSelectionHandler(Inventory inventory, ToggleGroup toggleGroup,
        string equipmentSlot, Equipment equipment, CollectionPreviewer collectionPreviewer)
    {
        _inventory = inventory;
        _toggleGroup = toggleGroup;
        _toggleGroup.allowSwitchOff = false;
        _equipment = equipment;

        _equippedItem
            .Skip(1)
            .Subscribe(item => equipment.EquipItem(equipmentSlot, item));

        collectionPreviewer.RegisterPreviewSelectionHandler(equipmentSlot, this);
    }

    public void RegisterItemSelector(CollectionItem collectionItem)
    {
        _toggleGroup.RegisterToggle(collectionItem.Toggle);
        collectionItem.OnSelected += () =>
        {
            _toggleGroup.NotifyToggleOn(collectionItem.Toggle);
            if (_inventory.GetStock(collectionItem.Item) > 0)
            {
                _equippedItem.Value = collectionItem.Item;
            }
            _previewingItem.Value = collectionItem.Item;
            _selectedCollectionItem.Value = collectionItem;

        };
        collectionItem.Toggle.isOn = _equippedItem.Value == collectionItem.Item;
        _collectionItems.Add(collectionItem);
    }

    public void SelectItem(IItem item)
    {
        var collectionItem = _collectionItems.Find(i => i.Item == item);
        PreviewAction?.Invoke(item.Id);
        if (collectionItem != null)
        {
            collectionItem.Toggle.isOn = true;
        }
    }


}
