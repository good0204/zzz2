using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionComponent : MonoBehaviour
{
    [SerializeField] Vector3 ReflectPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            bullet.TweenKill();
            // Vector3 a = bullet.StartPosition;
            // Vector3 incomingVec = bullet.transform.localPosition - bullet.StartPosition;
            // Vector3 normalVec = other.contacts[0].normal;
            // Vector3 reflctVec = Vector3.Reflect(incomingVec, normalVec);
            // bullet.Reflection(reflctVec.normalized);
            //ReflectPosition.y = 0f;
            bullet.Reflection(ReflectPosition);
        }
    }
}
