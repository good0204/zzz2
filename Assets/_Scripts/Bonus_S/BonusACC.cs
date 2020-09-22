using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusACC : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string trigger;
    void Start()
    {
        animator.SetTrigger(trigger);
    }

}
