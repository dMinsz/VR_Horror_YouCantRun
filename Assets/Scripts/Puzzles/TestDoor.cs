using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    public bool isActivated;

    public Rigidbody rb;

    public float openForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        isActivated = false;
        // rb.isKinematic = true; // ¿·±Ë
    }

    public void Test()
    {
        gameObject.SetActive(false);
        // rb.AddForce(transform.forward * openForce, ForceMode.Impulse);
    }
}
