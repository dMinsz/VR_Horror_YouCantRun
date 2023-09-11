using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareRegdoll : MonoBehaviour
{
    Coroutine destroyCoroutine;
    private void Start()
    {
        //SetBones(true);
        SetColliders(false);
        destroyCoroutine = StartCoroutine(ShowAndDestroy());
    }

    public IEnumerator ShowAndDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        SetBones(false);
        SetColliders(true);
        yield return null;
    }

    private void SetBones(bool state)
    {
        Rigidbody[] bones = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in bones)
        {
            rigidbody.isKinematic = state;
        }
    }

    private void SetColliders(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


}
