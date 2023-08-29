using UnityEngine;


public class GameManager : MonoBehaviour
{
    //gameManager
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    //SceneManager
    private static SceneManagerEX sceneManager;
    public static SceneManagerEX Scene { get { return sceneManager; } }

    //PoolManager
    private static PoolManager poolManager;

    public static PoolManager Pool { get { return poolManager; } }

    //ResourceManager
    private static ResourceManager resourceManager;
    public static ResourceManager Resource { get { return resourceManager; } }

    //UI Manager
    private static UIManager uiManager;
    public static UIManager UI { get { return uiManager; } }


    private GameManager() { }


    private void Awake() // 유니티에서는 에디터상에서 추가할수 있기때문에 이런식으로구현
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("GameInstance: valid instance already registered.");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // 유니티는 씬을 전환하면 자동으로 오브젝트들이 삭제된다
                                       // 해당 코드로 삭제 안하고 유지
        instance = this;

        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {

        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.SetParent(transform);
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.SetParent(transform);
        poolManager = poolObj.AddComponent<PoolManager>();


        GameObject uiObj = new GameObject();
        uiObj.name = "UIManager";
        uiObj.transform.parent = transform;
        uiManager = uiObj.AddComponent<UIManager>();


        GameObject sObj = new GameObject();
        sObj.name = "SceneManagerEX";
        sObj.transform.SetParent(transform);
        sceneManager = sObj.AddComponent<SceneManagerEX>();


        poolManager.Init();
    }
}
