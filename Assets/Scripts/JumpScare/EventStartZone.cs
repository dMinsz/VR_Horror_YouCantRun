using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStartZone : MonoBehaviour
{
    [SerializeField] JumpScareBase owner;
    private bool isRunning;
    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }

    private void Awake()
    {
        owner = GetComponentInParent<JumpScareBase>();
        isRunning = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isRunning)
            return;
        if(other.gameObject.CompareTag("Player"))
         owner.FocusDirection.PlayerIn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning)
            return;
        if (other.gameObject.CompareTag("Player"))
            owner.FocusDirection.PlayerOut();
    }
}
