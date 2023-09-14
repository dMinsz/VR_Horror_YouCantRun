using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerDisplay : MonoBehaviour
{
    public Transform[] targetPoints;

    private void Start()
    {
        StartCoroutine(AutoMove());
    }

    IEnumerator AutoMove() 
    {
        while (true) 
        { 
            while (!MoveToPoint(targetPoints[0].position)) 
            {
                yield return new WaitForFixedUpdate();
            }

            while (!MoveToPoint(targetPoints[1].position))
            {
                yield return new WaitForFixedUpdate();
            }
            
            yield return new WaitForFixedUpdate();
        }
    }


    bool MoveToPoint(Vector3 point) 
    {
        transform.position = Vector3.Lerp(transform.position, point, 0.05f);

        if (Vector3.Distance(transform.position, point) <= 0.01f)
        {
            return true;

        }
        return false;
    }

}
