using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Torch : MonoBehaviour
{
    private bool IsOn = true;

    Light light;
    public GameObject lightObject;

    private void Start()
    {
        lightObject = transform.GetChild(0).gameObject;
        //lightObject = GetComponentInChildren<Light>().gameObject;
        //light = GetComponentInChildren<Light>();
    }
    public void OnOffLight()
    {
        //light.enabled = IsOn;
        lightObject.SetActive(IsOn);
        IsOn = !IsOn;
    }
}
