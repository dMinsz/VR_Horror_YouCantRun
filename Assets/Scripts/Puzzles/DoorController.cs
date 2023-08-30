using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool elevatorActive;

    public bool open;

    public GameObject leftDoor;
    public GameObject rightDoor;

    void Awake()
    {
        DisableDoor();
    }

    void EnableDoor()
    {
        // TODO : Elevator Activate
        Debug.Log("Enable Door");

        elevatorActive = true;
    }

    void DisableDoor()
    {
        // TODO : Elevator Deactivate
        Debug.Log("Disable Door");

        elevatorActive = false;
    }

    public void SetDoorActive(bool active)
    {
        // It's the same state?
        if (active == elevatorActive)
            return;

        // Change the machine state
        elevatorActive = active;
        if (elevatorActive)
            EnableDoor();
        else
            DisableDoor();
    }

    public void OpenDoor()
    {
        if (elevatorActive)
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
