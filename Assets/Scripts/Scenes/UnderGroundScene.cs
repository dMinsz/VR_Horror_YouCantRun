using System.Collections;
using UnityEngine;

public class UnderGroundScene : BaseScene
{
    public bool isDebug = true;
    public Transform StartPos;
    GameObject player;
    GameObject playerPreFab;
    protected override void Awake()
    {

    }

    protected override IEnumerator LoadingRoutine()
    {
        playerPreFab = GameManager.Resource.Load<GameObject>("Player");
        player = GameManager.Resource.Instantiate(playerPreFab, StartPos.position, StartPos.rotation);

        progress = 1f;
        yield return null;
        //yield return new WaitForSecondsRealtime(0.2f);
    }

    public override void Clear()
    {
        GameManager.Resource.Destroy(player);
    }


}