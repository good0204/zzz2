using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PressGimmick : MonoBehaviour
{
    [SerializeField] Vector3 ArrivalPos;
    [SerializeField] float MoveTime;
    Tween Move;
    private void Start()
    {
        Move = transform.DOLocalMove(ArrivalPos, MoveTime).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            // other.GetComponent<TargetGimmick>().PressClear();
        }
    }
}
