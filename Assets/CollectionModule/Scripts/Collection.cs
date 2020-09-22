using System.Collections.Generic;
using Aloha.Currency;
using Aloha.ShopSystem;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class Collection : MonoBehaviour
{
    [SerializeField] private CollectionPreviewer _collectionPreviewer;
    [SerializeField] private Equipment _equipment;

    [SerializeField] private List<CollectionTab> _collectionTabs;

    private static Collection _instance;

    public static void OpenInstance()
    {
        _instance.Open();
    }

    public static void CloseInstance()
    {
        _instance.Close();
    }

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    void Start()
    {
        foreach (var collectionTab in _collectionTabs)
        {
            collectionTab.Initialize(_equipment, _collectionPreviewer);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
