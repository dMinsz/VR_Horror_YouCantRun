using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SecondsAndThirdFloor;

public class SceneChanger : MonoBehaviour
{
    public GameObject CanDestroyObject;
    public SecondsAndThirdFloor nowScene;

    public bool isChange = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player") && nowScene.nowfloor == floor.Third)
            {
                nowScene.nowfloor = floor.Seconds;

                //GameManager.Resource.Destroy(nowScene.player);

                nowScene.LoadAsync();
            }
            else if (other.gameObject.CompareTag("Player") && nowScene.nowfloor == floor.Seconds) 
            {
                GameManager.Scene.LoadScene("1F");
            }

            isChange = true;
        }
    }


}
