using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SymbolAI : MonoBehaviour
{
    private abstract class BaseState : StateBase<State, SymbolAI>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Animator animator => owner.animator;
        

        protected BaseState(SymbolAI owner, StateMachine<State, SymbolAI> stateMachine) : base(owner, stateMachine)
        {
        }
    }
}
