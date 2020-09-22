using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UniRx;
public class MouseGameController : MonoBehaviour
{
    public Action<float> AdjectLine;
    public Action Firstposition;
    public Action<bool> TopButtonsActive;

    public Action<bool> Bulletshine;
    public Action<Vector3[], int, float> Launch;
    [SerializeField] DrawLine drawLine;
    [SerializeField] Camera Inputcamera;
    [SerializeField] float DelayTime;
    float PointX;
    bool _cantouch = false;
    bool isEDITOR;
    bool OnUi;
    bool IsThouching;
    private void Awake()
    {
#if UNITY_EDITOR
        isEDITOR = true;
#else 
		isEDITOR = false;
#endif
    }
    private void Update()
    {
        if (isEDITOR)
        {
            if (!_cantouch)
                return;
            if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PointX = Inputcamera.ScreenToViewportPoint(Input.mousePosition).x - 0.5f;
                AdjectLine?.Invoke(PointX);
                Firstposition?.Invoke();
                IsThouching = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (IsThouching)
                {
                    Launch?.Invoke(drawLine.LinePositions(), drawLine.direction, drawLine.rad);
                    drawLine.LineActive(false);
                    CanLaunch(false);
                    IsThouching = false;
                }
            }
        }
        else
        {
            if (!_cantouch)
                return;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    OnUi = false;
                }
                else
                {
                    OnUi = true;
                }
                IsThouching = true;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    OnUi = false;
                    PointX = Inputcamera.ScreenToViewportPoint(Input.mousePosition).x - 0.5f;
                    AdjectLine?.Invoke(PointX);
                    Firstposition?.Invoke();
                }
                else
                {
                    OnUi = true;
                }
                IsThouching = true;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (!OnUi && IsThouching)
                {
                    Launch?.Invoke(drawLine.LinePositions(), drawLine.direction, drawLine.rad);
                    drawLine.LineActive(false);
                    CanLaunch(false);
                    IsThouching = false;
                }
            }
            // foreach (Touch touch in Input.touches)
            // {

            //     if (touch.phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            //     {
            //         PointX = Inputcamera.ScreenToViewportPoint(Input.mousePosition).x - 0.5f;
            //         AdjectLine?.Invoke(PointX);
            //         Firstposition?.Invoke();
            //     }
            //     else if (touch.phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            //     {
            //         Launch?.Invoke(drawLine.LinePositions(), drawLine.direction, drawLine.rad);
            //         drawLine.LineActive(false);
            //         _cantouch = false;
            //     }
            // }
        }
    }
    public void CanLaunch(bool can)
    {
        _cantouch = can;
        TopButtonsActive?.Invoke(can);
        Bulletshine?.Invoke(can);
    }
    public void StageContinue()
    {
        _cantouch = true;
        TopButtonsActive?.Invoke(true);
        Bulletshine?.Invoke(true);
    }
}
