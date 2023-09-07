using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퓨즈/레버 활성화/비활성화 및 엘리베이터 문 열림 관리 스크립트 -> 이름 변경 될 수 있음
public class ElevatorController : MonoBehaviour
{
    public bool fuseActive;     // 퓨즈 활성화 여부
    public bool leverActive;    // 레버 활성화 여부

    public bool fuse21Active;   // 퓨즈 2-1 활성화 여부
    public bool fuse22Active;   // 퓨즈 2-2 활성화 여부
        
    public bool open;

    // 현재 leftDoor, rightDoor 두 개로 나누어져있는데 문 열림에 따라 수정 필요
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

    public void OpenDoor()  
    {
        // 퓨즈와 레버가 모두 활성화 되어있으면 실행
        if (fuseActive && leverActive)
        {
            // StartCoroutine(OpenElevatorRoutine());
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + Vector3.back, 1f);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + Vector3.forward, 1f);
        }
        else
        {
            Debug.Log("Not Active");
        }
    }

    // 코루틴에서 부드러운 움직임 x -> 수정 필요
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
