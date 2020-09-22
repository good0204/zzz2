using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class UIMark : MonoBehaviour
{
    [SerializeField] private string _dirtyMarkId;
    private Tween _tween;
    private IDisposable _dirtyMarkSubscription;
    
    void Start()
    {
        SubscribeDirtyMarkId();
    }

    public void SetNewDirtyMarkId(string newId)
    {
        _dirtyMarkSubscription?.Dispose();
        _dirtyMarkId = newId;
        SubscribeDirtyMarkId();
    }

    private void SubscribeDirtyMarkId()
    {
        gameObject.SetActive(false);
        _dirtyMarkSubscription = DirtyMarkManager
            .GetDirtyMark(_dirtyMarkId)
            .Subscribe(m =>
            {
                if (m)
                {
                    _tween?.Kill();
                    gameObject.SetActive(true);
                    transform.localScale = Vector3.zero;
                    _tween = transform.DOScale(1, .3f).SetEase(Ease.OutBack);
                }
                else
                {
                    _tween?.Kill();
                    _tween = transform.DOScale(0, .3f).SetEase(Ease.InBack)
                        .OnComplete(() => gameObject.SetActive(false));
                }
            }).AddTo(this);
    }
}
