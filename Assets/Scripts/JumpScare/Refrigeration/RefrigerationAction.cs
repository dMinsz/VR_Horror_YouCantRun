using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RefrigerationAction : MonoBehaviour
{

    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<GameObject> tables;
    [SerializeField] int tableIndex;
    bool isStarted;
    Coroutine jumpScareCoroutine;
    Coroutine tableMoveCoroutine;

    private void Awake()
    {
        isStarted = false;
        doors = GameObject.FindGameObjectsWithTag("RefrigerationDoor").ToList();
        tables = GameObject.FindGameObjectsWithTag("RefrigerationTable").ToList();
    }

    public void OpenTheFrigeDoors()
    {
        if (isStarted)
            return;
        isStarted = true;
        int encounter = Random.Range(25, 36);
        jumpScareCoroutine = StartCoroutine(JumpScareFrige(encounter));

    }

    public IEnumerator JumpScareFrige(int encounter)
    {
        int index = 0;
        for(int i = 0; i  < encounter; i++)
        {
            if (doors.Count == 0)
                break;
            if (tables.Count == 0)
                break;

            index = Random.Range(0, doors.Count);
            Rigidbody doorRb = doors[index].GetComponent<Rigidbody>();
            doorRb.AddForce((Vector3.forward * -1) * 500f);
            //doors[index].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-100,-120));
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
            //tables[index].transform.Translate(new Vector3(0, -1f, 0) * 5 * Time.deltaTime);
            tableMoveCoroutine = StartCoroutine(MoveTable(tables[index],new Vector3(0,Random.Range(-0.5f,-1.2f),0)));
            doors.Remove(doors[index]);
            tables.Remove(tables[index]);
        }

        yield return null;
    }

    public IEnumerator MoveTable(GameObject tableObj,Vector3 pos)
    {
        Debug.Log($"tableObj.Y {tableObj.transform.localPosition.y} pos.Y {pos.y}");
        while (tableObj.transform.localPosition.y > pos.y) {
            yield return new WaitForEndOfFrame();
            Debug.Log("무브테이블");
            tableObj.transform.Translate(new Vector3(0f, -0.05f, 0f) * 100f * Time.deltaTime);
        }
    }
}
