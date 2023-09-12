using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostEncounterAction : MonoBehaviour
{
    [SerializeField] private GhostEncounterSequence sequence;
    [SerializeField] private EventStartZone eventZone;
    [SerializeField] private GameObject ghostObject;
    private GhostEncounterGhost ghost;
    [SerializeField] private GameObject[] blinkingLights;
    [SerializeField] private GameObject door;
    [SerializeField] private float encounterTime;

    private float defaultEncounterTime;
    private bool isStarted;
    private Coroutine encounterCoroutine;
    private Coroutine jumpScareCoroutine;

    public GhostEncounterSequence Sequence {  get { return sequence; } }
    public EventStartZone EventZone { get { return eventZone; } }
    public GameObject GhostObject { get {  return ghostObject; } }
    public GhostEncounterGhost Ghost { get { return ghost; } set { ghost = value; } }
    public GameObject[] BlinkingLights { get { return blinkingLights; } }
    public GameObject Door { get { return door; } set { door = value; } }
    public float EncounterTime { get {  return encounterTime; } set { encounterTime = value; } }
    public float DefaultEncounterTime { get { return defaultEncounterTime; } }
    public bool IsStarted { get {  return isStarted; } }
    public Coroutine EncounterCoroutine { get { return encounterCoroutine; } set { encounterCoroutine = value; } }
    public Coroutine JumpScareCoroutine { get { return jumpScareCoroutine; } set { jumpScareCoroutine = value; } }

    private void Awake()
    {
        isStarted = false;
        defaultEncounterTime = encounterTime;
        sequence = GetComponentInChildren<GhostEncounterSequence>();
        eventZone = GetComponentInChildren<EventStartZone>();
        ghost = ghostObject.GetComponent<GhostEncounterGhost>();
    }

    private void Start()
    {
        StartCoroutine(FindMainCamera());
    }

    public void StartSequence()
    {
        if (isStarted)
            return;
        Debug.Log("시퀀스 시작");
        isStarted = true;
        sequence.GhostEncounterStart();
    }

    public void EndSequence()
    {
        Debug.Log("시퀀스 종료");
        isStarted = false;
        sequence.IsStarted = false;
        eventZone.IsRunning = false;
        // Destroy.this();
    }

    public void AddFlashLightToBlinkingLights()
    {
        List<GameObject> gameObjects = blinkingLights.ToList<GameObject>();
        gameObjects.Add(Camera.main.gameObject);
        blinkingLights = gameObjects.ToArray();
    }

    IEnumerator FindMainCamera()
    {
        yield return new WaitUntil(() => { return Camera.main != null; });

        AddFlashLightToBlinkingLights();
        Debug.Log($"{gameObject.name} : MainCam 찾음");

        yield break;
    }
}
