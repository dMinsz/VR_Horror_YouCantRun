using UnityEngine;
using UnityEngine.XR;

public class HandVelocitySupplier : MonoBehaviour
{
    [SerializeField, Tooltip("which velocity should be tracked. This should be LeftHand or RightHand")]
    XRNode trackedNode;

    Vector3 _velocity = Vector3.zero;

    public Vector3 velocity { get => _velocity; }

    private void Start()
    {
        InputDevices.GetDeviceAtXRNode(trackedNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out _velocity);
    }

    void Update()
    {
        InputDevices.GetDeviceAtXRNode(trackedNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out _velocity);
    }
}
