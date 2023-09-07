using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeGotoUnderGround : MonoBehaviour
{
    public bool isChange = false;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isChange = true;

                GameManager.Scene.LoadScene("UnderGround");
            }
        }
    }
}
