using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDrawer : MonoBehaviour
{
    public string openAniamtionName;
    public string closeAniamtionName;

    private Animation ani;

    public Collider coll;
    public bool isOpen;

    private void Awake()
    {
        ani = GetComponent<Animation>();
    }

    private void Start()
    {
        isOpen = false;
    }

    public void DrawerAnimPlay() 
    {
        if(!isOpen)
        {
            coll.enabled = false;
            ani.Play(openAniamtionName);
            isOpen = true;
        }
        else
        {
            ani.Play(closeAniamtionName);
            coll.enabled = true;
            isOpen = false;
        }
    }
}
