using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ElasticButton : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<Button>();

        button.OnPointerDownAsObservable()
            .Where(_ => button.interactable)
            .Subscribe(_ => { transform.DOScale(0.9f, 0.15f); });
        
        button.OnPointerUpAsObservable()
            .Where(_ => button.interactable)
            .Subscribe(_ => { transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic); });
    }
}
