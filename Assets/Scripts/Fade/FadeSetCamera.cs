using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSetCamera : MonoBehaviour
{
    Coroutine mainCamCoroutine;
    Canvas fadeCanvas;

    private void Awake()
    {
        fadeCanvas = GetComponent<Canvas>();
    }
    public void Start()
    {
        mainCamCoroutine = StartCoroutine(FindMainCamera());
    }

    IEnumerator FindMainCamera()
    {
        yield return new WaitUntil(() => { return Camera.main != null; });

        fadeCanvas.Re
        mainCam = Camera.main.gameObject;
        Debug.Log($"{gameObject.name} : MainCam Ã£À½");

        yield break;
    }
}
