using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float volume;

    // Start is called before the first frame update
    void Start()
    {
        float bgmVolume = 0;
        bgmVolume = volume != 0 ? volume : 2;
        GameManager.Sound.PlaySound(clip,Audio.BGM,transform.position, bgmVolume);
    }
}
