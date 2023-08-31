using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    public bool isActivated;

    private void Start()
    {
        isActivated = false;
    }

    public void Test()
    {
        if(!isActivated)
            StartCoroutine(CardTestRoutine());
    }

    IEnumerator CardTestRoutine()
    {
        isActivated = true;
        Debug.Log($"Activated : {isActivated}");

        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, 1f);

        yield return null;
    }
}
