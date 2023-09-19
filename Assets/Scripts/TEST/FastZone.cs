using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastZone : MonoBehaviour
{
    bool isChange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player") && GameManager.Gimmick.UnderTo1F)
            {


                other.transform.root.GetComponent<PlayerCaughtMode>().changeSpeed(4.0f);

                //other.transform.root.GetComponent<PlayerCaughtMode>().StopMove();
                isChange = true;
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && GameManager.Gimmick.UnderTo1F)
    //    {
    //        other.transform.root.GetComponent<PlayerCaughtMode>().changeSpeed(2.0f);
    //    }
    //}
}
