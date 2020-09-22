using System;
using System.Collections.Generic;
using Aloha.Save;
using UniRx;

public static class DirtyMarkManager
{
    private const string ROOT = "root";
    private static DirtyMarkNode _rootNode = new DirtyMarkNode(ROOT);
    private static DirtyMarkSaveManager _dirtyMarkSaveManager;

    public static IReadOnlyReactiveProperty<bool> GetDirtyMark(string id)
    {
        id = id.ToLower();
        if (id.Equals(ROOT)) return _rootNode.Dirty;
        return _rootNode.SearchChild(id).Dirty;
    }

    public static void MarkDirty(string id)
    {   
        id = id.ToLower();
        _rootNode.SearchChild(id).MarkDirty();
    }

    public static void Clean(string id)
    {
        id = id.ToLower();
        _rootNode.SearchChild(id).Clean();
    }

//    private static void Initialize()
//    {
//        if (_dirtyMarkSaveManager == null)
//        {
//            _dirtyMarkSaveManager = new DirtyMarkSaveManager(_rootNode);
//            SaveManager.Load(_dirtyMarkSaveManager);
//            _rootNode.ChildNodeUpdated += () => SaveManager.Save(_dirtyMarkSaveManager);
//        }
//    }
}

public class DirtyMarkSaveManager: ISaveable
{
    private DirtyMarkNode _root;
    
    public DirtyMarkSaveManager(DirtyMarkNode rootNode)
    {
        _root = rootNode;
    }

    public string Key => "dirty_mark_save";

    public Dictionary<string, object> GetSaveData()
    {
        var saveData = new Dictionary<string, object>();
        foreach (var child in _root.Children)
        {
            AddSaveData(child, null, saveData);
        }
        return saveData;
    }

    private void AddSaveData(DirtyMarkNode node, string combinedId, Dictionary<string, object> saveData)
    {
        var combined = string.IsNullOrEmpty(combinedId) ? node.id: $"{combinedId}.{node.id}";
        if (node.IsLeafNode)
        {
            saveData.Add(combined, node.Dirty.Value);
        }
        else
        {
            foreach (var child in node.Children)
            {
                AddSaveData(child, combined, saveData);
            }
        }
    }

    public void Load(Dictionary<string, object> saveData)
    {
        foreach (var pair in saveData)
        {
            var child = _root.SearchChild(pair.Key);
            if(Convert.ToBoolean(pair.Value)) child.MarkDirty();
        }
    }
}