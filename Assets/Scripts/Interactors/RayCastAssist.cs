using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayCastAssist : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] XRInteractorLineVisual lineVisual;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RendererOn() 
    {
        lineRenderer.enabled = true;
        lineVisual = this.gameObject.AddComponent<XRInteractorLineVisual>();
        lineVisual.lineLength = 5f;
    }

    public void RendererOff() 
    {
        lineRenderer.enabled = false;

        if (lineVisual != null)
        {
            Destroy(lineVisual);
        }
    }


}
