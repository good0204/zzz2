using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aloha.ItemSystem;
using UniRx;

[CreateAssetMenu(menuName = "Skin/GunSkin")]
public class GunSkin : ScriptableObject
{
    [System.Serializable]
    public class Gun : IItem
    {
        public int Id => _id;
        public int Max => 1;
        public string Name => _name;
        public Sprite Thumbnail => _Thumbnail;
        [SerializeField] private string _name;
        [SerializeField] private int _id;
        [SerializeField] private Sprite _Thumbnail;
        public Texture texture;
    }
    public List<Gun> _gun = new List<Gun>();
    Material Gunmaterial;

    public void Initialize(Material Gunmaterial, ItemManager itemManager)
    {
        this.Gunmaterial = Gunmaterial;
        foreach (var gun in _gun)
        {
            itemManager.AddItem(gun);
        }
    }

    public IItem GetSkin(int Id)
    {
        for (int i = 0; i < _gun.Count; i++)
        {
            if (_gun[i].Id == Id)
            {
                return _gun[i];
            }
        }
        return null;
    }
    public void InitSkin(int Id)
    {
        for (int i = 0; i < _gun.Count; i++)
        {
            if (_gun[i].Id == Id)
            {
                Gunmaterial.mainTexture = _gun[i].texture;
            }
        }
    }

}
