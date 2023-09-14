using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColor : MonoBehaviour
{
    Light firstFloorLight;
    private void Awake()
    {
        firstFloorLight = GetComponentInChildren<Light>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Gimmick.UnderTo1F)
        {
            if(firstFloorLight != null)
                firstFloorLight.color = Color.red;
        }
    }
}
