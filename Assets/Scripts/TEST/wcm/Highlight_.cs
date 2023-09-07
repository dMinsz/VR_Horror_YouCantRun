using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Highlight_ : MonoBehaviour
{
    [FormerlySerializedAs("renderer")]
    public Renderer Renderer;

    public float detectRadius = 15f;

    float m_Highlighted = 0.0f;
    MaterialPropertyBlock m_Block;
    int m_HighlightActiveID;

    private void Start()
    {
        if (Renderer == null)
        {
            Renderer = GetComponent<Renderer>();
        }

        m_HighlightActiveID = Shader.PropertyToID("HighlightActive");
        m_Block = new MaterialPropertyBlock();
        m_Block.SetFloat(m_HighlightActiveID, m_Highlighted);
        Renderer.SetPropertyBlock(m_Block);
    }

    private void FixedUpdate()
    {
        //RaycastHit hit
    }

    private void Update()
    {

        
    }

    public void Highlight()
    {
        m_Highlighted = 1.0f;

        Renderer.GetPropertyBlock(m_Block);
        m_Block.SetFloat(m_HighlightActiveID, m_Highlighted);
        Renderer.SetPropertyBlock(m_Block);
    }

    public void RemoveHighlight()
    {
        m_Highlighted = 0.0f;

        Renderer.GetPropertyBlock(m_Block);
        m_Block.SetFloat(m_HighlightActiveID, m_Highlighted);
        Renderer.SetPropertyBlock(m_Block);
    }



}