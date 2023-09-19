using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//로딩 은 아직 미구현해둠
public class SceneManagerEX : MonoBehaviour
{
    private BaseScene curScene;
    public BaseScene CurrentScene
    {
        get
        {
            if (curScene == null)
                curScene = GameObject.FindObjectOfType<BaseScene>();

            return curScene;
        }
    }

    public string nextScene;

    //private LoadingUI loadingUI;

    private void Awake()
    {
        //LoadingUI loadingUI = GameManager.Resource.Load<LoadingUI>("UI/LoadingUI");
        //this.loadingUI = Instantiate(loadingUI);
        //this.loadingUI.transform.SetParent(transform);
    }


    string GetSceneName(BaseScene scene)
    {
        string name = scene.name;
        return name;
    }

    public void LoadScene(BaseScene scene)
    {
        nextScene = scene.name;
        //SceneManager.LoadScene(GetSceneName(scene));
        StartCoroutine(LoadingRoutine(scene.name));
    }

    public void LoadScene(string name)
    {
        nextScene = name;
        //SceneManager.LoadScene(name);
        StartCoroutine(LoadingRoutine(name));
    }

    public AsyncOperation LoadSceneAsync(BaseScene nextScene)
    {
        return SceneManager.LoadSceneAsync(GetSceneName(nextScene));
    }

    public AsyncOperation LoadSceneAsync(string nextScene)
    {
        return SceneManager.LoadSceneAsync(nextScene);
    }


    IEnumerator LoadingRoutine(string sceneName)
    {
        GameManager.Sound.Clear();
        //loadingUI.gameObject.SetActive(true);

        if (!CurrentScene.isClear)
        {
            CurrentScene.isClear = true;
            CurrentScene.Clear();
        }
        AsyncOperation oper = LoadSceneAsync(sceneName);
        

        Time.timeScale = 0f; // Loading 중에는 시간 멈춤

        //loadingUI.SetProgress(0f);
        

        //yield return new WaitForSecondsRealtime(0.5f); // Wait fade out

        while (!oper.isDone)
        {
            //loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress)); // Scene Loading for 50%
            yield return null;
        }


        //추가로딩할것들 로딩
        CurrentScene.LoadAsync();

        while (CurrentScene.progress < 1f)
        {
            //loadingUI.SetProgress(Mathf.Lerp(0.5f, 1f, curScene.progress));
            yield return null;
        }


        oper.allowSceneActivation = true;
        //loadingUI.SetProgress(1f);
        //loadingUI.FadeOut();


        GameManager.Sound.InitSound();
        //yield return new WaitForSecondsRealtime(0.3f); // wait Fade out
        Time.timeScale = 1f;

        GameManager.Sound.FadeInAudio();
        GameManager.UI.FadeIn(0.5f);
        //loadingUI.gameObject.SetActive(false);
    }

}
