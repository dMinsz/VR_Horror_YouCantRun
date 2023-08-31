using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool fuseActive;     // 퓨즈 활성화 여부
    public bool leverActive;    // 레버 활성화 여부
        
    public bool open;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public void EnableFuse()
    {
        Debug.Log("Enable Fuse");

        fuseActive = true;
        if (fuseActive && leverActive) Debug.Log("Elevator Active");
    }

    public void DisableFuse()
    {
        Debug.Log("Disable Fuse");

        fuseActive = false;
    }

    public void EnableLever()
    {
        Debug.Log("Enable Lever");

        leverActive = true;
        if (fuseActive && leverActive) Debug.Log("Elevator Active");
    }

    public void DisableLever()
    {
        Debug.Log("Disable Lever");

        leverActive = false;
    }

    public void SetFuseActive(bool active)
    {
        // It's the same state?
        if (active == fuseActive)
            return;

        // Change the machine state
        fuseActive = active;
        if (fuseActive)
            EnableFuse();
        else
            DisableFuse();
    }

    public void SetLeverActive(bool active)
    {
        if (active == leverActive)
            return;

        leverActive = active;
        if (leverActive)
            EnableLever();
        else
            DisableLever();
    }

    public void OpenDoor()
    {
        if (fuseActive && leverActive)
        {
            StartCoroutine(OpenElevatorRoutine());
        }
    }

    IEnumerator OpenElevatorRoutine()
    {
        Debug.Log("OpenDoor");
        open = true;
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.back, 1f);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.forward, 1f);

        yield return new WaitForSeconds(5f);

        Debug.Log("CloseDoor");
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.forward, 1f);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.back, 1f);
        open = false;

        yield return null;
    }
}
