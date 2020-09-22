using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.GetComponent<Bullet>().Destroy();
            EffectManager.Instance.EffectPlay("ObjectHit", other.transform.position);
            SoundManager.Instance.Play("WallHit");
        }
    }
}
