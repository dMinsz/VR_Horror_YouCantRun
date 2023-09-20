using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

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
        float currentVolume = AudioListener.volume;

        // 씬매니저의 로딩시간이 페이드아웃 시간보다 작기 때문에 While문 로직을 뺌.
        AudioListener.volume = 0;
        Debug.Log("브금지우기");
        if (bgmObj != null)
        {
            Debug.Log($"브금지우기 : {bgmObj.name}");
            GameManager.Resource.Destroy(bgmObj);
        }
        if (loopSFX != null)
            GameManager.Resource.Destroy(loopSFX);
        isMuted = true;
        yield return null;

        // 아래는 FadeOut Version
        /*
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / 1f);
            if (AudioListener.volume <= 0f)
            {
                Debug.Log("브금지우기");
                if (bgmObj != null)
                {
                    Debug.Log($"브금지우기 : {bgmObj.name}");
                    GameManager.Resource.Destroy(bgmObj);
                }
                if (loopSFX != null)
                    GameManager.Resource.Destroy(loopSFX);
                isMuted = true;
                yield break;
            }
            yield return null;
        }
        */
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

    public GameObject PlaySound(AudioClip audioClip, Audio type = Audio.SFX, GameObject obj = null, float volume = 1.0f, float pitch = 1.0f, bool is3D = true, float maxDistance = 10f, bool loop = false)
    {
        StopCoroutine(FadeInRoutine());
        StopCoroutine(ClearRoutine());

        GameObject soundObj = null;

        if (audioClip == null)
        {
            Debug.Log("No Clip");
            return soundObj;
        }

        if (type == Audio.BGM)
        {
            bgmObj = GameManager.Resource.Instantiate<GameObject>("SoundObject/BGM");
            bgmObj.transform.parent = transform;
            bgmSource = bgmObj.GetComponent<AudioSource>();
            if (bgmSource.isPlaying)
                bgmSource.Stop();
            bgmSource.transform.parent = obj.transform;
            bgmSource.transform.position = obj.transform.position;
            bgmSource.volume = volume;
            bgmSource.spatialBlend = is3D ? 1 : 0;
            bgmSource.pitch = pitch;
            bgmSource.clip = audioClip;
            bgmSource.loop = true;
            bgmSource.maxDistance = maxDistance != 10 ? maxDistance : 10;
            bgmObj.name = bgmSource.clip.name;
            bgmSource.Play();
            soundObj = bgmObj;
        }
        else if (type == Audio.SFX)
        {
            if (loop)
            {
                loopSFX = GameManager.Resource.Instantiate<GameObject>("SoundObject/SFX");
                addSource = loopSFX.GetComponent<AudioSource>();
                addSource.transform.parent = obj.transform;
                addSource.transform.position = obj.transform.position;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addSource.maxDistance = maxDistance != 10 ? maxDistance : 10;
                loopSFX.name = addSource.clip.name;
                sfxSources.Add(addSource);
                soundObj = loopSFX;

                addSource.Play();
            }
            else
            {
                GameObject addObj = GameManager.Resource.Instantiate<GameObject>("SoundObject/SFX", true);

                addObj.transform.parent = transform;
                addSource = addObj.GetComponent<AudioSource>();
                addSource.transform.position = obj.transform.position;
                addSource.transform.parent = obj.transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addSource.maxDistance = maxDistance != 10 ? maxDistance : 10;
                addObj.name = addSource.clip.name;
                sfxSources.Add(addSource);
                soundObj = addObj;

                StartCoroutine(SFXPlayRoutine(addObj, audioClip));
            }
        }
        else
        {
            if (loop)
            {
                loopUISFX = GameManager.Resource.Instantiate<GameObject>("SoundObject/UISFX");
                addSource = loopUISFX.GetComponent<AudioSource>();
                addSource.transform.position = obj.transform.position;
                addSource.transform.parent = obj.transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addSource.maxDistance = maxDistance != 10 ? maxDistance : 10;
                loopUISFX.name = addSource.clip.name;
                sfxSources.Add(addSource);
                soundObj = loopUISFX;

                addSource.Play();
            }
            else
            {
                GameObject addObj = GameManager.Resource.Instantiate<GameObject>("SoundObject/UISFX", true);

                addObj.transform.parent = transform;
                addSource = addObj.GetComponent<AudioSource>();
                addSource.transform.position = obj.transform.position;
                addSource.transform.parent = obj.transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addSource.maxDistance = maxDistance != 10 ? maxDistance : 10;
                addObj.name = addSource.clip.name;
                sfxSources.Add(addSource);
                soundObj = addObj;

                StartCoroutine(UISFXPlayRoutine(addObj, audioClip));
            }
        }

        return soundObj;

    }

    public void PlaySound(AudioClip audioClip, Audio type = Audio.SFX, Vector3 pos = new Vector3(), float volume = 1.0f, float pitch = 1.0f,bool is3D = true, float maxDistance = 10f, bool loop = false)
    {
        StopCoroutine(FadeInRoutine());
        StopCoroutine(ClearRoutine());

        if (audioClip == null)
        {
            Debug.Log("No Clip");
            return;
        }

        if (type == Audio.BGM)
        {
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
        yield return new WaitWhile(() => { return addObj != null && audioSource.isPlaying; });
        if (addObj != null)
        {
            GameManager.Resource.Destroy(addObj);
        } else
        {
            Debug.Log("Already Destroy");
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

    public void PlaySound(string path, Audio type = Audio.SFX, Vector3 pos = new Vector3(), float volume = 1.0f, float pitch = 1.0f, bool is3D = true, float maxDistance = 10f, bool loop = false)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        PlaySound(audioClip, type, pos, volume, pitch, is3D,maxDistance, loop);
    }

    public GameObject PlaySound(string path, Audio type = Audio.SFX, GameObject obj = null, float volume = 1.0f, float pitch = 1.0f, bool is3D = true, float maxDistance = 10f, bool loop = false)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        return PlaySound(audioClip, type, obj, volume, pitch, is3D, maxDistance, loop);
       
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
