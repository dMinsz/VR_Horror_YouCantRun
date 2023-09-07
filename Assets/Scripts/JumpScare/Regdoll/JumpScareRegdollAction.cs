using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareRegdollAction : MonoBehaviour
{
    [SerializeField] protected EventStartZone eventStartZone;
    [SerializeField] protected JumpScareEventStartCondition focusDirection;
    [SerializeField] protected Transform objectSpawnZone;
    [SerializeField] protected GameObject regDollObejct;
    [SerializeField] protected float focusTime;
    [SerializeField] protected float shakeCamPower;

    public EventStartZone EventStartZone { get { return eventStartZone; } }
    public JumpScareEventStartCondition FocusDirection { get {  return focusDirection; } }
    public float ShakeCamPower { get {  return shakeCamPower; }  }
    public float FocusTime { get { return focusTime; } }

    public void SpawnObejct()
    {
        DisableFocusAndEventZoneDetecting();
        GameObject spawnMonster = Instantiate<GameObject>(regDollObejct, objectSpawnZone.position ,Quaternion.identity);
        if (spawnMonster != null)
        {
            Debug.Log("스폰완료");
        } else
        {
            Debug.Log("스폰실패");
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
