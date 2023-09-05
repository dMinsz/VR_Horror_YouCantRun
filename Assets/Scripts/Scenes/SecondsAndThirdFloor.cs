using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondsAndThirdFloor : BaseScene
{
    public bool isDebug = false;
    public SceneChanger changer;
    public GameObject CanDestroyObject;

    public GameObject playerPrefab;
    public GameObject player;
    public enum floor
    {
        Seconds,
        Third,
        None,
    }

    public Transform[] StartPos; // 0 == Third , 1 == Seconds

    public floor nowfloor = floor.Third;

    protected override void Awake()
    {
        if (isDebug) 
        {
            //test
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            player = GameManager.Pool.Get(true, playerPrefab, StartPos[0].position, StartPos[0].rotation);
        }
    }

    protected override IEnumerator LoadingRoutine()
    {
        changer.isChange = false;

        if (nowfloor == floor.Seconds)
        {

            GameManager.Resource.Destroy(player);
            GameManager.Pool.ResetDD(); // 임시방편

            playerPrefab= GameManager.Resource.Load<GameObject>("Player");
            player = GameManager.Pool.Get(true, playerPrefab, StartPos[1].position, StartPos[1].rotation);


            CanDestroyObject.SetActive(false);
        }
        else //third floor
        {
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            player = GameManager.Pool.Get(true, playerPrefab, StartPos[0].position, StartPos[0].rotation);
        }


        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {

        GameManager.Resource.Destroy(playerPrefab);
    }


}
