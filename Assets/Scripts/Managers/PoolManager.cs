using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


/*
 * 사용법 
 * 1. Get 함수에 첫번째 인자를 false 로 주면 해당 씬에서만 사용하는 풀을 만든다.
 *    이때에는 Reset() 함수를 이용해주면 이전에 풀들이 사라진다.(각 씬에있는 씬 스크립트에서 리셋해주는걸추천)
 *    
 * 2. Get 함수에 첫번째 인자를 True로 주면 Dont Destroy 풀이 되고 씬이동을 하더라도
 *    데이터는 사라지지않는다 만약 리셋을 해야할 상황이있다면 ResetDD() 함수를 이용하면된다.
 *    
 * 3. 몇까지 기능들이 더있긴한데 테스트 되지않았으니 문제가 발생하면 고치거나
 *    개발자에게 물어보자.
 */



public class PoolManager : MonoBehaviour
{
    // ObjectPool 유니티 공식 지원 풀 사용
    //key는 오브젝트의 이름으로 하기로 약속해둔다.
    public Dictionary<string, ObjectPool<GameObject>> poolDic; //사용할 풀
    public Dictionary<string, ObjectPool<GameObject>> uipoolDic; //사용할 풀
    Dictionary<string, Transform> poolContainer;// 풀 별로 모을 컨테이너

    Transform poolRoot; // 모든 풀들을 가지고있을 곳


    //Dont Destroy Pool

    public Dictionary<string, ObjectPool<GameObject>> ddpoolDic; //사용할 풀
    public Dictionary<string, ObjectPool<GameObject>> dduipoolDic; //사용할 풀
    Dictionary<string, Transform> ddpoolContainer;// 풀 별로 모을 컨테이너
    Transform DontDestroyPoolRoot; // 씬 전환시 없어지지않는 풀

    //UI 풀
    Canvas canvasRoot;

    public void erasePoolDicContet(string key , bool isDontDestroy = false)
    {
        if (!isDontDestroy)
        {
            poolDic.Remove(key);
        }
        else
        {
            ddpoolDic.Remove(key);
        }
    }

    public void eraseContainerContet(string key, bool isDontDestroy = false)
    {
        if (!isDontDestroy)
        {
            poolContainer.Remove(key);
        }
        else
        {
            ddpoolContainer.Remove(key);
        }
    }

    private void Awake()
    {
    }

    //Init new Pool Setting
    public void Init()
    {   // 무조건 해줘야함
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        uipoolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;


        ddpoolDic = new Dictionary<string, ObjectPool<GameObject>>();
        dduipoolDic = new Dictionary<string, ObjectPool<GameObject>>();
        ddpoolContainer = new Dictionary<string, Transform>();

        DontDestroyPoolRoot = new GameObject("DontDestroyPoolRoot").transform;
        DontDestroyPoolRoot.transform.parent = transform;

        canvasRoot = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
    }

    public void Reset()
    {
        canvasRoot = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        uipoolDic = new Dictionary<string, ObjectPool<GameObject>>();

        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
    }

    public void ResetDD() 
    { // dont destroy pool Reset
        if (DontDestroyPoolRoot != null)
        {
            Destroy(DontDestroyPoolRoot.gameObject);
        }

        canvasRoot = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        dduipoolDic = new Dictionary<string, ObjectPool<GameObject>>();

        ddpoolDic = new Dictionary<string, ObjectPool<GameObject>>();
        ddpoolContainer = new Dictionary<string, Transform>();

        DontDestroyPoolRoot = new GameObject("DontDestroyPoolRoot").transform;
        DontDestroyPoolRoot.transform.parent = transform;
    }


    public void CleanUpDontDestroyRoot()
    { // 비워있는 컨테이너 삭제
        if (DontDestroyPoolRoot != null && DontDestroyPoolRoot.childCount > 0)
        {

            for (int i = 0; i < DontDestroyPoolRoot.childCount; i++)
            {
                var container = DontDestroyPoolRoot.GetChild(i);

                if (container.childCount <= 0)
                {
                    Destroy(container.gameObject);
                }

            }
        }
    }

    public void DestroyContainer(GameObject obj)
    {
        if (DontDestroyPoolRoot != null && DontDestroyPoolRoot.childCount > 0)
        {

            for (int i = 0; i < DontDestroyPoolRoot.childCount; i++)
            {
                var container = DontDestroyPoolRoot.GetChild(i);

                if (container.childCount > 0)
                {
                    if (container.GetChild(0).gameObject == obj)
                    {
                        Destroy(container.gameObject);
                    }
                }


            }
        }
    }



    public T Get<T>(bool IsDontDestroy, T original, Vector3 position, Quaternion rotation, Transform parent, string suffix = "") where T : Object
    {
        if (!IsDontDestroy)
        {

            if (original is GameObject) // gameObject 일때
            {
                GameObject prefab = original as GameObject;
                string key = prefab.name; // 키를 오브젝트의 이름으로


                if (suffix != "")
                {
                    key += suffix;
                }
                GameObject obj;

                if (!poolDic.ContainsKey(key)) // 이미 키로 설정되어 있지않으면(해당하는 이름의 풀이없으면)
                    CreatePool(key, prefab); // 풀로만든다.

                obj = poolDic[key].Get(); // 키로 이미 있으면 가져와서 쓰고 없으면 위에 만든 풀에서 가져와서쓴다


                //해당하는 오브젝트에 부모,위치,포지션,로테이션 설정
                obj.transform.parent = parent;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj as T; //만든거 리턴
            }
            else if (original is Component) // Component 일때 
            {
                Component component = original as Component;
                string key = component.gameObject.name;// 키를 해당 컴포넌트가 가지고있는 오브젝트의 이름으로

                if (suffix != "")
                {
                    key += suffix;
                }

                GameObject obj;

                if (!poolDic.ContainsKey(key)) // 이미 키로 설정되어 있지않으면(해당하는 이름의 풀이없으면)
                    CreatePool(key, component.gameObject); // 풀로만든다.

                obj = poolDic[key].Get(); // 키로 이미 있으면 가져와서 쓰고 없으면 위에 만든 풀에서 가져와서쓴다

                //해당하는 오브젝트에 부모,위치,포지션,로테이션 설정
                obj.transform.parent = parent;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj.GetComponent<T>(); // 만든거의 컴포넌트 리턴
            }
            else // 게임오브젝트도아니고 컴포넌트도 아닐때 아무것도안함
            {
                return null;
            }
        }
        else 
        {
            if (original is GameObject) // gameObject 일때
            {
                GameObject prefab = original as GameObject;
                string key = prefab.name; // 키를 오브젝트의 이름으로


                if (suffix != "")
                {
                    key += suffix;
                }

                GameObject obj;

                if (!ddpoolDic.ContainsKey(key)) // 이미 키로 설정되어 있지않으면(해당하는 이름의 풀이없으면)
                    DCreatePool(key, prefab); // 풀로만든다.

                obj = ddpoolDic[key].Get(); // 키로 이미 있으면 가져와서 쓰고 없으면 위에 만든 풀에서 가져와서쓴다


                //해당하는 오브젝트에 부모,위치,포지션,로테이션 설정
                obj.transform.parent = parent;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj as T; //만든거 리턴
            }
            else if (original is Component) // Component 일때 
            {
                Component component = original as Component;
                string key = component.gameObject.name;// 키를 해당 컴포넌트가 가지고있는 오브젝트의 이름으로

                if (suffix != "")
                {
                    key += suffix;
                }

                GameObject obj;

                if (!ddpoolDic.ContainsKey(key)) // 이미 키로 설정되어 있지않으면(해당하는 이름의 풀이없으면)
                    DCreatePool(key, component.gameObject); // 풀로만든다.

                obj = ddpoolDic[key].Get(); // 키로 이미 있으면 가져와서 쓰고 없으면 위에 만든 풀에서 가져와서쓴다

                //해당하는 오브젝트에 부모,위치,포지션,로테이션 설정
                obj.transform.parent = parent;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj.GetComponent<T>(); // 만든거의 컴포넌트 리턴
            }
            else // 게임오브젝트도아니고 컴포넌트도 아닐때 아무것도안함
            {
                return null;
            }

        }
    }


    // 아래의 Get 함수들은 각각 하나의 값들씩 안넣을때 오버로딩
    // 안넣은 값은 기본값으로 넣어준다.

    public T Get<T>(bool IsDontDestroy, T original, Vector3 position, Quaternion rotation, string suffix) where T : Object
    {
        return Get<T>(IsDontDestroy, original, position, rotation, null, suffix);
    }


    public T Get<T>(bool IsDontDestroy, T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Get<T>(IsDontDestroy, original, position, rotation, null);
    }

    public T Get<T>(bool IsDontDestroy, T original, Transform parent) where T : Object
    {
        return Get<T>(IsDontDestroy, original, Vector3.zero, Quaternion.identity, parent);
    }

    public T Get<T>(bool IsDontDestroy, T original) where T : Object
    {
        return Get<T>(IsDontDestroy, original, Vector3.zero, Quaternion.identity, null);
    }



    //풀 에서 해제 / 릴리즈
    //풀에 없으면 
    public bool Release<T>(T instance , bool isDonDestroy = false) where T : Object
    {
        if (!isDonDestroy) 
        {
            if (instance is GameObject)
            {
                GameObject go = instance as GameObject;
                string key = go.name;

                if (!poolDic.ContainsKey(key)) // 해당하는 key 가 풀에없으면 당연히 릴리즈 실패
                    return false;

                poolDic[key].Release(go); // 여기에 Release 는 유니티의 ObjectPool 릴리즈 를 사용
                return true;
            }
            else if (instance is Component)
            {
                Component component = instance as Component;
                string key = component.gameObject.name;

                if (!poolDic.ContainsKey(key))// 해당하는 key 가 풀에없으면 당연히 릴리즈 실패
                    return false;

                poolDic[key].Release(component.gameObject); // 컴포넌트라 컴포넌트를 가지고있는 오브젝트를 릴리즈한다.
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            if (instance is GameObject)
            {
                GameObject go = instance as GameObject;
                string key = go.name;

                if (!ddpoolDic.ContainsKey(key)) // 해당하는 key 가 풀에없으면 당연히 릴리즈 실패
                    return false;

                ddpoolDic[key].Release(go); // 여기에 Release 는 유니티의 ObjectPool 릴리즈 를 사용
                return true;
            }
            else if (instance is Component)
            {
                Component component = instance as Component;
                string key = component.gameObject.name;

                if (!ddpoolDic.ContainsKey(key))// 해당하는 key 가 풀에없으면 당연히 릴리즈 실패
                    return false;

                ddpoolDic[key].Release(component.gameObject); // 컴포넌트라 컴포넌트를 가지고있는 오브젝트를 릴리즈한다.
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //풀안에 해당하는 오브젝트가 있는 지 확인하는 함수
    public bool IsContain<T>(T original, bool isDonDestroy = false) where T : Object
    {
        if (!isDonDestroy)
        {
            if (original is GameObject)
            {
                GameObject prefab = original as GameObject;
                string key = prefab.name;

                if (poolDic.ContainsKey(key))
                    return true;
                else
                    return false;

            }
            else if (original is Component)
            {
                Component component = original as Component;
                string key = component.gameObject.name;

                if (poolDic.ContainsKey(key))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        else 
        {
            if (original is GameObject)
            {
                GameObject prefab = original as GameObject;
                string key = prefab.name;

                if (ddpoolDic.ContainsKey(key))
                    return true;
                else
                    return false;

            }
            else if (original is Component)
            {
                Component component = original as Component;
                string key = component.gameObject.name;

                if (ddpoolDic.ContainsKey(key))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
      
    }

    //풀이 없을때 만드는 함수
    private void CreatePool(string key, GameObject prefab)
    {
        GameObject root = new GameObject();
        root.gameObject.name = $"{key}Container";

        root.transform.parent = poolRoot;
        poolContainer.Add(key, root.transform);

        //유니티 지원 ObjectPool 을 만들때 
        // 만들때,가져올때,릴리즈할때,지울때 에 액션을 넣어줘야한다.
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
                obj.transform.parent = null;
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.parent = poolContainer[key];
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );


        poolDic.Add(key, pool);

    }

    //DonDestroy Pool Create

    private void DCreatePool(string key, GameObject prefab)
    {
        GameObject root = new GameObject();
        root.gameObject.name = $"{key}DContainer";

        root.transform.parent = DontDestroyPoolRoot;
        ddpoolContainer.Add(key, root.transform);

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
                obj.transform.parent = null;
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.parent = ddpoolContainer[key];
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        ddpoolDic.Add(key, pool);
    }



    // UI 풀
    public T GetUI<T>(T original, Vector3 position, string suffix = "") where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (suffix != "")
            {
                key += suffix;
            }


            if (!uipoolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = uipoolDic[key].Get();
            obj.transform.position = position;
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (suffix != "")
            {
                key += suffix;
            }

            if (!uipoolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = uipoolDic[key].Get();
            obj.transform.position = position;
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public T GetUI<T>(T original, Transform parent, string suffix = "") where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (suffix != "")
            {
                key += suffix;
            }

            if (!uipoolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = uipoolDic[key].Get();
            //obj.transform.position = position;
            obj.transform.SetParent(parent, false);
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (suffix != "")
            {
                key += suffix;
            }

            if (!uipoolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = uipoolDic[key].Get();
            //obj.transform.position = position;
            obj.transform.SetParent(parent, false);
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public T GetUI<T>(T original, string suffix = "") where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (suffix != "")
            {
                key += suffix;
            }

            if (!uipoolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = uipoolDic[key].Get();
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (suffix != "")
            {
                key += suffix;
            }

            if (!uipoolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = uipoolDic[key].Get();
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public bool ReleaseUI<T>(T instance) where T : Object
    {
        if (instance is GameObject)
        {
            GameObject go = instance as GameObject;
            string key = go.name;

            if (!uipoolDic.ContainsKey(key))
                return false;

            uipoolDic[key].Release(go);
            return true;
        }
        else if (instance is Component)
        {
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!uipoolDic.ContainsKey(key))
                return false;

            uipoolDic[key].Release(component.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    // UI 는 캔버스에 넣어두고 다시빼서 이동시키는게
    // 위치값이 안이상해진다.
    private void CreateUIPool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(canvasRoot.transform, false);
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        uipoolDic.Add(key, pool);
    }


    public bool IsContainUI<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (uipoolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (uipoolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }
}
