using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGroundToUp : MonoBehaviour
{

    public bool isChange = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player") )
            {

                GameManager.Gimmick.UnderTo1F = true;

                GameManager.Scene.LoadScene("1F");
            }
   
            isChange = true;
        }
    }
}
