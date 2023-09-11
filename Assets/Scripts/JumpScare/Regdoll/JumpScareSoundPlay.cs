using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpScareSoundPlay : MonoBehaviour
{
    bool isSFXPlayed;

    private void Awake()
    {
        isSFXPlayed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSFXPlayed)
        {
            collision.collider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isSFXPlayed)
        {
            isSFXPlayed = true;
            GameManager.Sound.PlaySound("JumpScare_1", Audio.UISFX,new Vector3(),0.5f);
        }
    }
}
