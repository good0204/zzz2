using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class BulletLauncher : MonoBehaviour
{
    public Action<bool, bool> _checkGameResult;
    public Action<Transform> Followbullet;
    public Action<int> _chageBulletCount;
    public Action<int> _changeBulletBG;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform FirePosition;
    [SerializeField] float ForceValue;
    [SerializeField] float moveSpeed;
    [SerializeField] int Numberofbullets;
    Bullet bullet;
    Factory bulletFactory;
    Vector3 initialPos;

    private void Start()
    {

        bulletFactory = new Factory(bulletPrefab, 1);
        initialPos = transform.position;
    }
    public void Initailize(int Numberofbullets)
    {
        this.Numberofbullets = Numberofbullets;
        _changeBulletBG?.Invoke(Numberofbullets);
        _chageBulletCount?.Invoke(Numberofbullets);
    }

    public void Launch(Vector3[] LinePositions, int direction, float radian)
    {
        SoundManager.Instance.Play("bulletFire");
        bullet = bulletFactory.Get() as Bullet;
        bullet.transform.SetParent(transform);
        bullet.transform.position = FirePosition.position;
        bullet.Initialize(ForceValue, moveSpeed);
        bullet.moveBullet(LinePositions);
        bullet.rad = radian;
        bullet.initz = transform.position.z;
        bullet.direction = direction;
        bullet.Destroyed += OnDestroyed;
        Followbullet?.Invoke(bullet.transform);
        CheangeBulletCount();
        StartCoroutine(CheckDestroy());
    }
    IEnumerator CheckDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        if (bullet.gameObject.activeSelf)
        {
            bullet.Destroyed?.Invoke(bullet);
        }
        _checkGameResult?.Invoke(CheckGameResult(), false);
    }
    public int GetNumberofbullets()
    {
        return Numberofbullets;
    }
    public void CheangeBulletCount()
    {
        Numberofbullets--;
        _chageBulletCount?.Invoke(Numberofbullets);
    }
    void OnDestroyed(Bullet usedBullet)
    {
        usedBullet.Destroyed -= OnDestroyed;
        bulletFactory.Restore(usedBullet);
    }
    public void MoveLaucher(int z)
    {
        transform.DOMoveZ(transform.position.z + 35f, 1f);
    }
    public void ReSet()
    {
        transform.position = initialPos;
    }
    bool CheckGameResult()
    {
        if (Numberofbullets == 0)
        {
            return true;
        }
        return false;
    }
    public void CheckSubResult()
    {
        _checkGameResult?.Invoke(CheckGameResult(), true);
    }
    public void CheckRemainingbullets()
    {
        //CoinManager.Instance.StageExternalCoinUp(Numberofbullets * 10);
    }



}
