using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelChairDisplay : MonoBehaviour
{
    public Material highlight;

    public MeshRenderer[] renderers;
    public List<Material> m_origins;


    private void Awake()
    {
        m_origins = new List<Material>();

        foreach (var renderer in renderers) 
        {
            m_origins.Add(renderer.material);
        }
    }

    private void Start()
    {
        StartCoroutine(HighLighting());
    }

    IEnumerator HighLighting() 
    {
        while (true) 
        { 
            foreach (var renderer in renderers)
            {
                renderer.material = highlight;
            }

            yield return new WaitForSeconds(0.5f);

            for(int i=0; i < renderers.Length; i++)
            {
                renderers[i].material = m_origins[i];
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

}
