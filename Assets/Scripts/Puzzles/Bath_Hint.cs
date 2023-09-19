using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bath_Hint : MonoBehaviour
{
    Collider thisCollider;
    bool isPlayed;

    private void Awake()
    {
        isPlayed = false;
        thisCollider = GetComponent<Collider>();
    }

    public void PlayDrainSound()
    {
        if (isPlayed)
            return;

        GameManager.Sound.PlaySound("bath_drain", Audio.SFX, transform.position);
        thisCollider.enabled = false;
        isPlayed = true;
    }
}
