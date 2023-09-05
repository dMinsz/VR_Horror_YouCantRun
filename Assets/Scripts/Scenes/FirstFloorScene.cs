using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloorScene : BaseScene
{
    public bool isDebug = true;
    public Transform StartPos;
    GameObject playerPrefab;
    GameObject player;
    protected override void Awake()
    {
        if (isDebug) 
        {
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            player= GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);
        }
    }

    protected override IEnumerator LoadingRoutine()
    {
        playerPrefab = GameManager.Resource.Load<GameObject>("Player");
        player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {
        GameManager.Resource.Destroy(player);
    }


}