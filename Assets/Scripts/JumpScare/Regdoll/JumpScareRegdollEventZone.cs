using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareRegdollEventZone : EventStartZone
{
    [SerializeField] JumpScareRegdollAction owner;

    protected override void Awake()
    {
        base.Awake();
        owner = GetComponentInParent<JumpScareRegdollAction>();
    }

    private void PlayerComeInEventZone(Collider other)
    {
            owner.FocusDirection.PlayerIn();
    }

    private void PlayerComeOutEventZone(Collider other)
    {
            owner.FocusDirection.PlayerOut();
    }
}
