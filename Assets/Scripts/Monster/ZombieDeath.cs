using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeath : MonoBehaviour
{
    Animator anim;
    bool isDone = false;

    public AudioClip[] audioClips;
    AudioSource sfx;

    Coroutine mainroutine;


    private void Awake()
    {
        sfx = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        sfx.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDone)
        {
            isDone = true;
            mainroutine = StartCoroutine(Crawl());
        }
    }

    IEnumerator Crawl()
    {

        sfx.Stop();
        sfx.clip = audioClips[1];
        anim.SetTrigger("Crawl");
        sfx.Play();
        yield return new WaitForSeconds(5f);

        anim.SetTrigger("Die");
        sfx.Stop();
    }
}
