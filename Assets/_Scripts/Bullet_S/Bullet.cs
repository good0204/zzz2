using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class Bullet : RecycleObject
{
    Tween move;
    float moveSpeed;
    [SerializeField] Rigidbody rigid;
    public Vector3 StartPosition;
    public Action<Bullet> Destroyed;
    float ForceValue;
    Vector3[] lines;
    public int direction;
    public float initz;
    public float rad;
    [SerializeField] float inclination;
    [SerializeField] Vector2 forward;
    [SerializeField] Vector2 dir;
    [SerializeField] Vector3 refectionPosition;


    public void Initialize(float ForceValue, float moveSpeed)
    {
        this.ForceValue = ForceValue;
        this.moveSpeed = moveSpeed;
        StartPosition = transform.localPosition;
        move.SetAutoKill(false);
    }
    public void moveBullet(Vector3[] lines)
    {
        this.lines = lines;
        move = rigid.DOLocalPath(lines, moveSpeed).OnWaypointChange(x => StartPosition = lines[x - 1]).SetLookAt(0.01f)
        .OnComplete(() =>
           {
               Destroy();
           });
    }
    public void TweenKill()
    {
        move.Kill();
    }
    public void Reflection(Vector3 refectionPosition)
    {
        rigid.velocity = Vector3.zero;
        rigid.velocity = refectionPosition * ForceValue;
        this.refectionPosition = rigid.velocity;
        transform.LookAt(transform.position + refectionPosition.normalized);
    }
    public void Destroy()
    {
        TweenKill();
        Destroyed?.Invoke(this);
        rigid.velocity = Vector3.zero;
    }
    public IEnumerator ButtonDestroy()
    {
        TweenKill();
        gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        Destroyed?.Invoke(this);
    }
}

