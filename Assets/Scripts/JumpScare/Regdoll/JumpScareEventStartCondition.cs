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
        cam = Camera.main;
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
            //if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // 이 오브젝트에서 카메라 방향으로, distToTarget 까지 RayCast 쏘기 
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask)) // 이 오브젝트에서 카메라 방향으로, distToTarget 까지 RayCast 쏘기 
            {
                return;
            }

            // 시야에 FocusObject가 있다.
            // JumpScareBase의 FocusTime 동안 응시한다면 SpawnObejct 함수 호출
            if (nowFocusTime >= owner.FocusTime)
            {
                owner.SpawnObejct();
            }
            nowFocusTime += Time.deltaTime;
        }
        else
        {
            // 플레이어 시야에 FocusObject가 없다.
        }
    }
}
