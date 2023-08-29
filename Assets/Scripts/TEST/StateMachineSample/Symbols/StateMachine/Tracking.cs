using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SymbolAI : MonoBehaviour
{
    private class TrackingState : BaseState
    {

        private NavMeshAgent navAgent;
        //private Unit target;
        private Animator anim;
        private Vector3 tempPos;
        private float tempSpeed;
        public TrackingState(SymbolAI owner, StateMachine<State, SymbolAI> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            navAgent = owner.agent;
            //target = owner.target;
            anim = owner.animator;
            tempSpeed = navAgent.speed;
        }

        public override void Enter()
        {
            navAgent.isStopped = false;

            Debug.Log("SymbolAI : Enter Tracking");
            anim.SetBool("Move", true);
            navAgent.speed *= 1.5f;
            //if (target != null)
            //{
            //    tempPos = navAgent.destination;
            //    navAgent.destination = target.transform.position;
            //}
            //else
            //{
            //    navAgent.destination = tempPos;
            //}
        }

        public override void Update()
        {
            //target = owner.FindPlayer();

            //if (target != null)
            //{
            //    tempPos = navAgent.destination;
            //    navAgent.destination = target.transform.position;
            //}
            //else 
            //{
            //    navAgent.destination = tempPos;
            //}

        }
        public override void Transition()
        {
            //if (target == null) 
            //{
            //    stateMachine.ChangeState(State.Patroll);
            //}
            //else
            //{
            //    if (owner.CanAttack())
            //    {
            //        stateMachine.ChangeState(State.Attack);
            //    }
            //}
        }

        public override void Exit()
        {
            Debug.Log("SymbolAI : Exit Tracking");
            navAgent.speed = tempSpeed;
            anim.SetBool("Move", false);
        }



    }
}
