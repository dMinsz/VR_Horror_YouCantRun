using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareEventStartCondition : MonoBehaviour
{
    [SerializeField] JumpScareRegdollAction owner;
    [SerializeField] LayerMask obstacleMask;
    Camera cam;
    Collider col;
    Plane[] cameraFrustum;
    float nowFocusTime;
    Coroutine findCam;

    private bool isRunning;

    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }

    private void Awake()
    {
        isRunning = false;
        owner = GetComponentInParent<JumpScareRegdollAction>();
        col = GetComponentInParent<Collider>();
    }

    private void Start()
    {
        findCam = StartCoroutine(FindMainCamera());
    }

    public void PlayerIn()
    {
        if (isRunning)
            return;
        isRunning = true;
    }

    public void PlayerOut()
    {
        if (!isRunning)
            return;
        isRunning = false;
    }

    void LateUpdate()
    {
        if (!isRunning)
            return;

        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, col.bounds))
        {
            //if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // �� ������Ʈ���� ī�޶� ��������, distToTarget ���� RayCast ��� 
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // �� ������Ʈ���� ī�޶� ��������, distToTarget ���� RayCast ��� 
            {
                // ���ع��� �����Ѵ�.
                Debug.Log($"���ع� ���� : {hit.collider.gameObject.name}");
                return;
            }

            // �þ߿� FocusObject�� �ִ�.
            // JumpScareBase�� FocusTime ���� �����Ѵٸ� SpawnObejct �Լ� ȣ��
            Debug.Log("�÷��̾� ����");
            if (nowFocusTime >= owner.FocusTime)
            {
                owner.SpawnObejct();
            }
            nowFocusTime += Time.deltaTime;
        }
        else
        {
            // �÷��̾� �þ߿� FocusObject�� ����.
            Debug.Log("����X");
        }
    }

    IEnumerator FindMainCamera()
    {
        yield return new WaitUntil(() => { return Camera.main != null; });

        cam = Camera.main;

        yield break;
    }

    private void OnDisable()
    {
        StopCoroutine(findCam);
    }
}
