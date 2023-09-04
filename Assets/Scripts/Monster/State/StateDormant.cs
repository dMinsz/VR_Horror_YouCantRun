using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class StateDorant : MonsterStateBase<Mannequin>
{
    float recognizeCount;

    public StateDorant(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        recognizeCount = 0f;
        owner.Agent.speed = 0f;
        MannequinIdlePose();
    }

    public void MannequinIdlePose()
    {
        owner.Animator.SetTrigger("Idle");
    }

    public override void Exit()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Setup()
    {
    }

    public override void Transition()
    {
    }

    public override void Update()
    {
        WaitPlayer();
    }
      
    public void WaitPlayer()
    {
        // 1. 범위에 들어옴
        if (owner.PlayerInColliderRange(owner.SenseRange).Length > 0)
        {
            recognizeCount += Time.deltaTime;
            if (recognizeCount >= 5f)
                owner.ChangeState(Mannequin_State.Chase);
        } else
        {
            recognizeCount = 0f;
        }
    }


    


}
