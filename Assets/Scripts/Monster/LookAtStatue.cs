using System.Collections;
using UnityEngine;

public class LookAtStatue : MonoBehaviour
{
    public float lookDelay = 1f;

    public Transform originTransform;
    Transform lookTarget;

    Coroutine mainroutine;

    private void Start()
    {
        mainroutine = StartCoroutine(LookAtRoutine());
    }

    IEnumerator LookAtRoutine()
    {
        while (true)
        {
            if (lookTarget != null)
            {
                var look = lookTarget.position;
                originTransform.LookAt(look);

                var origin = originTransform.rotation;
                origin.x = 0;
                origin.z = 0;

                originTransform.rotation = origin;
            }

            yield return new WaitForSeconds(lookDelay);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lookTarget = other.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        lookTarget = null;

        StopCoroutine(mainroutine);
    }

}
