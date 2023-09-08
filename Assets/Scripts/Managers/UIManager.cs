using UnityEngine;
//using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    //private EventSystem eventSystem;

    public Canvas mainCanvas;

    private Canvas inGameCanvas;

    void Awake()
    {
        //Make Event System for UI to Start Time
        //eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        //eventSystem.transform.parent = transform;

        mainCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        mainCanvas.gameObject.name = "MainCanvas";
        mainCanvas.sortingOrder = 100;

        inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        inGameCanvas.gameObject.name = "InGameCanvas";
        inGameCanvas.sortingOrder = 0;
    }
    public void Reset()
    {
        if (mainCanvas == null)
        {
            mainCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
            mainCanvas.gameObject.name = "MainCanvas";
            mainCanvas.sortingOrder = 100;

        }
        if (inGameCanvas == null)
        {
            inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
            inGameCanvas.gameObject.name = "InGameCanvas";
            inGameCanvas.sortingOrder = 0;
        }
    }

    #region IngameUI
    public T ShowInGameUI<T>(T gameUi) where T : InGameUI
    {
        T ui = GameManager.Pool.GetUI(gameUi);
        ui.transform.SetParent(inGameCanvas.transform, false);

        return ui;
    }

    public T ShowInGameUI<T>(string path) where T : InGameUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowInGameUI(ui);
    }

    public void CloseInGameUI<T>(T inGameUI) where T : InGameUI
    {
        GameManager.Pool.ReleaseUI(inGameUI.gameObject);
    }

    public void ClearInGameUI()
    {
        InGameUI[] inGames = inGameCanvas.GetComponentsInChildren<InGameUI>();

        foreach (InGameUI inGameUI in inGames)
        {
            GameManager.Pool.ReleaseUI(inGameUI.gameObject);
        }
    }
    #endregion
}
