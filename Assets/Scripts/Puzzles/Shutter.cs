using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : MonoBehaviour
{
    [HideInInspector]public Collider coll;
    public Transform endPoint;
    private void Awake()
    {
        coll = GetComponent<Collider>();
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && GameManager.Gimmick.UnderTo1F)
    //    {
    //        coll.enabled = false;

    //        StartCoroutine(MoveUp());
    //    }
    //}

    public IEnumerator MoveUp() 
    {
        GameManager.Sound.PlaySound("ShutterUP", Audio.SFX, transform.position,3f);
        while (true) 
        {
            this.transform.position = Vector3.Lerp(transform.position, endPoint.position, 0.01f);

            if (Vector3.Distance(transform.position, endPoint.position) <= 0.01f)
            {
                break;
            }
            else 
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
