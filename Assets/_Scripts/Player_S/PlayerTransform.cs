using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class PlayerTransform : MonoBehaviour
{
    public Action _idle;
    public Action<bool> _canLaunch;
    public Action SubResultcheck;
    Vector3 initialPos;
    bool StageFirstTouch;
    private void Start()
    {
        initialPos = transform.position;
        transform.DORotate(new Vector3(0, 180, 0), 0);
    }
    public void PlayerMove(int currentStage)
    {
        StateMachine.Instance.ChangeState("Stage" + currentStage, null);
        StartCoroutine(Wait());
    }
    public void FirstTouchPosition()
    {
        if (!StageFirstTouch)
        {
            transform.position = transform.position + new Vector3(1, 0, 0);
            StageFirstTouch = true;
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.8f);
        Sequence a = DOTween.Sequence();
        transform.position = new Vector3(-1, 0, transform.position.z + 25f);
        a.Append(transform.DORotate(new Vector3(0, 0, 0), 0.5f));
        a.Join(transform.DOMoveZ(transform.position.z + 10f, 1f).SetEase(Ease.Linear));
        a.OnComplete(() =>
        {
            _idle?.Invoke();
            _canLaunch?.Invoke(true);
            SubResultcheck?.Invoke();
            StageFirstTouch = false;
            transform.DORotate(new Vector3(0, 180, 0), 0);

        });
    }
    public void ReSet()
    {
        transform.position = initialPos;
        //transform.DORotate(new Vector3(0, 180, 0), 0);
        StageFirstTouch = false;
    }
    public void ClearTurn()
    {
        transform.DORotate(new Vector3(0, 180, 0), 0);
    }
}
