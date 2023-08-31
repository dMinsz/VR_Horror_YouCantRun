using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKeyPad : MonoBehaviour
{
    [SerializeField] Material correct;

    Renderer render;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

    public void Correct()
    {
        render.material = correct;
    }
}
