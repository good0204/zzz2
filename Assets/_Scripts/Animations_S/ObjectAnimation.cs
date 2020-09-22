using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimation : MonoBehaviour
{
    [SerializeField] Animator ObjectAnimator;
    [Range(1, 8)]
    [SerializeField] int _idleNum;
    enum state { idle, walk, run, dance, pause }
    [SerializeField] state State;
    bool OnTerrified;
    private void Awake()
    {
        if (State == state.idle)
        {
            IdleState();
        }
        else if (State == state.walk)
        {
            WalkState();
        }
        else if (State == state.run)
        {
            Run();
        }
        else if (State == state.dance)
        {
            Dance();
        }
    }
    void IdleState()
    {
        ObjectAnimator.SetTrigger("Idle" + _idleNum);
    }
    void WalkState()
    {
        ObjectAnimator.SetTrigger("Walk");
    }
    void Run()
    {
        ObjectAnimator.SetTrigger("Run");
    }
    void Dance()
    {
        ObjectAnimator.SetTrigger("Dance");
    }
    public void Pause()
    {
        ObjectAnimator.SetTrigger("Pause");
    }
    public void ReturnState()
    {
        if (State == state.walk)
        {
            WalkState();
        }
        else if (State == state.run)
        {
            Run();
        }
    }
    public void Terrified()
    {
        if (!OnTerrified)
        {
            OnTerrified = true;
            ObjectAnimator.SetBool("Terrified", OnTerrified);
        }
    }
    public void TerrifiedReset()
    {
        if (OnTerrified)
        {
            OnTerrified = false;
            ObjectAnimator.SetBool("Terrified", OnTerrified);
        }
    }


}
