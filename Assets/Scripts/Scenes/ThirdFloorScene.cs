using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloorScene : BaseScene
{
    public bool isDebug = false;

    public GameObject playerPrefab;
    public GameObject player;

    public Transform StartPos;

    public SceneChanger changer;

    protected override void Awake()
    {
        if (isDebug)
        {
            //test
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

            //changer.vibe.Setup(player.GetComponent<Player>().controllers[0], player.GetComponent<Player>().controllers[1]);

            //player = GameManager.Pool.Get(true, playerPrefab, StartPos[0].position, StartPos[0].rotation);
        }
    }

    private void Start()
    {
        if (isDebug)
        {
            changer.vibe.Setup(player.GetComponent<Player>().controllers[0], player.GetComponent<Player>().controllers[1]);
        }
    }

    protected override IEnumerator LoadingRoutine()
    {
        playerPrefab = GameManager.Resource.Load<GameObject>("Player");
        player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

        changer.vibe.Setup(player.GetComponent<Player>().controllers[0], player.GetComponent<Player>().controllers[1]);

        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {

        GameManager.Resource.Destroy(player);
    }


}