using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    Vector3[] spawnPoints;

    public void InitSpawnArray()
    {
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");
        Debug.Log($"스폰포인트 수 {spawnPointObjects.Length}");
        spawnPoints = new Vector3[spawnPointObjects.Length];
        for(int i = 0; i < spawnPointObjects.Length; i++)
        {
            spawnPoints[i] = spawnPointObjects[i].transform.position;
        }
    }

    public void SpawnMonster(string path)
    {
        InitSpawnArray();
        GameObject monster = GameManager.Resource.Load<GameObject>(path);        
        GameManager.Instantiate(monster, spawnPoints[Random.Range(0,spawnPoints.Length)],Quaternion.identity);
    }
}
