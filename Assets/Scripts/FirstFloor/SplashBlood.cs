
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SplashBlood : MonoBehaviour
{
    //뿌리는 transform
    public Transform originTransform;

    public bool InfiniteDecal;
    public Light DirLight;
    public bool isVR = true;

    public GameObject BloodAttach;

    //뿌리는 주기
    [SerializeField] private float makeDuration = 0.1f;

    //decal 삭제 주기
    [SerializeField] private float decalDuration = 4f;

    public Vector3 direction;


    private void Start()
    {
        Vector3 rayOrigin = originTransform.position;
        Vector3 rayDirection = originTransform.forward;
        Coroutine routine1 = StartCoroutine(MakeRoutine());
    }

    public IEnumerator MakeRoutine()
    {
        while (true)
        {
            //var instance = Instantiate(BloodFX[effectIdx], originTransform.position, Quaternion.identity);
            GameObject obj = GameManager.Resource.Load<GameObject>($"Bloods/Blood{Random.Range(1, 17)}");
            var instance = GameManager.Pool.Get<GameObject>(false ,obj, originTransform.position, Quaternion.identity);
            
            transform.parent.localRotation = Quaternion.Euler(0, Random.Range(transform.localRotation.y, 360), 0);

            yield return new WaitForSeconds(makeDuration);
            if (!InfiniteDecal) Destroy(instance, decalDuration);
        }
    }

    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

}
