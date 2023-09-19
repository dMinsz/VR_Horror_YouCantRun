using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public AudioClip lockSound;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void LockSound()
    {
        // 잠겨있으면 lockSound 재생
        if (rb.isKinematic)
        {
            audioSource.clip = lockSound;
            audioSource.Play();
        }
    }
}
