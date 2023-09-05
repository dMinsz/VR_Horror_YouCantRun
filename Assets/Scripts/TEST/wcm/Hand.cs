using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType
{
    Left,
    Right
}


public class Hand : MonoBehaviour
{
    //private float speed=1f;

    public HandType handType;
    private Animator anim;
    private InputDevice inputDevice;

    private float gripValue;
    private float triggerValue;

    private float triggerCurrent;
    private float gripCurrent;

    void Start()
    {   
        anim = GetComponent<Animator>();
        inputDevice = GetInputDevice();
    }   
    private void Update()
    {
        AnimateHand();
    }

    InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (handType == HandType.Left)
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        return inputDevices[0];
    }

    private void AnimateHand()  
    {
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);

        /*if (gripCurrent != gripValue)
        {
            gripCurrent = Mathf.Lerp(gripCurrent, gripValue, speed);
            anim.SetFloat("Grip", gripCurrent);
        }
        if (triggerCurrent != triggerValue)
        {
            triggerCurrent = Mathf.Lerp(triggerCurrent, triggerValue, speed);
            anim.SetFloat("Trigger", triggerCurrent);
        }*/
        anim.SetFloat("Grip", gripValue);
        anim.SetFloat("Trigger", triggerValue);
    }
            
   /* public void SetGrip(float v)
    {
        gripTarget = v;
    }
    public void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    private void GrabMotion()
    {
        isGrab = true;
        anim.SetFloat("Grip", 1);
    }
    private void OnSelect()
    {

        Debug.Log(1);
        GrabMotion();
    }*/
}