using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    private Rigidbody rb;
    private HingeJoint joint;

    public float MaxAngle = 120f;

    public DoorAssist assist;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
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
            /*if (joint.angle >= 1 && joint.angle < 20)
            {
                Debug.Log("1<5");
                rb.AddForce(transform.forward * -200f, ForceMode.Impulse);
                assist.OffColliders();
            }
            if (joint.angle > -20 && joint.angle <= -1)
            {
                Debug.Log("-1<-5");
                rb.AddForce(transform.forward * 200f, ForceMode.Impulse);
                assist.OffColliders();
            }*/
            if (joint.angle >= MaxAngle || joint.angle <= -MaxAngle)
            {
                rb.isKinematic = true;

                assist.OnColliders();
                assist.DestroyHandle();

                Debug.Log("Max");
                break;
            }
            yield return null;
        }
    }

    public void KeyCardInteract()
    {
        // 카드 키 상호작용
        rb.isKinematic = false;
    }

    public void PushDoor()
    {
        if (joint.angle >= 25f)
        {
            Debug.Log(">=20");
            rb.AddForce(Vector3.right * -500f, ForceMode.Impulse);
            assist.OffColliders();
        }
        else if (joint.angle >= 1f && joint.angle < 20f)
        {
            Debug.Log("1<20");
            rb.AddForce(Vector3.right * -1000f, ForceMode.Impulse);
            assist.OffColliders();
        }
        else if (joint.angle > -20f && joint.angle <= -1f)
        {
            Debug.Log("-1<-20");
            rb.AddForce(Vector3.left * 1000f, ForceMode.Impulse);
            assist.OffColliders();
        }
        else if (joint.angle <= -25f)
        {
            Debug.Log("<=-20");
            rb.AddForce(Vector3.left * 500f, ForceMode.Impulse);
            assist.OffColliders();
        }
    }
}
