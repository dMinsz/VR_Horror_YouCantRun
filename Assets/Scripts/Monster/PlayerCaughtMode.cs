using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Auto Move Wheelchair
public class PlayerCaughtMode : MonoBehaviour
{
    public Player originPlayer;
    NavMeshAgent agent;
    [SerializeField] Transform[] goals;
    private int destPoint = 0;
    Coroutine mainRoutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;
    }

    private void Start()
    {
        mainRoutine = StartCoroutine(Move());
    }


    void GotoNextPoint()
    {
        if (goals.Length == 0 || goals.Length == destPoint)
        {
            return;
        }

        agent.destination = goals[destPoint].position;

        destPoint = destPoint + 1;
    }


    public void SetUpPoints(Transform[] points) 
    {
        goals = points;
    }

    IEnumerator Move()
    {
       
        while (true)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if (goals.Length == destPoint)
                {
                    break;
                }
                GotoNextPoint();
            }
            yield return new WaitForEndOfFrame();
        }

        originPlayer.MakeMoveable();
        originPlayer.transform.SetParent(null);

        this.gameObject.SetActive(false);

        yield return null;
    }

}
