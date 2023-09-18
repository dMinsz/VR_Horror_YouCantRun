using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퓨즈/레버 활성화/비활성화 및 엘리베이터 문 열림 관리 스크립트 -> 이름 변경 될 수 있음
public class ElevatorController : MonoBehaviour
{
    public bool isDebug = false;
    public bool fuseActive;     // 퓨즈 활성화 여부
    public bool leverActive;    // 레버 활성화 여부

    public bool fuse21Active;   // 퓨즈 2-1 활성화 여부
    public bool fuse22Active;   // 퓨즈 2-2 활성화 여부

    public bool open = false;

    // 현재 leftDoor, rightDoor 두 개로 나누어져있는데 문 열림에 따라 수정 필요
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Vector3 leftOrigin;
    public Vector3 rightOrigin;

    public AudioSource audioSource;
    public AudioClip elevatorBellClip;
    public AudioClip elevatorOpenClip;

    Coroutine OpenCoroutine;

    private void Awake()
    {
        leftOrigin = leftDoor.transform.position;
        rightOrigin = rightDoor.transform.position;
        audioSource = GetComponent<AudioSource>();
    }


    //for test
    private void Update()
    {
        //if (fuseActive && leverActive && !open)
        //{
        //    ForceOpenDoor();
        //}

        //if (isDebug)
        //{
        //    fuseActive = true;
        //    leverActive = true;
        //}
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
        
        leftDoor.GetComponent<Collider>().enabled = false;
        rightDoor.GetComponent<Collider>().enabled = false;

        if (OpenCoroutine == null)
        {
            OpenCoroutine = StartCoroutine(OpenElevatorRoutine());

            audioSource.clip = elevatorBellClip;
            audioSource.Play();
        }
        
    }

    public void OpenDoor()
    {
        // 퓨즈와 레버가 모두 활성화 되어있으면 실행
        if (fuseActive && leverActive)
        {
            if (!open)
            {
                leftDoor.GetComponent<Collider>().enabled = false;
                rightDoor.GetComponent<Collider>().enabled = false;

                if (isCoroutineRunning == false)
                {
                    OpenCoroutine = StartCoroutine(OpenElevatorRoutine());

                    audioSource.clip = elevatorBellClip;
                    audioSource.Play();
                }
            }
        }
        else
        {
            Debug.Log("Not Active");
        }
    }

    public void CloseDoor()
    {
        if (open)
        {
            audioSource.Pause();
            audioSource.clip = elevatorOpenClip;
            audioSource.Play();
        }

        leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOrigin, 0.05f);
        rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOrigin, 0.05f);

        if (Vector3.Distance(leftDoor.transform.position, leftOrigin) <= 0.011f)
        {
            leftDoor.GetComponent<Collider>().enabled = true;
            rightDoor.GetComponent<Collider>().enabled = true;

            open = false;
            leverActive = false;
        }

    }

    bool isCoroutineRunning = false;
    IEnumerator OpenElevatorRoutine()
    {
        isCoroutineRunning = true;
        yield return new WaitUntil(() => audioSource.isPlaying == false);

        audioSource.clip = elevatorOpenClip;
        audioSource.Play();

        while (open == false)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOrigin + Vector3.left * 3f, 0.05f);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOrigin + Vector3.right * 3f, 0.05f);

            if (Vector3.Distance(leftDoor.transform.position, leftOrigin + Vector3.left * 3f) <= 0.1f)
            {
                leftDoor.GetComponent<Collider>().enabled = false;
                rightDoor.GetComponent<Collider>().enabled = false;


                open = true;
            }

            yield return new WaitForFixedUpdate();
        }

        isCoroutineRunning = false;
    }
}
