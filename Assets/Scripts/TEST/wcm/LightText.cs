using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightText : MonoBehaviour
{
    public Light lightSource; // ������ �����մϴ�.
    //public TMPro_text textToControl; // ���� ���� ǥ�õ� TMP Text�� �����մϴ�.

    public GameObject textToControl;

    private void Start()
    {
        //textToControl = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (lightSource != null && textToControl != null)
        {
            // ������ �ؽ�Ʈ ������Ʈ�� ���� ���߰� ���� ���� �ؽ�Ʈ�� ǥ���մϴ�.
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