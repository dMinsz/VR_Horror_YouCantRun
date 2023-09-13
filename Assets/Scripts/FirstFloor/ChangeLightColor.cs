using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColor : MonoBehaviour
{
    Light light;
    private void Awake()
    {
        light = GetComponentInChildren<Light>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Gimmick.UnderTo1F)
        {
            if(light != null)
            light.color = Color.red;
        }
    }
}
