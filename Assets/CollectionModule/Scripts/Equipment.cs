using System;
using System.Collections.Generic;
using System.Linq;
using Aloha.ItemSystem;
using Aloha.Save;
using UnityEngine;
using UniRx;
using Saveable = Aloha.Save.ISaveable;
using AlohaSaveManager = Aloha.Save.SaveManager;

[CreateAssetMenu(menuName = "Skin/Equipment")]
public class Equipment : ScriptableObject, Saveable
{
    [SerializeField] private ItemManager _itemManager;
    private Dictionary<string, ReactiveProperty<IItem>> _slots = new Dictionary<string, ReactiveProperty<IItem>>();
    private Dictionary<string, Type> _typeConstraints = new Dictionary<string, Type>();

    public void EquipItem(string slotName, IItem item)
    {
        if (_typeConstraints.ContainsKey(slotName))
        {
            if (item.GetType() != _typeConstraints[slotName])
            {
                Debug.LogError($"Equipment :: Equipment slot {slotName} requires type {_typeConstraints[slotName]}. Item {item.Name} not equipped.");
            }
        }

        if (!_slots.ContainsKey(slotName))
        {
            _slots.Add(slotName, new ReactiveProperty<IItem>());
            _slots[slotName].Skip(1).Subscribe(_ => AlohaSaveManager.Save(this));
        }

        _slots[slotName].Value = item;
    }

    public IReadOnlyReactiveProperty<IItem> GetReactiveItem(string slotName)
    {
        return _slots[slotName];
    }

    public IReadOnlyReactiveProperty<T> GetReactiveItem<T>(string slotName) where T : class, IItem
    {
        return _slots[slotName].Select(it => it as T).ToReadOnlyReactiveProperty();
    }

    public IItem GetItem(string slotName)
    {
        return _slots[slotName].Value;
    }

    public T GetItem<T>(string slotName) where T : class, IItem
    {
        return _slots[slotName].Value as T;
    }

    public void AddSlotTypeConstraint<T>(string slotName) where T : IItem
    {
        _typeConstraints[slotName] = typeof(T);
    }

    public bool IsSlotEmpty(string slotName)
    {
        return !_slots.ContainsKey(slotName) || _slots[slotName] == null;
    }


    #region ISaveable

    public string Key => "equipment";

    public Dictionary<string, object> GetSaveData()
    {
        var saveData = _slots.ToDictionary(pair => pair.Key, pair => (object)pair.Value.Value.Id);
        return saveData;
    }

    public void Load(Dictionary<string, object> saveData)
    {
        foreach (var pair in saveData)
        {
            EquipItem(pair.Key, _itemManager.GetItemById(Convert.ToInt32(pair.Value)));
        }
    }

    #endregion
}
