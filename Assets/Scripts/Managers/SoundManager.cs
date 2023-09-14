using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Audio { BGM, SFX, UISFX, Size }

public class SoundManager : MonoBehaviour
{
    GameObject bgmObj;
    AudioSource bgmSource;
    GameObject loopSFX;
    GameObject loopUISFX;
    AudioSource addSource;
    List<AudioSource> sfxSources;
    Dictionary<string, AudioClip> audioDic;
    bool isMuted = false;

    private void Awake()
    {
        InitSound();
    }

    public void InitSound()
    {
        sfxSources = new List<AudioSource>();
        audioDic = new Dictionary<string, AudioClip>();
    }

    public void Clear()
    {
        StartCoroutine(ClearRoutine());

        sfxSources.Clear();
        audioDic.Clear();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public bool IsBGMPlayed()
    {
        if (bgmObj == null)
            return false;

        return true;
    }

    IEnumerator ClearRoutine()
    {
        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / 1f);
            if (AudioListener.volume <= 0f)
            {
                if (bgmObj != null)
                    GameManager.Resource.Destroy(bgmObj);
                if (loopSFX != null)
                    GameManager.Resource.Destroy(loopSFX);
                isMuted = true;
                yield break;
            }
            yield return null;
        }
    }

    public void FadeInAudio()
    {
        AudioListener.volume = 0f;
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            AudioListener.volume = Mathf.Lerp(currentVolume, 1f, elapsedTime / 1f);
            if (AudioListener.volume >= 1f)
            {
                isMuted = false;
                yield break;
            }
            yield return null;
        }
    }

    public void PlaySound(AudioClip audioClip, Audio type = Audio.SFX, Vector3 pos = new Vector3(), float volume = 1.0f, float pitch = 1.0f,bool is3D = true, bool loop = false)
    {
        StopCoroutine(FadeInRoutine());
        StopCoroutine(ClearRoutine());

        if (audioClip == null)
        {
            Debug.Log("클립없음");
            return;
        }

        if (type == Audio.BGM)
        {
            Debug.Log("This is BGM");
            bgmObj = GameManager.Resource.Instantiate<GameObject>("SoundObject/BGM");
            bgmObj.transform.parent = transform;
            bgmSource = bgmObj.GetComponent<AudioSource>();
            if (bgmSource.isPlaying)
                bgmSource.Stop();
            bgmSource.transform.position = pos;
            bgmSource.transform.parent = transform;
            bgmSource.volume = volume;
            bgmSource.spatialBlend = is3D ? 1 : 0;
            bgmSource.pitch = pitch;
            bgmSource.clip = audioClip;
            bgmSource.loop = true;
            bgmObj.name = bgmSource.clip.name;
            bgmSource.Play();
        }
        else if (type == Audio.SFX)
        {
            //Debug.Log("This is SFX");
            if (loop)
            {
                loopSFX = GameManager.Resource.Instantiate<GameObject>("SoundObject/SFX");
                addSource = loopSFX.GetComponent<AudioSource>();
                addSource.transform.position = pos;
                addSource.transform.parent = transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                loopSFX.name = addSource.clip.name;
                sfxSources.Add(addSource);

                addSource.Play();
            }
            else
            {
                GameObject addObj = GameManager.Resource.Instantiate<GameObject>("SoundObject/SFX", true);

                addObj.transform.parent = transform;
                addSource = addObj.GetComponent<AudioSource>();
                addSource.transform.position = pos;
                addSource.transform.parent = transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addObj.name = addSource.clip.name;
                sfxSources.Add(addSource);

                StartCoroutine(SFXPlayRoutine(addObj, audioClip));
            }
        }
        else
        {
            Debug.Log("This is UISFX");
            if (loop)
            {
                loopUISFX = GameManager.Resource.Instantiate<GameObject>("SoundObject/UISFX");
                addSource = loopUISFX.GetComponent<AudioSource>();
                addSource.transform.position = pos;
                addSource.transform.parent = transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                loopUISFX.name = addSource.clip.name;
                sfxSources.Add(addSource);

                addSource.Play();
            }
            else
            {
                Debug.Log("This is UISFX and NOT LOOP");
                GameObject addObj = GameManager.Resource.Instantiate<GameObject>("SoundObject/UISFX", true);

                addObj.transform.parent = transform;
                addSource = addObj.GetComponent<AudioSource>();
                addSource.transform.position = pos;
                addSource.transform.parent = transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addObj.name = addSource.clip.name;
                sfxSources.Add(addSource);

                StartCoroutine(UISFXPlayRoutine(addObj, audioClip));
            }
        }
    }

    IEnumerator SFXPlayRoutine(GameObject addObj, AudioClip audioClip)
    {
        AudioSource audioSource = addObj.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
        yield return new WaitWhile(() => { return audioSource.isPlaying; });
        if (addObj != null)
        {
            GameManager.Resource.Destroy(addObj);
        }
        yield break;
    }

    IEnumerator UISFXPlayRoutine(GameObject addObj, AudioClip audioClip)
    {
        AudioSource audioSource = addObj.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
        yield return new WaitWhile(() => { return audioSource.isPlaying; });
        if (addObj != null)
        {
            GameManager.Resource.Destroy(addObj);
        }
        yield break;
    }

    public void PlaySound(string path, Audio type = Audio.SFX, Vector3 pos = new Vector3(), float volume = 1.0f, float pitch = 1.0f,bool is3D = true, bool loop = false)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        PlaySound(audioClip, type, pos, volume, pitch,is3D, loop);
    }

    public AudioClip GetOrAddAudioClip(string path, Audio type = Audio.SFX)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Audio.BGM)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (audioDic.TryGetValue(path, out audioClip) == false)
            {
                audioClip = GameManager.Resource.Load<AudioClip>(path);
                audioDic.Add(path, audioClip);
            }
        }

        return audioClip;
    }
}
