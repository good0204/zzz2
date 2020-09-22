using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
public class RewardCoinEffect : MonoBehaviour
{
    [SerializeField] Transform cointextPos;
    [SerializeField] Transform InstantiatePos;
    [SerializeField] Transform Coin;
    [SerializeField] Transform CoinParent;
    [SerializeField] GameObject CanTouch;

    public async Task<bool> AnimationPlay()
    {
        for (int i = 0; i < 20; i++)
        {
            ToTarget();
        }

        await Task.Delay(1500);
        CanTouch.gameObject.SetActive(false);
        return true;
    }

    void ToTarget()
    {
        Transform coin = Instantiate(Coin);
        coin.SetParent(CoinParent);
        Sequence a = DOTween.Sequence();
        coin.localScale = new Vector3(100, 100, 100);
        CanTouch.gameObject.SetActive(true);
        coin.localPosition = InstantiatePos.localPosition + new Vector3(Random.Range(-300f, 300f), Random.Range(-300f, 300f), -30);


        a.Append(coin.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental));
        a.AppendInterval(1f);
        a.Join(coin.DOLocalMove(cointextPos.localPosition + new Vector3(0, 0, -30), 0.5f).OnComplete(() =>
        {
            Destroy(coin.gameObject);
        }));
    }
}
