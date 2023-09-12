using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushForward : MonoBehaviour
{
    [SerializeField] float pushPower;
    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        body.AddForce(transform.forward * pushPower,ForceMode.Impulse);    
    }
}
