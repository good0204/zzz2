using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BombGimmick : GimmickComponents
{
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float upwardsModifier;
    [SerializeField] int Bombnum;
    [SerializeField] Material bombMaterial;
    [SerializeField] MeshCollider z;
    [SerializeField] MeshRenderer meshRenderer;
    private void Start()
    {
        base.Initialie();
    }
    public void BombAction(bool Button)
    {
        if (Button)
        {
            bombMaterial.DOColor(Color.red, 0.5f).OnComplete(() =>
            {
                BombRes();
            });
        }
        else
        {
            BombRes();
        }

    }
    void BombRes()
    {
        SoundManager.Instance.Play("bombExplosion");
        EffectManager.Instance.EffectPlay("Bomb", transform.position, Vector3.zero, Bombnum);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        // z.enabled = false;
        // meshRenderer.enabled = false;
        foreach (Collider collider in colliders)
        {
            // RaycastHit hit;
            // Debug.Log(collider.name);
            // if (Physics.Raycast(transform.position + new Vector3(0, 0.15f, 0), collider.transform.position - transform.position, out hit, Mathf.Infinity))
            // {
            //     // Debug.DrawRay(transform.position + new Vector3(0, 0.15f, 0), collider.transform.position - transform.position, Color.red, 10f);
            //     if (hit.collider == collider)
            //     {
            //         Debug.DrawRay(collider.transform.position + new Vector3(0, 0.15f, 0), transform.position - collider.transform.position, Color.blue, 10f);
            //         Debug.Log(collider.name);
            if (collider.GetComponent<TargetGimmick>() != null)
            {
                collider.GetComponent<TargetGimmick>().BombClear(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }
            if (collider.GetComponent<CharacterComponents>() != null)
            {
                collider.GetComponentInParent<CharacterComponents>().BombAction(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }
            // }
            // }
        }
        gameObject.SetActive(false);
        bombMaterial.DOColor(Color.white, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            BombAction(false);
            //other.GetComponent<Bullet>().Destroy();
        }

    }
}
