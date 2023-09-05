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
        cam = Camera.main;
        mannequin = GetComponentInParent<Mannequin>();
        collider = GetComponentInParent<Collider>();
    }

    void LateUpdate()
    {
        // 카메라 시야 내에 나의 Collider가 감지된다면
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, collider.bounds))
        {
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // 이 오브젝트에서 카메라 방향으로, distToTarget 까지 RayCast 쏘기 
            {
                if (mannequin.CurState != Mannequin_State.Dormant)
                    mannequin.MannequinBecameInvisible();
                return;
            }

            if (mannequin.CurState != Mannequin_State.Dormant)
                mannequin.MannequinBecameVisible();
        } else
        {
            if (mannequin.CurState != Mannequin_State.Dormant)
                mannequin.MannequinBecameInvisible();

        }
    }    
}
