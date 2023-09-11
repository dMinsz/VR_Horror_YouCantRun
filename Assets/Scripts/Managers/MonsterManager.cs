using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    Vector3[] spawnPoints;

    public void InitSpawnArray()
    {
        GameObject[] spawnPointObjects = GameObject.FindObjectsOfType<GameObject>();
        spawnPoints = new Vector3[spawnPointObjects.Length];
        foreach(GameObject spawnPointObject in spawnPointObjects)
        {
            if (spawnPointObject != null)
            {
                spawnPoints[spawnPointObject.GetInstanceID()] = spawnPointObject.transform.position;
            }
        }
    }

    public void SpawnMonster(string path)
    {
        InitSpawnArray();
        GameObject monster = GameManager.Resource.Load<GameObject>(path);
        GameManager.Instantiate(monster, spawnPoints[Random.Range(0,spawnPoints.Length+1)],Quaternion.identity);
    }
}
