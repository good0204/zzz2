using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EffectManager : SingletonComponent<EffectManager>
{
    [System.Serializable]
    public class Effects
    {
        public string Id;
        public List<ParticleSystem> _effects = new List<ParticleSystem>();
    }
    [SerializeField] List<Effects> effects;
    [SerializeField] Transform Parent;
    [SerializeField] ParticleSystem AreaClearEffect;
    [SerializeField] ParticleSystem[] StageClearEffect;
    [SerializeField] Text StageClearText;
    public ClearEffect clearEffect;
    private void Start()
    {
        clearEffect = new ClearEffect(AreaClearEffect, StageClearEffect, StageClearText);
    }
    public void StageClearEffectPlay()
    {
        clearEffect.StageClearEffectPlay();
    }
    public void AreaClearEffectPlay()
    {
        clearEffect.AreaClearEffectPlay();
    }
    public void StopClearEffect()
    {
        clearEffect.StopEffect();
    }
    public void EffectPlay(string Id, Vector3 position)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].Id == Id)
            {
                for (int j = 0; j < effects[i]._effects.Count; j++)
                    Instantiate(effects[i]._effects[j], position, Quaternion.Euler(0, 0, 0)).transform.SetParent(Parent);
            }
        }
    }
    public void EffectPlay(string Id, Vector3 position, Vector3 rotation, int bombnum)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].Id == Id)
            {
                Instantiate(effects[i]._effects[bombnum], position, Quaternion.Euler(rotation.x, rotation.y, rotation.z)).transform.SetParent(Parent);
            }
        }
    }
}
