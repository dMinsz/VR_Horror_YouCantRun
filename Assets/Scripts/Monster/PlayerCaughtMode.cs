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

    public GameObject phisics;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;
    }

    private void Start()
    {

        phisics.SetActive(false);

        mainRoutine = StartCoroutine(Move());
    }

    public void SetUpPoints(Transform[] points)
    {
        goals = points;
    }

    /// <summary>
    ///  Change Speed Base Speed = 2.0f , slow = 1.0f(Recomanded)
    /// </summary>
    /// <param name="speed">Change Speed Value</param>
    public void changeSpeed(float speed = 1.0f) 
    {
        agent.speed = speed;
    }

    public void StopMove()
    {
        agent.isStopped = true;
    }

    public void ReStartMove() 
    {
        agent.isStopped = false;
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
            yield return new WaitForFixedUpdate();
        }
        phisics.SetActive(true);
        
        originPlayer.transform.SetParent(null);

        this.gameObject.SetActive(false);

        yield return null;
    }

}
