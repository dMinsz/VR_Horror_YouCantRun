using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ű �е� Render ���� ��ũ��Ʈ
public class KeyPadRenderSwitch : MonoBehaviour
{
    [SerializeField] Material keyPad;
    [SerializeField] Material keyPadCorrect;
    [SerializeField] Material keyPadDisCorrect;

    Renderer render;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

    public void CorrectPassword()
    {
        render.material = keyPadCorrect;
    }

    public void DisCorrectPassword()
    {
        StartCoroutine(DisCorrectRoutine());
    }

    IEnumerator DisCorrectRoutine()
    {
        render.material = keyPadDisCorrect;

        yield return new WaitForSeconds(3f);

        render.material = keyPad;

        yield return null;
    }
}
