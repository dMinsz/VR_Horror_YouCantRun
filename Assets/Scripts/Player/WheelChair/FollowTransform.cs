using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [Tooltip("Transform of the rigidbody to follow.")]
    public Transform target;
    Vector3 offset;

    void Start()
    {
        offset = transform.localPosition - target.localPosition;
    }

    void Update()
    {
        Vector3 rotatedOffset = target.localRotation * offset;
        transform.localPosition = target.localPosition + rotatedOffset;

        transform.rotation = target.rotation;
    }
}
