using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UpDownGimmick : MonoBehaviour
{
    [SerializeField] Vector3 ArrivePos;
    [SerializeField] float MovingTime;
    [SerializeField] float PrependInterval;
    [SerializeField] float ArriveInterval;

    private void Start()
    {
        Vector3 initialPos = transform.localPosition;
        Sequence a = DOTween.Sequence();
        a.PrependInterval(PrependInterval);
        a.Append(transform.DOLocalMove(ArrivePos, MovingTime).SetEase(Ease.Linear));
        a.AppendInterval(ArriveInterval);
        a.Append(transform.DOLocalMove(initialPos, MovingTime).SetEase(Ease.Linear));
        a.SetLoops(-1, LoopType.Restart);
    }


}
