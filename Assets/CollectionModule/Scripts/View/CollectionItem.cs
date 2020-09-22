using System;
using Aloha.ItemSystem;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    public event Action OnSelected;
    public Toggle Toggle => _toggle;
    public IItem Item { get; private set; }

    [SerializeField] private Inventory _inventory;
    
    [SerializeField] private Toggle _toggle;
    
    [Header("Images")]
    [SerializeField] private Image _thumbnail;
    [SerializeField] private Image _outline;
    [SerializeField] private Image _background;
    [SerializeField] private Image _nonHolding;
    [SerializeField] private Sprite _initializedBackgroundSprite;
    
    [Header("Selection Images")]
    [SerializeField] private Sprite _selectedOutlineSprite;
    [SerializeField] private Sprite _unselectedOutlineSprite;

    [Header("Tag Fields")]
    [SerializeField] private Image _tagField;
    [SerializeField] private TextMeshProUGUI _tagText; 
    [SerializeField] private Sprite _normalTag;
    [SerializeField] private Sprite _rareTag;
    [SerializeField] private Sprite _legendaryTag;
    [SerializeField] private Sprite _tagSelected;

    void Awake()
    {
        _toggle.interactable = false;
        _thumbnail.enabled = false;
        _outline.enabled = false;
        _tagField.enabled = false;
        _tagText.enabled = false;
    }

    void Start()
    {
        _toggle.OnValueChangedAsObservable()
            .Subscribe(HandleToggle);
        
        HandleToggle(_toggle.isOn);
    }

    private void HandleToggle(bool v)
    {
        if (v) OnSelected?.Invoke();
        _toggle.interactable = !v;
//        _selectedBackground.enabled = v && _inventory.GetStock(Item) > 0;
        _outline.sprite = v ? _selectedOutlineSprite : _unselectedOutlineSprite;
        _tagField.sprite = v ? _tagSelected : _normalTag;
    }   

    public void Initialize(IItem item, bool previewable, IReadOnlyReactiveProperty<string> tagName)
    {
        _toggle.interactable = true;
        _thumbnail.enabled = true;
        _outline.enabled = true;
        _tagField.enabled = true;
        _tagText.enabled = true;
        _background.sprite = _initializedBackgroundSprite;
        
        Item = item;
        _thumbnail.sprite = item.Thumbnail;
        tagName.Subscribe(tag => _tagText.text = tag);

        _inventory.OnStockUpdated += (id, stock) =>
        {
            if (id == item.Id)
            {
                _nonHolding.enabled = stock <= 0;
            }
        };
        _nonHolding.enabled = _inventory.GetStock(item) <= 0;
    }

    public void Select()
    {
        _toggle.isOn = true;
    }
}
