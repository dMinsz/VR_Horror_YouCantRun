using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

enum ManneqionPose { None,Chase, Surprise, Size }
public class StateChase : MonsterStateBase<Mannequin>
{
    bool mannequinMove;
    [SerializeField] ManneqionPose pose;

    public StateChase(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.Agent.speed = owner.MoveSpeed;
        mannequinMove = true;
        owner.mannequinMoveCoroutine = owner.StartCoroutine(MannequinMove());
        owner.mannequinSoundPlayCoroutine = owner.StartCoroutine(PlayMoveSound());
    }

    public override void Exit()
    {
        pose = ManneqionPose.None;
        mannequinMove = false;
    }

    public override void LateUpdate()
    {
    }

    public override void Setup()
    {

    }

    public override void Transition()
    {
        // 마네킹 머리 LookAt
        owner.MannequinParts[0].transform.LookAt(owner.playerPos.position);

        // 마네킹 Pose
        if (owner.PlayerInColliderRange(owner.AttackRange + 3).Length > 0)
        {
            if (pose == ManneqionPose.Surprise)
                return;
            MannequinSurprisePose();
        }
        else if (owner.PlayerInColliderRange(owner.SenseRange - 5).Length > 0)
        {
            if (pose == ManneqionPose.Chase)
                return;
            MannequinChasePose();
        }
    }

    public void MannequinChasePose()
    {
        Debug.Log("Chase Pose 함수");
        pose = ManneqionPose.Chase;
        owner.Animator.SetTrigger($"Chase_{Random.Range(1,6)}");
    }

    public void MannequinSurprisePose()
    {
        Debug.Log("Surprise Pose 함수");
        pose = ManneqionPose.Surprise;
        owner.Animator.SetTrigger("Surprise");
    }

    public override void Update()
    {
        if (owner.PlayerInColliderRange(owner.ChaseRange).Length <= 0)
        {
            owner.ChangeState(Mannequin_State.Dormant);
        }
        else if (owner.PlayerInColliderRange(owner.AttackRange).Length > 0)  // 플레이어가 공격범위에 들어오면 Attack으로 상태 변경
        {
            owner.ChangeState(Mannequin_State.Attack);
        }
    }

    public IEnumerator MannequinMove()
    {
        while (mannequinMove)
        {
            yield return new WaitForEndOfFrame();
            owner.Agent.destination = owner.playerPos.position;
        }
        yield return null;

    }

    public IEnumerator PlayMoveSound()
    {

        while (mannequinMove)
        {
            yield return new WaitForSeconds(Random.Range(0.3f,0.6f));
            GameManager.Sound.PlaySound($"MannequinMove_{Random.Range(1, 8)}", Audio.SFX, owner.transform.position, 0.7f, 0.9f);
            GameManager.Sound.PlaySound($"MannequinMove_{Random.Range(7, 15)}", Audio.SFX, owner.transform.position, 0.2f, 0.9f);
        }
    }
}
