using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aloha.ItemSystem;
using UniRx;
using System.Linq;
[CreateAssetMenu(menuName = "Skin/TrailSkin")]
public class TrailSkin : ScriptableObject
{
    [System.Serializable]
    public class Trail : IItem
    {
        public int Id => _id;
        public int Max => 1;
        public string Name => _name;
        public Sprite Thumbnail => _Thumbnail;
        [SerializeField] private string _name;
        [SerializeField] private int _id;
        [SerializeField] private Sprite _Thumbnail;
        public Gradient brandmode;
    }
    [SerializeField] List<Trail> _trail = new List<Trail>();
    TrailRenderer[] trailRenderers;
    public void Initialize(TrailRenderer[] trailRenderer, ItemManager itemManager)
    {
        this.trailRenderers = trailRenderer;

        foreach (var trail in _trail)
        {
            itemManager.AddItem(trail);
        }

    }
    public IItem GetSkin(int Id)
    {
        for (int i = 0; i < _trail.Count; i++)
        {
            if (_trail[i].Id == Id)
            {
                return _trail[i];
            }
        }
        return null;
    }
    public List<IItem> ProgressSkins()
    {
        return _trail.Select(t => t as IItem).ToList();
    }
    public List<Trail> ProgressSkins2()
    {
        return _trail;
    }
    public void InitSkin(int Id)
    {
        for (int i = 0; i < _trail.Count; i++)
        {
            if (_trail[i].Id == Id)
            {
                for (int j = 0; j < trailRenderers.Length; j++)
                    trailRenderers[j].colorGradient = _trail[i].brandmode;
            }
        }
    }
}
