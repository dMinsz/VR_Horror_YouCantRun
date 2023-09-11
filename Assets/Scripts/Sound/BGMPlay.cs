using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Sound.PlaySound(clip,Audio.BGM,transform.position);
    }
}
