using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeHead : MonoBehaviour
{
    private void Update()
    {
        //transform.localRotation = Quaternion.Euler(0, -90, 0);
    }
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(0, Random.Range(transform.localRotation.y - 4, transform.localRotation.y + 4), 0);
    }
}
