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
    Collider coll;
    Plane[] cameraFrustum;

    private void Awake()
    {
        cam = Camera.main;
        mannequin = GetComponentInParent<Mannequin>();
        coll = GetComponentInParent<Collider>();
    }

    void LateUpdate()
    {
        // ī�޶� �þ� ���� ���� Collider�� �����ȴٸ�
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, GetComponent<Collider>().bounds))
        {
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // �� ������Ʈ���� ī�޶� ��������, distToTarget ���� RayCast ��� 
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
