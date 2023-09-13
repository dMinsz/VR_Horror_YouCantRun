using System.Collections;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] public TMP_Text floorText; // �������� �� ��

    public float shakeDuration = 0.5f;   // ���� �ð�
    public float shakeMagnitude = 0.2f;  // ���� ����

    [SerializeField]
    //private Transform cameraTransform;   // Interaction Setup -> Camera Offset Transform ����ȭ
    private Vector3 originPos;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public int curFloor;    // ���� ��

    private void Start()
    {
        //originPos = cameraTransform.localPosition;

        StartElevatorMovement(curFloor); // Test
    }

    // ���� ���� �´� �ڷ�ƾ ����
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

    //    // 1�� �� �� ����
    //    yield return new WaitForSeconds(1f);

    //    leftDoor.SetActive(false);
    //    rightDoor.SetActive(false);
    //}

    private void ChangeFloorText(int floor)
    {
        switch (floor)
        {
            case -1:     // ���Ͽ��� ���� ��. -1 -> 1
                floorText.text = "1";
                break;
            case 1:     // 1������ ���� �� (Ÿ�� ����)
                break;
            case 2:     // 2������ ���� ��. 2 -> 1
                floorText.text = "1";
                break;
            case 3:     // 3������ ���� ��. 3 -> 2
                floorText.text = "2";
                break;
        }
        
    }

}
