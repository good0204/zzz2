using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Revolution : MonoBehaviour, IComponent
{
    [SerializeField] bool Left;
    [SerializeField] float radius;
    [SerializeField] float movingTime;
    bool IsAction = false;
    int Direction;
    private void Awake()
    {
        if (Left)
        {
            Direction = -1;
        }
        else
        {
            Direction = 1;
        }
    }
    public Tween Action()
    {
        GameObject RevolutionPoint = new GameObject("RevolutionPoint");
        RevolutionPoint.transform.SetParent(transform.parent);
        RevolutionPoint.transform.localPosition = this.transform.localPosition;
        RevolutionPoint.transform.localEulerAngles = transform.localEulerAngles;
        transform.SetParent(RevolutionPoint.transform);
        transform.localPosition = new Vector3(radius, 0, 0);
        IsAction = true;
        Tween a = RevolutionPoint.transform.DORotate(new Vector3(0, 360f, 0) * Direction, movingTime, RotateMode.FastBeyond360);
        return a;
    }
}
