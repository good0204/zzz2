using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ArrowTemp : MonoBehaviour
{
    [SerializeField] int Tempint;
    [SerializeField] SpriteRenderer Arrow;

    private void Start()
    {
        if (Tempint == 0)
            Arrow.transform.DOLocalMove(new Vector3(0.78f, 2.0f, 0), 0.5f).SetLoops(-1, LoopType.Yoyo);
        else
        {
            Arrow.transform.DOLocalMove(new Vector3(1.6f, 0.46f, 2), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
