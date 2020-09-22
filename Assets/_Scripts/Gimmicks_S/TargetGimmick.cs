using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGimmick : MonoBehaviour
{

    [SerializeField] Rigidbody bombrigidbody;
    [SerializeField] Transform EmojiPos;
    [SerializeField] CapsuleCollider thisCollider;
    [SerializeField] EmojiEffectTrigger emojiEffectTrigger;
    [SerializeField] Transform EmojiParent;
    [SerializeField] bool OnEmoji;

    [HideInInspector] public bool _clear;
    [HideInInspector] public bool IsBonus;
    [HideInInspector] public int BonusCoin;
    [HideInInspector] public TargetHit targetHit;
    [HideInInspector] public bool IsLastAreaTarget;

    bool EmojiStart = false;
    public void Clear()
    {
        _clear = true;
        StartEmoji("Dead");
        SoundManager.Instance.Play("Hit");
        GetComponent<Animator>().enabled = false;
        CoinManager.Instance.StageStandardCoinUp(10);
    }
    private void Update()
    {
        if (EmojiStart == true)
        {
            emojiEffectTrigger.followEmoji(EmojiPos.position);
        }
    }
    public bool CheckClear()
    {
        return _clear;
    }
    public void BombClear(float explosionForce, Vector3 position, float explosionRadius, float upwardsModifier)
    {
        thisCollider.enabled = false;
        if (IsLastAreaTarget)
        {
            if (targetHit.CheckLastTarget())
            {
                StartCoroutine(BombCo(explosionForce, position, explosionRadius, upwardsModifier));
                return;
            }
        }
        Clear();
        bombrigidbody.AddExplosionForce(explosionForce, position, explosionRadius, upwardsModifier, ForceMode.Impulse);
    }
    IEnumerator BombCo(float explosionForce, Vector3 position, float explosionRadius, float upwardsModifier)
    {
        targetHit.LastTargetHit(transform);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        Clear();
        bombrigidbody.AddExplosionForce(explosionForce, position, explosionRadius, upwardsModifier, ForceMode.Impulse);
        SoundManager.Instance.Play("PerfectHit");
    }
    public void PressClear()
    {
        EffectManager.Instance.EffectPlay("CleanHit", transform.position);
        SoundManager.Instance.Play("PerfectHit");
        thisCollider.enabled = false;
        gameObject.SetActive(false);
        Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            StartCoroutine(TriggetEnter(other.transform));
        }
    }
    IEnumerator TriggetEnter(Transform other)
    {
        if (!IsBonus)
        {
            if (IsLastAreaTarget)
            {
                if (targetHit.CheckLastTarget())
                {
                    targetHit.LastTargetHit(transform);
                    Time.timeScale = 0;
                    yield return new WaitForSecondsRealtime(1f);
                    Time.timeScale = 1;
                    EffectManager.Instance.EffectPlay("CleanHit", transform.position);
                    SoundManager.Instance.Play("PerfectHit");
                    CoinManager.Instance.StageExternalCoinUp(5);
                }
            }
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            float inclination = (Mathf.Tan(bullet.rad) - 2 * bullet.direction * (other.transform.position.z - bullet.initz) * ((Mathf.Pow(bullet.rad, 2f)) * 0.09f / Mathf.Pow(Mathf.Cos(bullet.rad), 2))) * 1.44f;
            Vector2 forward = new Vector2(0.1f, 0.1f * inclination);
            Vector2 dir = new Vector2(transform.position.z - other.transform.position.z, transform.position.x - other.transform.position.x);
            float angle = Mathf.Acos(Vector2.Dot(forward.normalized, dir.normalized));
            if (!IsLastAreaTarget)
            {
                if (angle < 0.64)
                {
                    EffectManager.Instance.EffectPlay("CleanHit", transform.position);
                    CoinManager.Instance.StageExternalCoinUp(5);
                    SoundManager.Instance.Play("PerfectHit");
                }
                else
                {
                    EffectManager.Instance.EffectPlay("Hit", bullet.transform.position);
                }
            }
            thisCollider.enabled = false;
        }
        else
        {
            gameObject.SetActive(false);
            EffectManager.Instance.EffectPlay("BonusHit", transform.position + new Vector3(0, 1f, 0));
            EffectManager.Instance.EffectPlay("CleanHit", transform.position);
            SoundManager.Instance.Play("coinSplash");
            CoinManager.Instance.StageStandardCoinUp(10);
        }
        Clear();
    }
    public void StartEmoji(string Emoji)
    {
        if (!OnEmoji) return;

        emojiEffectTrigger.StartEffect(EmojiParent, Emoji, EmojiPos.position);
        if (Emoji == "Dead")
            EmojiStart = true;

        Invoke("OffEmoji", 1.9f);
    }
    void OffEmoji()
    {
        EmojiStart = false;
    }

}
