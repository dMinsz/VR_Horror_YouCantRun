using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float number;

    public void DebugTest()
    {
        Debug.Log("Test Complete!");
    }

    public void StartDebugRoutine()
    {
        StartCoroutine(DebugTestRoutine());
    }

    IEnumerator DebugTestRoutine()
    {
        while (true)
        {
            Debug.Log("1");
            yield return new WaitForSeconds(1f);
            Debug.Log("2");
            yield return new WaitForSeconds(1f);
            Debug.Log("3");
            yield return new WaitForSeconds(1f);
            yield break;
        }
    }
}
