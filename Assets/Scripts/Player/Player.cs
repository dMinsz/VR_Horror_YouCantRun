using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField] List<GameObject> attachs = new List<GameObject>();
    private List<Vector3> invetoryAttachs = new List<Vector3>();

    public WheelInteractable[] phisics;
    public Rigidbody[] rigidbodies;

    [SerializeField] public XRBaseController[] controllers;
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

    public void MoveTo(Vector3 pos)
    {
        transform.position = Vector3.Lerp(gameObject.transform.position, pos, 0.05f);
    }

    public void OffPhisics() 
    {
        foreach (var p in phisics) 
        {
            p.enabled = false;
        }

        foreach (var rb in rigidbodies)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnPhisics()
    {
        foreach (var p in phisics)
        {
            p.enabled = true;
        }
    }
}
