using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : MonoBehaviour
{
    protected string monsterName;
    protected Animator animator;
    protected NavMeshAgent agent;
    // 테스트용 SerializeField
    [SerializeField] protected GameObject player;
    [SerializeField] protected TMP_Text currentText;

    public Animator Animator { get { return animator; } }
    public NavMeshAgent Agent { get { return agent; } }
    public string MonsterName { get { return monsterName; } }
    public GameObject Player { get { return player; } }
    public Transform playerPos { get { return player.transform; } } 

    public virtual void Awake()
    {
        monsterName = gameObject.name;
        player = Camera.main.gameObject;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
}

