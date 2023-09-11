using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<GameObject> attachs = new List<GameObject>();
    private List<Vector3> invetoryAttachs = new List<Vector3>();

    public GameObject phisics;
    public void SetupItems()
    {
        for (int i = 0; i < GameManager.Items.itemList.Length; i++)
        {
            if (GameManager.Items.itemList[i] != null)
            {
                GameObject item;
                GameManager.Items.itemPrfabs.TryGetValue(GameManager.Items.itemList[i], out item);

                if (item != null)
                {
                    GameManager.Resource.Instantiate(item, attachs[i].transform.position, attachs[i].transform.rotation);
                }

            }
        }

    }

    public void MakeMoveable() 
    {
        phisics.SetActive(true);
    }
}
