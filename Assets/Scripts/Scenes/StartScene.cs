using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    public enum floorType
    {
        First,
        Seconds,
        Third,
        Under,
        None
    }

    public floorType floor;
    public void StartGame() 
    {

        switch (floor)
        {
            case floorType.First:
                GameManager.Scene.LoadScene("1F");
                break;
            case floorType.Seconds:
                GameManager.Scene.LoadScene("2F");
                break;
            case floorType.Third:
                GameManager.Scene.LoadScene("3F");
                break;
            case floorType.Under:
                GameManager.Scene.LoadScene("UnderGround");
                break;
        }
    }

    protected override IEnumerator LoadingRoutine()
    {

        progress = 1f;
        yield return null;
    }

    public override void Clear()
    {

    }

}
