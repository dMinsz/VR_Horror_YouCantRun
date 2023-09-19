using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllersVibration : MonoBehaviour
{

    private XRBaseController[] xrs = new XRBaseController[2];

    public float duration = 5f;
    public float Amplitude = 1f;


    public void Setup(XRBaseController left, XRBaseController right) 
    {
        xrs[0] = left;
        xrs[1] = right; 
    }

    public void Vibe() 
    {
        foreach (var xr in xrs) 
        {
            xr.SendHapticImpulse(Amplitude, duration);
        }
    }
}
