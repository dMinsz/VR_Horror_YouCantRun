using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAssist : MonoBehaviour
{
    public Collider[] colls;

    public void OffColliders()
    {
        foreach (var c in colls)
        {
            c.enabled = false;
        }
    }

    public void OnColliders()
    {
        foreach (var c in colls)
        {
            c.enabled = true;
        }
    }

    public void DestroyHandle() 
    {
        //handles
        colls[1].enabled = false;
        colls[2].enabled = false;
    }

}
