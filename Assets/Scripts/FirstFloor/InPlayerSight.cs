using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InPlayerSight : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask obstacleMask;
    Plane[] cameraFrustum;
    public UnityEvent inSight;
    bool playerSawThis;

    private void Start()
    {
        StartCoroutine(FindMainCamera());
    }

    void LateUpdate()
    {
        if (playerSawThis)
            return;
        if (cam == null)
        {
            return;
        }
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustum, GetComponent<Collider>().bounds))
        {
            if (Physics.Linecast(transform.position, cam.transform.position, out var hit, obstacleMask))
            {
                return;
            }

            //보고있음.
            Debug.Log("in Player Sight");
            GameManager.Sound.PlaySound("JumpScare_1",Audio.SFX,transform.position,0.6f);
            playerSawThis = true;
            inSight?.Invoke();
        }
    }

    IEnumerator FindMainCamera()
    {
        yield return new WaitUntil(() => { return Camera.main != null; });

        cam = Camera.main;

        yield break;
    }
}
