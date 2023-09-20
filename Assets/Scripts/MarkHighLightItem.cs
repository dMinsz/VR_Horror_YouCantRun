using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkHighLightItem : MonoBehaviour
{
    public void OnHighLight()
    {
        this.gameObject.layer = LayerMask.NameToLayer("MarkItems");
    }

    public void OffHighLight()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Items");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnHighLight();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OffHighLight();
        }
    }
}
