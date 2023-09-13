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
        mannequin = GetComponentInParent<Mannequin>();
        coll = GetComponentInParent<Collider>();
    }

    private void Start()
    {
        StartCoroutine(FindMainCamera());
    }

    void LateUpdate()
    {
        // ī�޶� �þ� ���� ���� Collider�� �����ȴٸ�
        if (cam == null)
        {
            Debug.Log("cam is Null");
            return;
        }
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustum, GetComponent<Collider>().bounds))
        {
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // �� ������Ʈ���� ī�޶� ��������, distToTarget ���� RayCast ���?
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

    IEnumerator FindMainCamera()
    {
        yield return new WaitUntil(() => { return Camera.main != null; });

        cam = Camera.main;
        Debug.Log($"{gameObject.name}: MainCam ã��");

        yield break;
    }

}
