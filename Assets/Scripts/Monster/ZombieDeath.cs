using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeath : MonoBehaviour
{
    Animator anim;
    bool isDone = false;

    Coroutine mainroutine;

    IEnumerator Crawl() 
    {        
        anim.SetTrigger("Crawl");

        yield return new WaitForSeconds(5f);

        anim.SetTrigger("Die");
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDone)
        {
            isDone = true;
            mainroutine = StartCoroutine(Crawl());
        }
    }
}
