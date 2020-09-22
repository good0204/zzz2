using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FingerTutorial : MonoBehaviour
{
    [SerializeField] Transform Finger;
    private void Start()
    {
        Sequence a = DOTween.Sequence();
        a.Append(Finger.DOScale(new Vector3(1, 1, 1), 0.5f));
        a.Append(Finger.DOLocalMoveX(200, 0.75f));
        a.Append(Finger.DOLocalMoveX(0, 0.75f));
        a.AppendInterval(0.5f);
        a.Append(Finger.DOLocalMoveX(-200, 0.75f));
        a.Append(Finger.DOLocalMoveX(0, 0.75f));
        a.SetLoops(-1, LoopType.Restart);
    }

    private void OnEnable()
    {
        Finger.gameObject.SetActive(true);

    }
    private void OnDisable()
    {
        Finger.gameObject.SetActive(false);
    }
}
