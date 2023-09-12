using System.Collections;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] public TMP_Text floorText; // 보여지는 층 수

    public float shakeDuration = 0.5f;   // 진동 시간
    public float shakeMagnitude = 0.2f;  // 진동 세기

    [SerializeField]
    //private Transform cameraTransform;   // Interaction Setup -> Camera Offset Transform 직렬화
    private Vector3 originPos;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public int curFloor;    // 현재 층

    private void Start()
    {
        //originPos = cameraTransform.localPosition;

        StartElevatorMovement(curFloor); // Test
    }

    // 현재 층에 맞는 코루틴 실행
    public void StartElevatorMovement(int curFloor)
    {
        floorText.text = $"{curFloor}";

        //StartCoroutine(ShakeCoroutine(curFloor));
    }

    //IEnumerator ShakeCoroutine(int curFloor)
    //{
    //    float elapsed = 0f;

    //    while (elapsed < shakeDuration)
    //    {
    //        Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
    //        cameraTransform.localPosition = originPos + shakeOffset;

    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    cameraTransform.localPosition = originPos;
    //    ChangeFloorText(curFloor);

    //    // 1초 뒤 문 열림
    //    yield return new WaitForSeconds(1f);

    //    leftDoor.SetActive(false);
    //    rightDoor.SetActive(false);
    //}

    private void ChangeFloorText(int floor)
    {
        switch (floor)
        {
            case -1:     // 지하에서 탔을 때. -1 -> 1
                floorText.text = "1";
                break;
            case 1:     // 1층에서 탔을 때 (타지 않음)
                break;
            case 2:     // 2층에서 탔을 때. 2 -> 1
                floorText.text = "1";
                break;
            case 3:     // 3층에서 탔을 때. 3 -> 2
                floorText.text = "2";
                break;
        }
        
    }

}
