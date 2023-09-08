using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEncounterEventZone : MonoBehaviour
{ 
    [SerializeField] GhostEncounterBase owner;
    [SerializeField] float timeForEventStart;
    float prevTimeForEventStart;
    private bool isRunning;
    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }

    private void Awake()
    {
        owner = GetComponentInParent<GhostEncounterBase>();
        prevTimeForEventStart = timeForEventStart;
        isRunning = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isRunning)
            return;

        if (owner.IsStarted)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{this.gameObject.name} : Player in");
            if (timeForEventStart <= 0)
                owner.StartSequence();
            timeForEventStart -= Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning)
            return;

        if (other.gameObject.CompareTag("Player"))
            // RESTART COUNTDOWN
            timeForEventStart = prevTimeForEventStart;
    }
}
