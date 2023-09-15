using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloorSwitchBGM : MonoBehaviour
{
    [SerializeField] GameObject bgmFirst;
    [SerializeField] GameObject bgmTwice;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Gimmick.UnderTo1F)
        {
            bgmTwice.SetActive(true);
        } else
        {
            bgmFirst.SetActive(true);
        }
        
    }

}
