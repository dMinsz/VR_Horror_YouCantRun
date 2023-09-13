using System.Collections;
using UnityEngine;

public class UnderGroundScene : BaseScene
{
    public bool isDebug = true;

    public Transform StartPos;
    GameObject player;
    GameObject playerPrefab;
    protected override void Awake()
    {
        if (isDebug)
        {
            playerPrefab = GameManager.Resource.Load<GameObject>("Player");
            //player = GameManager.Pool.Get(true, playerPrefab, StartPos.position, StartPos.rotation);
            player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);
        }
    }

    protected override IEnumerator LoadingRoutine()
    {

        playerPrefab = GameManager.Resource.Load<GameObject>("Player");
        //player = GameManager.Pool.Get(true, playerPrefab, StartPos.position, StartPos.rotation);
        player = GameManager.Resource.Instantiate(playerPrefab, StartPos.position, StartPos.rotation);

        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {
        //GameManager.Resource.Destroy(player);
    }


}