using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEncounterEventStartCondition : MonoBehaviour
{ 
    [SerializeField] GhostEncounterAction owner;
    [SerializeField] float timeForEventStart; 
    float prevTimeForEventStart;

    private void Awake()
    {
        owner = GetComponentInParent<GhostEncounterAction>();
        prevTimeForEventStart = timeForEventStart;
    }

    public void PlayerIn()
    { 
        if (!owner.IsStarted && timeForEventStart <= 0)
        {
            owner.StartSequence();
        }
        timeForEventStart -= Time.deltaTime;
    }

    public void PlayerOut()
    {
        // RESTART COUNTDOWN
        timeForEventStart = prevTimeForEventStart;
    }
}
