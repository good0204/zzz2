using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] string ObjectName;
    [SerializeField] TutorialArrow tutorial;
    void Start()
    {
        Transform a = tutorial.ArrowStart(ObjectName, transform);
        if (ObjectName != "Bomb" && ObjectName != "Button")
        {
            a.DOLocalMove(a.localPosition + new Vector3(0, -0.5f, 0), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else if (ObjectName == "Bomb")
        {
            a.DOLocalMoveZ(0.0257f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            a.DOLocalMoveZ(1.8f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

    }
}
