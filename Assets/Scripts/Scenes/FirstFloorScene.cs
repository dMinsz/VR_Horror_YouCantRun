using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloorScene : BaseScene
{
    public bool isDebug = true;
    public SceneChanger sceneChanger;
    public Transform StartPos;
    public Transform[] MovePoints;
    GameObject playerPrefab;
    GameObject player;


    public GameObject destroyedObjects;
    public GameObject obstructions; // �ٽ� ���Ϸ� ������ ���¿�
    protected override void Awake()
    {
        if (isDebug) 
        {
            //playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            //player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

            //Caught Test

            GameManager.Gimmick.UnderTo1F = true;

            destroyedObjects.SetActive(false);
            obstructions.SetActive(true);

            playerPrefab = GameManager.Resource.Load<GameObject>("Player_caught");
            player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

            player.GetComponent<Player>().SetupItems();
            player.GetComponent<PlayerCaughtMode>().SetUpPoints(MovePoints);

            //playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            //player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

            //player.GetComponent<Player>().SetupItems();


            sceneChanger.OpenDoor();
        }
    }

    protected override IEnumerator LoadingRoutine()
    {

        sceneChanger.OpenDoor();

        if (GameManager.Gimmick.UnderTo1F)
        {
            destroyedObjects.SetActive(false);
            obstructions.SetActive(true);

            playerPrefab = GameManager.Resource.Load<GameObject>("Player_caught");
            player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

            player.GetComponent<Player>().SetupItems();
            player.GetComponent<PlayerCaughtMode>().SetUpPoints(MovePoints);


        }
        else 
        {
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

            player.GetComponent<Player>().SetupItems();

        }




        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {
        GameManager.Resource.Destroy(player);
    }


}