using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GhostLookAtPlayer : MonoBehaviour
{
    private void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform);

        var origin = this.transform.rotation;
        origin.x = 0;
        origin.z = 0;

        this.transform.rotation = origin;
    }
}
