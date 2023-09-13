using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindTextRay : MonoBehaviour
{
    public float maxDistance;

    private int RayNum = 17;
    private bool isRayOn;

    private Light light;
    private void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(ShootingRay());
    }

    private void OnEnable()
    {
        isRayOn = true;
    }

    private void OnDisable()
    {
        isRayOn = false;
        //light.spotAngle;
    }

    IEnumerator ShootingRay()
    {
        while (true)
        {/*
            RaycastHit hit;
            Ray ray = (transform.position, Vector3.forward);
            
            Physics.Raycast(transform.position, Vector3.forward, maxDistance);
            */
            DrawRay();
            yield return new WaitForEndOfFrame();
            
        }
    }
    private void DrawRay()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 50;
        Gizmos.DrawRay(transform.position, direction);
    }

}
