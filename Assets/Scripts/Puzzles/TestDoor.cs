using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    public bool isActivated;

    Quaternion currentRotation;
    Vector3 targetEulerAngles;
    Quaternion targetRotation;

    float elapsedTime = 0f;
    bool rotating = false;
    float rotateTime = 0.1f;

    private void Start()
    {
        isActivated = false;
    }

    public void Test()
    {
        StartCoroutine(CardTestRoutine());
    }

    IEnumerator CardTestRoutine()
    {
        isActivated = true;
        Debug.Log($"Activated : {isActivated}");

        DoorRotation();
        rotating = true;

        while (elapsedTime < Time.deltaTime)
        {
            // transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, 1f);
            transform.rotation = Quaternion.Euler(Vector3.Slerp(
                    currentRotation.eulerAngles, targetRotation.eulerAngles, elapsedTime / rotateTime)
                );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetEulerAngles.y = Rotate90(targetEulerAngles.y);
        this.transform.rotation = Quaternion.Euler(targetEulerAngles);
    }

    public void DoorRotation()
    {
        Quaternion currentRotation = this.transform.rotation;
        Vector3 targetEulerAngles = this.transform.rotation.eulerAngles;
        targetEulerAngles.y += (88.0f);

        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
    }

    public float Rotate90(float f)
    {
        float r = f % 90;
        return (r < 45) ? f - r : f - r + 90;
    }
}
