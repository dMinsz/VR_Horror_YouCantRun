using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    private Rigidbody rb;
    private HingeJoint joint;

    private Collider coll;
    public float MaxAngle = 120f;

    public DoorAssist assist;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        //rb.isKinematic = false;
        StartCoroutine(StopAngle());
    }

    IEnumerator StopAngle()
    {
        while (true)
        {
            if (joint.angle >= 10)
            {
                /*transform.rotation = Quaternion.Euler(transform.rotation.x, 120, transform.rotation.z);
                yield return new WaitUntil(() => joint.angle >= MaxAngle);*/
                assist.OffColliders();
                rb.AddForce(transform.forward * -50f, ForceMode.Acceleration);
            }

            if (joint.angle <= -10)
            {
                /*transform.rotation = Quaternion.Euler(transform.rotation.x, -120, transform.rotation.z);

                yield return new WaitUntil(() => joint.angle <= -MaxAngle);*/
                assist.OffColliders();
                rb.AddForce(Vector3.forward * 50f, ForceMode.Acceleration);
            }

            if (joint.angle >= MaxAngle || joint.angle <= -MaxAngle)
            {
                coll.enabled = false;
                rb.isKinematic = true;

                assist.OnColliders();
                assist.DestroyHandle();

                break;
            }
            yield return null;
        }
    }

    public void Test()
    {
        // 카드 키 상호작용
        gameObject.SetActive(false);
    }
}
