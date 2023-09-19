using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] AudioClip[] clip;
    [SerializeField] float volume;
    [SerializeField] float startTime;
    [SerializeField] float endTime;
    [SerializeField] bool isLoop;
    Coroutine soundPlayCoroutine;
    
    private void Start()
    {
        if (isLoop)
        {
            GameManager.Sound.PlaySound(clip[Random.Range(0, clip.Length)], Audio.SFX, this.gameObject , volume, 1f, true);
        }
        else
        {
            startTime = startTime != 0 ? startTime : 0.7f;
            endTime = endTime != 0 ? endTime : 1.5f;

            soundPlayCoroutine = StartCoroutine(RepeatSoundPlay());
        }
    }

    IEnumerator RepeatSoundPlay()
    {
        while (true)
        {
            AudioClip audioclip = clip[Random.Range(0, clip.Length)];
            GameManager.Sound.PlaySound(audioclip, Audio.SFX,this.gameObject,volume);
            yield return new WaitForSeconds(Random.Range(startTime, endTime));
        }
        //yield return null;
    }

    private void OnDisable()
    {
        if(soundPlayCoroutine != null)
            StopCoroutine(soundPlayCoroutine);
    }
}
