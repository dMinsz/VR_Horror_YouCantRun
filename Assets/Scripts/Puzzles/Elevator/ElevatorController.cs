using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ǻ��/���� Ȱ��ȭ/��Ȱ��ȭ �� ���������� �� ���� ���� ��ũ��Ʈ -> �̸� ���� �� �� ����
public class ElevatorController : MonoBehaviour
{
    public bool isDebug = false;
    public bool fuseActive;     // ǻ�� Ȱ��ȭ ����
    public bool leverActive;    // ���� Ȱ��ȭ ����

    public bool fuse21Active;   // ǻ�� 2-1 Ȱ��ȭ ����
    public bool fuse22Active;   // ǻ�� 2-2 Ȱ��ȭ ����
        
    public bool open = false;

    // ���� leftDoor, rightDoor �� ���� ���������ִµ� �� ������ ���� ���� �ʿ�
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Vector3 leftOrigin;
    public Vector3 rightOrigin;

    private void Awake()
    {
        leftOrigin = leftDoor.transform.position;
        rightOrigin = rightDoor.transform.position;
    }


    //for test
    private void Update()
    {
        if (fuseActive && leverActive && !open)
        {
            OpenDoor();
        }

        if (isDebug)
        {
            fuseActive = true;
            leverActive = true;
        }
    }

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

    public void DoubleFuseActive1(bool active)
    {
        fuse21Active = active;
        if (fuse21Active && fuse22Active) fuseActive = true;
        else fuseActive = false;
    }

    public void DoubleFuseActive2(bool active)
    {
        fuse22Active = active;
        if (fuse21Active && fuse22Active) fuseActive = true;
        else fuseActive = false;
    }

    public void ForceOpenDoor() 
    {
        if (!open)
        {
            leftDoor.GetComponent<Collider>().enabled = false;
            rightDoor.GetComponent<Collider>().enabled = false;
            // StartCoroutine(OpenElevatorRoutine());
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.left * 10f, 3f);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.right * 10f, 3f);

            open = true;
            
        }
    }

    public void OpenDoor()  
    {
        // ǻ��� ������ ��� Ȱ��ȭ �Ǿ������� ����
        if (fuseActive && leverActive)
        {
            if (!open)
            {
                leftDoor.GetComponent<Collider>().enabled = false;
                rightDoor.GetComponent<Collider>().enabled = false;
                // StartCoroutine(OpenElevatorRoutine());
                leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.left * 10f, 3f);
                rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.right * 10f, 3f);
                open = true;
            }
        }
        else
        {
            Debug.Log("Not Active");
        }
    }

    public void CloseDoor()
    {
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOrigin, 0.05f);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOrigin, 0.05f);

        if (Vector3.Distance(leftDoor.transform.position, leftOrigin) <= 0.01f)
        {
            leftDoor.GetComponent<Collider>().enabled = true;
            rightDoor.GetComponent<Collider>().enabled = true;

            open = false;
            leverActive = false;
        }
        
    }


    // �ڷ�ƾ���� �ε巯�� ������ x -> ���� �ʿ�
    IEnumerator OpenElevatorRoutine()
    {
        Debug.Log("OpenDoor");
        open = true;
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.left, 1f);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.right, 1f);

        yield return new WaitForSeconds(5f);

        Debug.Log("CloseDoor");
        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.right, 1f);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.left, 1f);
        open = false;

        yield return null;
    }
}
