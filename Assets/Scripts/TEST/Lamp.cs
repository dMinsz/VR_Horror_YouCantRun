using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    public GameObject top;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void LampSelect()
    {
        // top.SetActive(false);

        // col.enabled = false;

        rb.AddForce(Vector3.right * 3f, ForceMode.Impulse);
    }
}
