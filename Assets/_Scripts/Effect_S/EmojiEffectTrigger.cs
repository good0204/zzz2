using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlohaCorp.UI;
using TMPro;
public class EmojiEffectTrigger : MonoBehaviour
{
    CapsuleCollider grazecollider;
    [SerializeField] ParticleSystem[] Deadparticles;
    [SerializeField] ParticleSystem[] derisionparticles;
    ParticleSystem particle;
    Vector3 followPos;

    public void StartEffect(Transform parent, string Emojiname, Vector3 followPos)
    {
        if (Emojiname == "Dead")
        {
            particle = Instantiate(Deadparticles[Random.Range(0, Deadparticles.Length)]);
        }
        else
        {
            particle = Instantiate(derisionparticles[Random.Range(0, derisionparticles.Length)]);
        }
        particle.transform.SetParent(parent);
        this.followPos = followPos;
        particle.transform.parent.position = followPos + new Vector3(0, 1.5f, 0);

        particle.transform.localPosition = Vector3.zero;
    }
    public void followEmoji(Vector3 followPos)
    {
        particle.transform.parent.position = followPos + new Vector3(0, 1.5f, 0);
    }
}
