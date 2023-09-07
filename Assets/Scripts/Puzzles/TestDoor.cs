using System.Collections;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    private Rigidbody rb;
    private HingeJoint joint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
    }

    private void Start()
    {
        rb.isKinematic = false;
        StartCoroutine(StopAngle());
    }

    IEnumerator StopAngle()
    {
        while (true)
        {
            if (joint.angle == 120 || joint.angle == -120)
            {
                rb.isKinematic = true;

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
