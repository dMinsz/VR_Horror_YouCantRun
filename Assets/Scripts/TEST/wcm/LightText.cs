using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightText : MonoBehaviour
{
    public Light lightSource; // 조명을 참조합니다.
    //public TMPro_text textToControl; // 조명에 따라 표시될 TMP Text를 참조합니다.

    public GameObject textToControl;

    private void Start()
    {
        //textToControl = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (lightSource != null && textToControl != null)
        {
            // 조명이 텍스트 오브젝트에 빛을 비추고 있을 때만 텍스트를 표시합니다.
            if (lightSource.isActiveAndEnabled)
            {
                textToControl.SetActive(true);
            }
            else
            {
                textToControl.SetActive(false);
            }
        }
    }
}