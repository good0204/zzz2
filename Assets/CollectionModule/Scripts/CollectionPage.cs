using System.Collections.Generic;
using System.Linq;
using Aloha.ItemSystem;
using DG.Tweening;
using UnityEngine;

public abstract class CollectionPage : MonoBehaviour
{
    [SerializeField] protected List<CollectionItem> _collectionItems = new List<CollectionItem>();
    [SerializeField] protected GameObject _purchaseButtons;

    public bool ContainsItem(IItem item)
    {
        return _collectionItems.Any(ci => ci.Item == item);
    }

    public abstract void Initialize(CollectionItemSelectionHandler collectionItemSelectionHandler,
        CollectionPurchaseController purchaseController);

    public void OnFocused()
    {
        _purchaseButtons.SetActive(true);
        _purchaseButtons.transform.localScale = Vector3.zero;
        _purchaseButtons.transform.DOScale(1, .2f).SetEase(Ease.OutQuad);
        OnFocusedCallback();
    }

    protected virtual void OnFocusedCallback() { }

    public void OnUnfocused()
    {
        _purchaseButtons.SetActive(false);
    }
}
