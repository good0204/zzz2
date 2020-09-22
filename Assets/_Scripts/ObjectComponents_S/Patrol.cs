using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Patrol : MonoBehaviour, IComponent
{
    [Header("패트롤 속성값")]
    PathType pathType = PathType.Linear;

    [Tooltip("Element1부터 시작")]
    [SerializeField] Vector3[] wayPoints;

    [SerializeField] float movingTime;
    void Awake()
    {
        wayPoints[0] = transform.position;
        for (int i = 1; i < wayPoints.Length; i++)
        {
            wayPoints[i].y = transform.position.y;
        }

    }
    public Tween Action()
    {
        Tween a;
        if (GetComponent<Revolution>() == null)
        {
            a = transform.DOLocalPath(wayPoints, movingTime, pathType, PathMode.Full3D, 10, Color.red);
        }
        else
        {
            a = transform.parent.DOLocalPath(wayPoints, movingTime, pathType, PathMode.Full3D, 10, Color.red);
        }
        return a;
    }
}
