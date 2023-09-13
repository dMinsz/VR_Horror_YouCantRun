using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushForward : MonoBehaviour
{
    [SerializeField] float pushPower;
    [SerializeField] EventStartZone eventZone;

    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        eventZone.IsRunning = false;
        body.AddForce(transform.forward * pushPower,ForceMode.Impulse);
        GameManager.Sound.PlaySound("JumpScare_1", Audio.UISFX);
    }
}
