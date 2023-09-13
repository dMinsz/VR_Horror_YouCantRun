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

    //Sound Manager
    private static SoundManager soundManager;
    public static SoundManager Sound { get { return soundManager; } }

    //Monster Manager
    private static MonsterManager monsterManager;
    public static MonsterManager Monster { get { return monsterManager; } }

    //Inventory
    private static SpecialItemManager specialItemManager;
    public static SpecialItemManager Items { get { return specialItemManager; } }

    //gimmick Manager

    private static GimmickManager gimmickManager;
    public static GimmickManager Gimmick { get { return gimmickManager; } }


    private GameManager() { }


    private void Awake() // ����Ƽ������ �����ͻ󿡼� �߰��Ҽ� �ֱ⶧���� �̷������α���
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("GameInstance: valid instance already registered.");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // ����Ƽ�� ���� ��ȯ�ϸ� �ڵ����� ������Ʈ���� �����ȴ�
                                       // �ش� �ڵ�� ���� ���ϰ� ����
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

        GameObject soundObj = new GameObject();
        soundObj.name = "SoundManager";
        soundObj.transform.parent = transform;
        soundManager = soundObj.AddComponent<SoundManager>();

        GameObject monsterObj = new GameObject();
        monsterObj.name = "MonsterManager";
        monsterObj.transform.parent = transform;
        monsterManager = monsterObj.AddComponent<MonsterManager>();

        GameObject invenObj = new GameObject();
        invenObj.name = "SpecialItemManager";
        invenObj.transform.parent = transform;
        specialItemManager = invenObj.AddComponent<SpecialItemManager>();

        GameObject gmObj = new GameObject();
        gmObj.name = "GimmickManager";
        gmObj.transform.parent = transform;
        gimmickManager = gmObj.AddComponent<GimmickManager>();

        poolManager.Init();
    }
}
