using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0f, Random.Range(-180,180), 0f);
    }
}
