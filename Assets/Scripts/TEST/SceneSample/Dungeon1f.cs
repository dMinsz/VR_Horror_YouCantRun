using System.Collections;
using UnityEngine;

//예시용 씬, 씬매니저 사용법

public class Dungeon1f : BaseScene
{

    //public DialogueSystem dialog;
    //public PlayerGenerator playerGenerator;
    //public SymbolGenerator symbolGenerator;
    protected override void Awake()
    {
        Debug.Log("Dungeon 1F Scene Init");

        //GameManager.Data.Dungeon.SetUp("Dungeon1f");
        

        //리셋해줄걸 리셋해줍니다. 
        GameManager.Pool.Reset();
        GameManager.UI.Reset();

    }

    protected override IEnumerator LoadingRoutine()
    {
     
        //필요한걸 로드합니다. 중요한것은 progress 값을 올려줘야 로딩이끝납니다.
       
        yield return new WaitForSecondsRealtime(0.2f);
        progress = 0.6f;

    
        yield return new WaitForSecondsRealtime(0.2f);
        progress = 0.8f;

        yield return new WaitForSecondsRealtime(0.2f);
        progress = 1.0f;

        //GameManager.Data.BattleBackGround.Play();
    }


    public override void Clear()
    {

        //씬이 끝나고 다음씬으로 넘어갈때 해당 함수가 실행됩니다.
        //필요한걸 해주시면됩니다.

        //GameManager.Data.BattleBackGround.Stop();

        //GameManager.Data.Dungeon.didBattle = false;

        //GameManager.Data.Dialog.ResetData();
        ////다이알로그 릴리즈
        ////GameManager.Pool.ReleaseUI(GameManager.Data.Dialog.dialog_obj);

        //GameManager.Pool.erasePoolDicContet(GameManager.Data.Dialog.dialog_obj.name);


        //foreach (var player in GameManager.Data.Dungeon.InBattlePlayers) 
        //{
        //    GameManager.Pool.Release(player);
        //}

        //foreach (var symbol in GameManager.Data.Dungeon.aliveInDungeonSymbols)
        //{
        //    GameManager.Pool.Release(symbol);
        //}

    }
}