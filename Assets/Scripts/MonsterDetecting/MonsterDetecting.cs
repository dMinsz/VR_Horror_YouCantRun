using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MonsterDetecting : MonoBehaviour
{
    [SerializeField] Mannequin mannequin;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float plusY;
    Collider collider;
    Plane[] cameraFrustum;

    private void Awake()
    {
        mannequin = GetComponentInParent<Mannequin>();
        collider = GetComponentInParent<Collider>();
    }

    /*
    private void OnBecameVisible()
    {            
        if (mannequin.CurState != Mannequin_State.Dormant)
        mannequin.MannequinBecameVisible();
    }

    private void OnBecameInvisible()
    {
        if (mannequin.CurState != Mannequin_State.Dormant)
            mannequin.MannequinBecameInvisible();
    }
    */

    void LateUpdate()
    {
        // 카메라 시야 내에 나의 Collider가 감지된다면
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, collider.bounds))
        {
            Debug.Log($"X :{transform.position.x} Y : {transform.position.y + plusY} Z : { transform.position.z}");
            // 1.레이케스트
            Vector3 dirTarget = (transform.position - cam.transform.position).normalized; // 이 오브젝트에서 카메라의 방향 일반화

            // 2. 장애물 확인
            float distToTarget = Vector3.Distance(transform.position, cam.transform.position); // 이 오브젝트에서 카메라까지 거리 
            Debug.Log($"몬스터와 카메라간 거리 {distToTarget}");
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // 이 오브젝트에서 카메라 방향으로, distToTarget 까지 RayCast 쏘기 
            {
                Debug.Log("몬스터 감지불가");
                if (mannequin.CurState != Mannequin_State.Dormant)
                    mannequin.MannequinBecameInvisible();
                return;
            }

            Debug.Log("몬스터 감지");
            if (mannequin.CurState != Mannequin_State.Dormant)
                mannequin.MannequinBecameVisible();
        } else
        {
            Debug.Log("몬스터 감지불가");
            if (mannequin.CurState != Mannequin_State.Dormant)
                mannequin.MannequinBecameInvisible();

        }
    }    
}
