using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BulletPreviewAnimation : MonoBehaviour
{
    [SerializeField] TrailRenderer trailRenderer;
    Vector3 initialPos = new Vector3(-215, -262, 280);
    Vector3 DirPosion;
    Tween Launch;
    private void Awake()
    {
        trailRenderer.transform.localPosition = initialPos;
    }
    private void Start()
    {

    }
    public void StartAni()
    {
        trailRenderer.enabled = true;
        trailRenderer.time = 99999f;
        Launch = trailRenderer.transform.DOLocalMove(new Vector3(100, 201, 300), 0.3f).SetEase(Ease.InQuart);
    }
    public void EndAni()
    {
        trailRenderer.time = 0f;
        trailRenderer.enabled = false;
        Launch.Kill();
        trailRenderer.transform.localPosition = initialPos;
    }
}
