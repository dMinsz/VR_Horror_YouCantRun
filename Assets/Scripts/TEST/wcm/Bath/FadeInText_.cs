using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class FadeInText_ : MonoBehaviour
{
    //변환 시간
    public float fadeInDuration;
    
    //투명도
    public float curAlpha = 0.0f;
    public float targetAlpha;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        Color startcolor = text.color;
        startcolor.a = curAlpha;
        text.color = startcolor;
    }

    public void FadeInText()
    {
        StartCoroutine(FadeInTextRoutine());
    }

    public IEnumerator FadeInTextRoutine()
    {
        while(curAlpha < 1.0f)
        {
            curAlpha += Time.deltaTime / fadeInDuration;
            Color newColor = text.color;
            newColor.a = curAlpha;
            text.color = newColor;

            yield return new WaitForEndOfFrame();
        }
    }
}