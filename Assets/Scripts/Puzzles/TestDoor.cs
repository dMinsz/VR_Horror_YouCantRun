using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    public bool isActivated;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        isActivated = false;
        rb.isKinematic = true; // 잠김
    }

    public void Test()
    {
        rb.isKinematic = false; // 잠금 해제
        isActivated = true;
        Debug.Log($"Door Open");
    }
}
