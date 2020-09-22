using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class CameraController : SingletonComponent<CameraController>
{
    [SerializeField] OffscreenRendering Renderer;
    public void FollowBullet(Transform bullet)
    {
        Renderer.transform.SetParent(bullet);
        Renderer.transform.localPosition = new Vector3(0, 1.5f, -2);
        Renderer.gameObject.SetActive(true);
    }
    public void ShackCamera()
    {
        Debug.Log("dd");
        transform.DOShakePosition(1f, 100f, 1);
    }
}
