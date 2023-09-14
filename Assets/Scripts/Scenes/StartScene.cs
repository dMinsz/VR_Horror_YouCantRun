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

    public void Goto(int floor)
    {
        switch (floor)
        {
            case 1:
                GameManager.Scene.LoadScene("1F");
                break;
            case 2:
                GameManager.Scene.LoadScene("2F");
                break;
            case 3:
                GameManager.Scene.LoadScene("3F");
                break;
            case -1:
                GameManager.Scene.LoadScene("UnderGround");
                break;
        }
    }


    public floorType gotoFloor;
    public void StartGame() 
    {

        switch (gotoFloor)
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

    public void QuitGame() 
    {
        Application.Quit();
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
