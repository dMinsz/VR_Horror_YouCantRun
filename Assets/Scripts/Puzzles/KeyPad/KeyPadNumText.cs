using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyPadNumText : MonoBehaviour
{
    TMP_Text keyText;

    private void Awake()
    {
        keyText = GetComponent<TMP_Text>();
    }

    public void UpdateKeyPadNum()
    {

    }
}
