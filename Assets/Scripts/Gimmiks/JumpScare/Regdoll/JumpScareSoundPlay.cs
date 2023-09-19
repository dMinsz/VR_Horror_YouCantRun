using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpScareSoundPlay : MonoBehaviour
{
    bool isHitCollision;
    bool isSFXPlayed;

    private void Awake()
    {
        isHitCollision = false;
        isSFXPlayed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHitCollision && collision.gameObject.name.Contains("Ceiling"))
        {
            isHitCollision = true;
            Debug.Log("CollisionEnter");
            collision.collider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isSFXPlayed && other.gameObject.name.Contains("Ceiling"))
        {
            isSFXPlayed = true;
            GameManager.Sound.PlaySound("JumpScare_1", Audio.UISFX,new Vector3(),0.5f);
        }
    }
}
