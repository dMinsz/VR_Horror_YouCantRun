using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlinkLamps : MonoBehaviour
{
    [SerializeField] float startTime;
    [SerializeField] float endTime;
    Light myLight;
    Coroutine blinkCoroutine;

    private void Awake()
    {
        myLight = GetComponentInChildren<Light>();
    }
    // Start is called before the first frame update
    void Start()
    {
        blinkCoroutine = StartCoroutine(DoBlinking());
    }


    IEnumerator DoBlinking()
    {
        while (true)
        {
            myLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(startTime, endTime));
            for(int i = 0 ; i < Random.Range(1,8); i++)
            {
                yield return new WaitForSeconds(Random.Range(0.01f, 0.3f));
                myLight.enabled = false;
                yield return new WaitForSeconds(Random.Range(0.01f, 0.3f));
                myLight.enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
    }
}
