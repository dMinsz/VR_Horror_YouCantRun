using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareBase : MonoBehaviour
{
    [SerializeField] protected EventStartZone eventStartZone;
    [SerializeField] protected FocusDirection focusDirection;
    [SerializeField] protected Transform objectSpawnZone;
    [SerializeField] protected GameObject regDollObejct;
    [SerializeField] protected float focusTime;
    [SerializeField] protected float shakeCamPower;

    public EventStartZone EventStartZone { get { return eventStartZone; } }
    public FocusDirection FocusDirection { get {  return focusDirection; } }
    public float ShakeCamPower { get {  return shakeCamPower; }  }
    public float FocusTime { get { return focusTime; } }

    public void SpawnObejct()
    {
        DisableFocusAndEventZoneDetecting();
        GameObject spawnMonster = Instantiate<GameObject>(regDollObejct, objectSpawnZone.position ,Quaternion.identity);
        if (spawnMonster != null)
        {
            Debug.Log("�����Ϸ�");
        } else
        {
            Debug.Log("��������");
        }

    }

    public void ShakeCam()
    {

    }

    public void DisableFocusAndEventZoneDetecting()
    {
        eventStartZone.IsRunning = false;
        focusDirection.IsRunning = false;
    }
}