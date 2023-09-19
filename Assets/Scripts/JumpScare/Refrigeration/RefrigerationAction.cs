using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        int encounter = Random.Range(15, 25);
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
            GameManager.Sound.PlaySound($"Metal_Door_Open_{Random.Range(1,4)}", Audio.SFX, doors[index].transform.position,1.5f);
            //doors[index].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-100,-120));
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
            //tables[index].transform.Translate(new Vector3(0, -1f, 0) * 5 * Time.deltaTime);
            tableMoveCoroutine = StartCoroutine(MoveTable(tables[index],new Vector3(0,Random.Range(-0.5f,-1.2f),0)));
            doors.Remove(doors[index]);
            tables.Remove(tables[index]);
        }

        yield return null;
    }

    public IEnumerator MoveTable(GameObject tableObj,Vector3 pos)
    {
        GameManager.Sound.PlaySound($"Metal_Table_Slide_{Random.Range(1, 4)}", Audio.SFX, tableObj.gameObject, 1.5f);
        while (tableObj.transform.localPosition.y > pos.y) {
            yield return new WaitForEndOfFrame();
            tableObj.transform.Translate(new Vector3(0f, -0.05f, 0f) * 100f * Time.deltaTime);
        }
    }
}
