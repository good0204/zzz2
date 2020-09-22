using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickComponents : MonoBehaviour
{
    Collider thiscollider;
    Rigidbody thisrigidbody;

    public void Initialie()
    {
        thiscollider = GetComponent<Collider>();
        thiscollider.isTrigger = true;
        thisrigidbody = GetComponent<Rigidbody>();
        thisrigidbody.isKinematic = true;
    }

}
