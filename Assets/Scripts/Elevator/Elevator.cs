using System.Collections;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] TMP_Text floorText;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    [SerializeField]
    private Transform cameraTransform;
    private Vector3 originPos;

    public GameObject leftDoor;
    public GameObject rightDoor;

    private void Start()
    {
        originPos = cameraTransform.localPosition;

        StartElevatorMovement();
    }

    public void StartElevatorMovement()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            cameraTransform.localPosition = originPos + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originPos;
        ChangeFloorText(2);

        yield return new WaitForSeconds(1f);

        leftDoor.SetActive(false);
        rightDoor.SetActive(false);
    }

    public void ChangeFloorText(int floor)
    {
        floorText.text = $"{floor}";
    }
}
