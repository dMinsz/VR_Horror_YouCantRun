using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    //private EventSystem eventSystem;

    public Canvas mainCanvas;

    private Canvas inGameCanvas;

    private Canvas fadeInCanvas;

    private Image blackImage;

    private bool nowFading;

    private Coroutine fadeInOutCoroutine;

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

        fadeInCanvas = GameManager.Resource.Instantiate<Canvas>("UI/FadeInBlack");
        fadeInCanvas.gameObject.name = "FadeInCanvas";
        inGameCanvas.sortingOrder = 0;

        blackImage = fadeInCanvas.GetComponentInChildren<Image>();

        nowFading = false;

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

        if (fadeInCanvas == null)
        {
            fadeInCanvas = GameManager.Resource.Instantiate<Canvas>("UI/FadeInBlack");
            fadeInCanvas.gameObject.name = "FadeInCanvas";
            fadeInCanvas.sortingOrder = 0;
            blackImage = fadeInCanvas.GetComponentInChildren<Image>();
            nowFading = false;
        }
    }

    public void FadeIn(float fadeTime)
    {
        Debug.Log("Fade In");
        if (fadeInCanvas == null || blackImage == null)
            Reset();
        if (!nowFading)
        {
            nowFading = true;
            fadeInOutCoroutine = StartCoroutine(FadeInCoroutine(fadeTime));
        }
    }

    IEnumerator FadeInCoroutine(float fadeTime)
    {
        float startTime = 0f;
        Color imageColor = blackImage.color;

        while(startTime < fadeTime)
        {
            if (blackImage == null)
                blackImage = fadeInCanvas.GetComponentInChildren<Image>();
            startTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f,0f, startTime / fadeTime);
            imageColor.a = alpha;
            blackImage.color = imageColor;
            yield return null;
        }

        nowFading = false;
    }

    public void FadeOut(float fadeTime)
    {
        Debug.Log("Fade Out");
        if (fadeInCanvas == null || blackImage == null)
            Reset();
        if (!nowFading)
        {
            nowFading = true;
            fadeInOutCoroutine = StartCoroutine(FadeOutCoroutine(fadeTime));
        }

    }

    IEnumerator FadeOutCoroutine(float fadeTime)
    {
        float startTime = 0f;
        Color imageColor = blackImage.color;

        while (startTime < fadeTime)
        {
            if (blackImage == null)
                blackImage = fadeInCanvas.GetComponentInChildren<Image>();
            startTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, startTime / fadeTime);
            imageColor.a = alpha;
            blackImage.color = imageColor;
            yield return null;
        }

        nowFading = false;
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
