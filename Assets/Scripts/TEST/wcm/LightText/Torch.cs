using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;

public class Torch : MonoBehaviour
{
    private bool IsOn = false;

    private GameObject lightObject;
    private GameObject meshCollide;
    public UnityEvent offEvent;
    public UnityEvent onEvent;


    private void Start()
    {
        lightObject = transform.GetChild(0).gameObject;
        meshCollide = transform.GetChild(3).gameObject;

        OnOffLightMesh();       
    }

    public void OnOffLightMesh()
    {
        IsOn = !IsOn;
        lightObject.SetActive(IsOn);
        meshCollide.SetActive(IsOn);

        if(!IsOn) offEvent?.Invoke();
        else onEvent?.Invoke();
    }
}