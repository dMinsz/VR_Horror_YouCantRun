using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RefrigerationAction : MonoBehaviour
{

    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<GameObject> tables;
    bool isFirst;
    int arraySize;
    [SerializeField] int tableIndex;
    bool isStarted;
    Coroutine JumpScareCoroutine;

    private void Awake()
    {
        isFirst = true;
        isStarted = false;
        doors = GameObject.FindGameObjectsWithTag("RefrigerationDoor").ToList();
        tables = GameObject.FindGameObjectsWithTag("RefrigerationTable").ToList();
        arraySize = doors.Count == tables.Count ? doors.Count : 0;
    }

    public void OpenTheFrigeDoors()
    {
        if (isStarted)
            return;
        isStarted = true;
        int encounter = Random.Range(25, 36);
        JumpScareCoroutine = StartCoroutine(JumpScareFrige(encounter));

    }

    public IEnumerator JumpScareFrige(int encounter)
    {
        int index = 0;
        for(int i = 0; i  < encounter; i++)
        {
            index = Random.Range(0, doors.Count);
            doors[index].transform.localRotation = Quaternion.Euler(0, 0, -120);
            yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
            tables[index].transform.localPosition = new Vector3(0, Random.Range(-0.2f,-1.6f), 0);
            doors.Remove(doors[index]);
            tables.Remove(tables[index]);
        }

    }
}
