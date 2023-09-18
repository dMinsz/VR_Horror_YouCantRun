using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    private bool isOn = false;
    private TMP_Text text;
    private GameObject Torch;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TriggerArea")
            isOn = true;
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "TriggerArea")
            isOn = true;
        else
            isOn = false;
    }*/

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "TriggerArea")
            isOn = true;
        else
            isOn = false;
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "TriggerArea")
            isOn = false;
    }   

    private void Update()
    {
        text.enabled = isOn;
    }
}