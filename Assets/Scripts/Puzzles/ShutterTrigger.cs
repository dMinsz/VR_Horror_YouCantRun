using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterTrigger : MonoBehaviour
{
    public Shutter shutter;

    bool isChange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player") && GameManager.Gimmick.UnderTo1F)
            {
                shutter.coll.enabled = false;

                StartCoroutine(shutter.MoveUp());

                isChange = true;
            }
        }
    }

}
