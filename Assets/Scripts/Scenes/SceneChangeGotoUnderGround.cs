using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeGotoUnderGround : MonoBehaviour
{
    public bool isChange = false;
    public Coroutine fadeCoroutine;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isChange = true;
                fadeCoroutine = StartCoroutine(FadeOutCoroutine());
            }
        }
    }

    IEnumerator FadeOutCoroutine()
    {        
        yield return null;
        GameManager.UI.FadeOut(0.5f);
        yield return new WaitForSeconds(0.8f);
        GameManager.Scene.LoadScene("UnderGround");
    }

    private void OnDisable()
    {
        StopCoroutine(fadeCoroutine);
    }

}
