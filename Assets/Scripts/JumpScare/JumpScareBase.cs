using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpScareBase : MonoBehaviour
{
    [SerializeField] GameObject eventStartZone;
    [SerializeField] GameObject focusDirection;
    [SerializeField] GameObject objectSpawnZone;
    [SerializeField] float focusTime;
    [SerializeField] float shakeCamPower;

    public abstract void SpawnObejct();
    public abstract void ShakeCam();
}
