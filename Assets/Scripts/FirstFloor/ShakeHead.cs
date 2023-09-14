using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeHead : MonoBehaviour
{

    [SerializeField] float headShakePower;
    float rotateRange;

    private void Start()
    {
        rotateRange = headShakePower > 0 ? headShakePower : 4;
    }
    private void Update()
    {
        //transform.localRotation = Quaternion.Euler(0, -90, 0);
    }
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(0, Random.Range(transform.localRotation.y - rotateRange, transform.localRotation.y + rotateRange), 0);
    }
}
