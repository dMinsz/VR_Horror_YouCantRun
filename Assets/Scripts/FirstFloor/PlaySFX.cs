using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] AudioClip[] clip;
    [SerializeField] float volume;
    Coroutine soundPlayCoroutine;

    private void Start()
    {
        soundPlayCoroutine = StartCoroutine(RepeatSoundPlay());
    }

    IEnumerator RepeatSoundPlay()
    {
        while (true)
        {
            GameManager.Sound.PlaySound(clip[Random.Range(0,clip.Length)],Audio.SFX,transform.position,volume);
            yield return new WaitForSeconds(Random.Range(0.7f,1.5f));
        }
    }
}
