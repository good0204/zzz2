using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    List<RecycleObject> pool = new List<RecycleObject>();
    int DefaultPoolSize = 0;
    RecycleObject prefab;

    public Factory(RecycleObject prefab, int DefaultPoolSize)
    {
        this.prefab = prefab;
        this.DefaultPoolSize = DefaultPoolSize;
    }
    void CreatPool()
    {
        for (int i = 0; i < DefaultPoolSize; i++)
        {
            RecycleObject obj = prefab as RecycleObject;
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }
    public RecycleObject Get()
    {
        if (pool.Count == 0)
        {
            CreatPool();
        }
        int lastIndex = pool.Count - 1;
        RecycleObject obj = pool[lastIndex];
        pool.RemoveAt(lastIndex);
        obj.gameObject.SetActive(true);
        return obj;
    }
    public void Restore(RecycleObject obj)
    {
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }

}
