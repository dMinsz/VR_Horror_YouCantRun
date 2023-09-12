using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingZombieDoctor : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource sfx;

    [SerializeField] int index = 0;
    private void Start()
    {
        StartCoroutine(Grr());
    }

    IEnumerator Grr() 
    {
        while (true) 
        {
            yield return new WaitUntil(() => sfx.isPlaying == false);
            index = Random.Range(0, audioClips.Length);
            sfx.clip = audioClips[index];
            sfx.Play();
        }
    }
}
