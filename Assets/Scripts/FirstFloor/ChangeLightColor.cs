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
    void Start()
    {
        if (GameManager.Gimmick.UnderTo1F)
        {
            light.color = Color.red;
        }
    }
}
