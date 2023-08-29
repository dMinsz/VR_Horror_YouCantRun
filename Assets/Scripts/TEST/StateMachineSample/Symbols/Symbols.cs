using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Symbols : MonoBehaviour//, IHitable
{
    Animator animator;
    //public List<ShadowData> hasEnemys;

    private void Awake()
    {
        animator = transform.Find("Model").GetComponent<Animator>();
    }

    public void TakeHit(GameObject attacker)
    {
        // 배틀씬으로 이동한다.

        transform.LookAt(attacker.transform);

        Debug.Log("Symbol Take Hit , Go to Battle");

       // GameManager.Data.Dungeon.tempSymbolShadows = hasEnemys;

        //GameManager.Data.Dungeon.SetGoToBattle(DungeonDataSystem.Turn.Player, this.gameObject);

        //foreach (var symbol in GameManager.Data.Dungeon.aliveInDungeonSymbols)
        //{
        //    symbol.GetComponent<Symbols>().ReleasePool();
        //}

        //foreach (var player in GameManager.Data.Dungeon.InBattlePlayers)
        //{
        //    GameManager.Pool.Release(player);
        //}

        StartCoroutine(GotoBattleScene(attacker));
        //GameManager.Scene.LoadScene("BattleScene");
    }


    IEnumerator GotoBattleScene(GameObject attacker)
    {
        animator.SetTrigger("Hit");
        //attacker.transform.GetComponent<PlayerInput>().enabled = false;
        //attacker.transform.GetComponent<PlayerHiter>().Cam.m_Lens.FieldOfView = 30;
        yield return new WaitForSeconds(0.5f);
       // attacker.transform.GetComponent<PlayerHiter>().Cam.m_Lens.FieldOfView = 60;
       // attacker.transform.GetComponent<PlayerInput>().enabled = true;
        GameManager.Scene.LoadScene("BattleScene");
        yield break;
    }

}
