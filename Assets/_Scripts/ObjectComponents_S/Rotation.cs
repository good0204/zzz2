using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Rotation : MonoBehaviour, IComponent
{
    [SerializeField] bool Left;
    [SerializeField] float movingTime;
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
        Tween a = transform.DOLocalRotate(new Vector3(0, 360f, 0) * Direction, movingTime, RotateMode.FastBeyond360);
        return a;
    }

}
