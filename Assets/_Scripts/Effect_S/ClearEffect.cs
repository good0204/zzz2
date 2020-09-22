using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ClearEffect
{
    ParticleSystem AreaClearEffect;
    ParticleSystem[] StageClearEffect;
    Text StageClearText;
    Sequence TextSequence;
    string[] ClearString = new string[] { "Phenomenal!", "Sick!", "Fantastic!", "Marvelous!", "Awesome!", "Incredible!" };

    public ClearEffect(ParticleSystem AreaClearEffet, ParticleSystem[] StageClearEffect, Text StageClearText)
    {
        this.StageClearText = StageClearText;
        this.AreaClearEffect = AreaClearEffet;
        this.StageClearEffect = StageClearEffect;
    }
    public void StageClearEffectPlay()
    {
        for (int i = 0; i < StageClearEffect.Length; i++)
        {
            StageClearEffect[i].gameObject.SetActive(true);
        }
        StageClearText.text = ClearString[Random.Range(0, ClearString.Length)];
        TextSequence = DOTween.Sequence();
        TextSequence.Append(StageClearText.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack));
        TextSequence.AppendInterval(0.5f);
        TextSequence.Append(StageClearText.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack));
    }
    public void AreaClearEffectPlay()
    {
        AreaClearEffect.gameObject.SetActive(true);
    }
    public void StopEffect()
    {
        for (int i = 0; i < StageClearEffect.Length; i++)
        {
            StageClearEffect[i].gameObject.SetActive(false);
        }
        AreaClearEffect.gameObject.SetActive(false);
    }
}
