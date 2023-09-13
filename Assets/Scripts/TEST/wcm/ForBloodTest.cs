
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ForBloodTest : MonoBehaviour
{
    //뿌리는 transform
    public Transform originTransform;
    
    public bool InfiniteDecal;
    public Light DirLight;
    public bool isVR = true;


    public GameObject BloodAttach;
    //피 뿌려지는 종류 
    public GameObject[] BloodFX;

    //뿌리는 주기
    private float makeDuration = 0.1f;

    //decal 삭제 주기
    private float decalDuration = 4f;

    public Vector3 direction;
    int effectIdx;


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
            if (effectIdx == BloodFX.Length) effectIdx = 0;
            var instance = Instantiate(BloodFX[effectIdx], originTransform.position, Quaternion.identity);
            effectIdx++;
            
            //this.transform.localEulerAngles = new Vector3(this.transform.rotation.x, this.transform.rotation.y + 30, this.transform.rotation.z);

            transform.parent.localRotation = Quaternion.Euler(0, Random.Range(transform.localRotation.y, 360), 0);

            yield return new WaitForSeconds(makeDuration);
            if (!InfiniteDecal) Destroy(instance, decalDuration);
        }
    }

    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    /*Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }*/

    /*private void Update()
    {
        //if (effectIdx == BloodFX.Length) effectIdx = 0;
        //var instance = Instantiate(BloodFX[effectIdx], originTransform.position, Quaternion.identity);
        //effectIdx++;
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(rayOrigin, rayDirection);

        //Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        //RaycastHit hit;
        *//*if (Physics.Raycast(ray, out hit))
        {
            // var randRotation = new Vector3(0, Random.value * 360f, 0);
            // var dir = CalculateAngle(Vector3.forward, hit.normal);
            float angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;

            //var effectIdx = Random.Range(0, BloodFX.Length);
            if (effectIdx == BloodFX.Length) effectIdx = 0;

            var instance = Instantiate(BloodFX[effectIdx], hit.point, Quaternion.Euler(0, angle + 90, 0));
            effectIdx++;

            var settings = instance.GetComponent<BFX_BloodSettings>();
            //settings.FreezeDecalDisappearance = InfiniteDecal;
            settings.LightIntensityMultiplier = DirLight.intensity;

            *//*var nearestBone = originTransform;
            if (nearestBone != null)
            {
                var attachBloodInstance = Instantiate(BloodAttach);
                var bloodT = attachBloodInstance.transform;
                bloodT.position = hit.point;
                bloodT.localRotation = Quaternion.identity;
                bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
                bloodT.LookAt(hit.point + hit.normal, direction);
                bloodT.Rotate(90, 0, 0);
                bloodT.transform.parent = nearestBone;
                //Destroy(attachBloodInstance, 20);
            }*//*

            //if (!InfiniteDecal) Destroy(instance, 20);

        }*//*
    }*/
    

}
