using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorScene : BaseScene
{
    public bool isDebug = false;

    public GameObject playerPrefab;
    public GameObject player;

    public Transform StartPos;

    public SceneChanger sceneChanger;
   
    protected override void Awake()
    {
        if (isDebug)
        {
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);
            //sceneChanger.OpenDoor();
        }
    }

    //debug
    //private void Start()
    //{
    //    sceneChanger.OpenDoor();
    //}

    protected override IEnumerator LoadingRoutine()
    {
        playerPrefab = GameManager.Resource.Load<GameObject>("Player");
        player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

        sceneChanger.OpenDoor();
        //StartCoroutine(sceneChanger.OpenDoor());

        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {

        GameManager.Resource.Destroy(player);
        GameManager.Sound.Clear();
    }
}
