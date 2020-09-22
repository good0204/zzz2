using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class StateMachine : SingletonComponent<StateMachine>
{
    [SerializeField] Animator animator;
    [SerializeField] CinemachineVirtualCamera lasttargetCamera;

    public void ChangeState(string Triggername, Transform lasttarget)
    {
        if (Triggername == "LastTarget")
        {
            lasttargetCamera.Follow = lasttarget;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(Triggername))
        {
            animator.SetTrigger(Triggername);
        }
    }
}
