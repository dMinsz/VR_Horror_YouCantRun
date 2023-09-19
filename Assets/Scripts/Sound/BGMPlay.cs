using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float volume;
    [SerializeField] bool is3D;

    void Start()
    {
        float audioVolume = volume > 0 ? volume : 1; 
        GameManager.Sound.PlaySound(clip,Audio.BGM, this.gameObject, audioVolume, 1f, is3D);
    }
}
