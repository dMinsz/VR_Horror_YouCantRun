using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEncounterBase : MonoBehaviour
{
    [SerializeField] private GhostEncounterSequence sequence;
    [SerializeField] private GhostEncounterEventZone eventZone;
    [SerializeField] private GameObject ghostObject;
    [SerializeField] private GameObject[] blinkingLights;
    [SerializeField] private GameObject door;
    [SerializeField] private float encounterTime;
    private float defaultEncounterTime;
    private bool isStarted;
    private Coroutine encounterCoroutine;

    public GhostEncounterSequence Sequence {  get { return sequence; } }
    public GhostEncounterEventZone EventZone { get { return eventZone; } }
    public GameObject GhostObject { get {  return ghostObject; } }
    public GameObject[] BlinkingLights { get { return blinkingLights; } }
    public GameObject Door { get { return door; } set { door = value; } }
    public float EncounterTime { get {  return encounterTime; } set { encounterTime = value; } }
    public float DefaultEncounterTime { get { return defaultEncounterTime; } }
    public bool IsStarted { get {  return isStarted; } }
    public Coroutine EncounterCoroutine { get { return encounterCoroutine; } set { encounterCoroutine = value; } }

    private void Awake()
    {
        isStarted = false;
        defaultEncounterTime = encounterTime;
        sequence = GetComponentInChildren<GhostEncounterSequence>();
        eventZone = GetComponentInChildren<GhostEncounterEventZone>();
    }

    public void StartSequence()
    {
        if (isStarted)
            return;
        Debug.Log("½ÃÄö½º ½ÃÀÛ");
        isStarted = true;
        sequence.GhostEncounterStart();
    }

    public void EndSequence()
    {
        Debug.Log("½ÃÄö½º Á¾·á");
        isStarted = false;
        sequence.IsStarted = false;
        eventZone.IsRunning = false;
        // Destroy.this();
    }
}
