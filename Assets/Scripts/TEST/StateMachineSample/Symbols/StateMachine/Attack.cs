using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SymbolAI : MonoBehaviour
{
    private class AttackState : BaseState
    {

        private NavMeshAgent navAgent;
        private Animator anim;
        private bool CanAttack;
        //private Unit target;
        public AttackState(SymbolAI owner, StateMachine<State, SymbolAI> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            navAgent = owner.agent;
            anim = owner.animator;
            //target = owner.target;
        }

        public override void Enter()
        {
            Debug.Log("SymbolAI : Enter Attack");
            anim.SetTrigger("Attack");
        }

        public override void Update()
        {
            //CanAttack = owner.CanAttack();
        }
        public override void Transition()
        {
            //if (!CanAttack)
            //{
            //    Debug.Log("SymbolAI : Attack failed");
            //    stateMachine.ChangeState(State.Idle);
            //    //navAgent.isStopped = true;
            //}
            //else
            //{
            //    target = owner.target;

            //    Debug.Log("SymbolAI : Attack Success");
            //    navAgent.isStopped = true;
            //    //anim.SetBool("Move", false);
            //    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle2")) 
            //    {
            //        anim.SetTrigger("AttackSuccess");
            //    }

            //    //target.gameObject.GetComponent<PlayerHiter>().TakeHit(this.gameObject);


            //}
        }

        public override void Exit()
        {
            Debug.Log("SymbolAI : Exit Attack");
            navAgent.isStopped = false;
            
        }



    }
}