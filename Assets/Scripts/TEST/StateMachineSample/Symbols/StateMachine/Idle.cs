using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public partial class SymbolAI : MonoBehaviour
{
    private class IdleState : BaseState
    {
        private NavMeshAgent navAgent;
        //private Unit target;
        private UnityEvent OnIdle;
        private float idleTime;
        private float idleMaxTime;

        public IdleState(SymbolAI owner, StateMachine<State, SymbolAI> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            navAgent = owner.agent;
            //target = owner.target;
            OnIdle = owner.OnIdled;
            idleMaxTime = owner.idleMaxTime;
        }

        public override void Enter()
        {
            Debug.Log("SymbolAI : Enter Idle");
            idleTime = 0f;
            animator.SetBool("Idle", true);
            
            navAgent.isStopped = true;

            OnIdle?.Invoke();
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;

            //target = owner.FindPlayer();
        }

        public override void Transition()
        {

            //if (target != null)
            //{
            //    stateMachine.ChangeState(State.Tracking);
            //}
            //else if (idleTime > idleMaxTime)
            //{
            //    idleTime = 0f;
            //    stateMachine.ChangeState(State.Patroll);
            //}

        }

        public override void Exit()
        {
            Debug.Log("SymbolAI : Exit Idle");
            animator.SetBool("Idle", false);
            navAgent.isStopped = false;
        }
    }
}
