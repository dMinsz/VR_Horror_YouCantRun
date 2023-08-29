using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SymbolAI : MonoBehaviour
{
    private class PatrollState : BaseState
    {
        private NavMeshAgent navAgent;
        //private Unit target;

        private Animator anim;
        private List<Vector3> patrollPoints;
        private int destPoint = 0;
        public PatrollState(SymbolAI owner, StateMachine<State, SymbolAI> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            navAgent = owner.agent;
            //target = owner.target;
            patrollPoints = owner.patrollPoints;
            anim = owner.animator;

        }

        public override void Enter()
        {
            Debug.Log("SymbolAI : Enter Patrol");
            anim.SetBool("Move", true);
            GotoNextPoint();

            //navAgent.isStopped = false;
            //if (target == null)
            //{
            //    Debug.Log("SymbolAI : Enter Patrol");
            //    anim.SetBool("Move", true);
            //    GotoNextPoint();
            //}
            //else 
            //{
            //    Debug.Log("SymbolAI : Patroll Enter : Find Target, change State");
            //    stateMachine.ChangeState(State.Tracking);
            //}
           
        }

        public override void Update()
        {
             //target = owner.FindPlayer();

            //if (target != null)
            //    return;

            //if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
            //    GotoNextPoint();
        }
        public override void Transition()
        {
            //if (target != null)
            //    stateMachine.ChangeState(State.Tracking);
        }

        public override void Exit()
        {
            Debug.Log("SymbolAI : Exit Patrol");
            anim.SetBool("Move",false);

        }

        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (patrollPoints.Count == 0)
                return;

            // Set the agent to go to the currently selected destination.
            navAgent.destination = patrollPoints[destPoint];

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % patrollPoints.Count;
        }



    }
}