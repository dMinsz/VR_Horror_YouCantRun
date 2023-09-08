using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDrawer : MonoBehaviour
{
    Animation ani;
    public Collider coll;
    private void Awake()
    {
        ani = GetComponent<Animation>();
    }


    public void OpenAnimPlay() 
    {
        coll.enabled = false;
        ani.Play();

    }
}
