using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GazeMonsterLifeSpan : MonoBehaviour
{
    private float lifeTime;
    //private bool isWatched = false;

    public UnityEvent OnDeaded;

    private void OnEnable()
    {
        OnDeaded.AddListener(RemoveItSelf);
    }

    private void OnDisable()
    {
        OnDeaded.RemoveListener(RemoveItSelf);
    }

    public void RemoveItSelf()
    {
        Destroy(this.gameObject);
    }

    public void GazeEnter()
    {
        lifeTime = 3f;
        StartCoroutine(lifeTimeRoutine());
    }

    public void GazeExit()
    {
        StopAllCoroutines();
    }

    IEnumerator lifeTimeRoutine()
    {
        while (true)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
            {
                OnDeaded?.Invoke();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}