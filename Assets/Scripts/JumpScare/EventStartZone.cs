using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventStartZone : MonoBehaviour
{
    private bool isRunning;
    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }

    public UnityEvent OnTriggerd;
    public UnityEvent OnTriggerExited;

    protected virtual void Awake()
    {
        isRunning = true;
    }

    protected void OnTriggerStay(Collider other)
    {
        if (!isRunning)
            return;
        if (other.gameObject.CompareTag("Player"))
            OnTriggerd?.Invoke();
    }

    protected void OnTriggerExit(Collider other)
    {
        if (!isRunning)
            return;

        if (other.gameObject.CompareTag("Player"))
            OnTriggerExited?.Invoke();
    }
}
