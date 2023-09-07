using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelInteractable : XRGrabInteractable
{

    //[Tooltip("Choose Left Hand Or Right Hand")]
    //public XRNode trackedNode;


    public float wheelStopTorque = 25f;
    Rigidbody rb;

    float wheelRadius;

    bool onSlope = false;
    [SerializeField] bool hapticsEnabled = true;

    [Range(0, 0.5f), Tooltip("Distance from wheel collider at which the interaction manager will cancel selection.")]
    [SerializeField] float deselectionThreshold = 0.25f;

    GameObject grabPoint;

    public Text label1;
    public Text label2;

    WheelMoveAssist assist;


    #region Make Attactch Point

    //protected override void Awake()
    //{
    //    base.Awake();
    //    CreateAttachTransform();
    //}
    //protected override void OnSelectEntering(SelectEnterEventArgs args)
    //{
    //    base.OnSelectEntering(args);
    //    MatchAttachPoint(args.interactorObject);
    //}

    //protected void MatchAttachPoint(IXRInteractor interactor)
    //{
    //    if (IsFirstSelecting(interactor))
    //    {
    //        bool isDirect = interactor is XRDirectInteractor;
    //        attachTransform.position = isDirect ? interactor.GetAttachTransform(this).position : transform.position;
    //        attachTransform.rotation = isDirect ? interactor.GetAttachTransform(this).rotation : transform.rotation;
    //    }
    //}

    //private bool IsFirstSelecting(IXRInteractor interactor)
    //{
    //    return interactor == firstInteractorSelecting;
    //}

    //private void CreateAttachTransform()
    //{
    //    if (attachTransform == null)
    //    {
    //        GameObject createdAttachTransform = new GameObject();
    //        createdAttachTransform.transform.parent = this.transform;
    //        attachTransform = createdAttachTransform.transform;
    //    }
    //}

    #endregion

    private void Start()
    {
        assist = GetComponent<WheelMoveAssist>();
        rb = GetComponent<Rigidbody>();
        wheelRadius = GetComponent<SphereCollider>().radius;

        // Slope check is run in coroutine at optimized intervals.
        StartCoroutine(CheckForSlope());
    }

    protected override void OnSelectEntered(SelectEnterEventArgs eventArgs)
    {
        base.OnSelectEntered(eventArgs);

        XRBaseInteractor interactor = (XRBaseInteractor)eventArgs.interactorObject;

        interactionManager.CancelInteractableSelection((IXRSelectInteractable)this);

        //float gripValue = 0f;
        //InputDevices.GetDeviceAtXRNode(trackedNode).TryGetFeatureValue(CommonUsages.grip, out gripValue); // 1f 일때 꽉잡은거

        SpawnGrabPoint(interactor);


        StartCoroutine(BrakeAssist(interactor));

        StartCoroutine(MonitorDetachDistance(interactor));

        if (hapticsEnabled)
        {
            StartCoroutine(SendHapticFeedback(interactor));
        }


    }

    /// <summary>
    /// Generates a grab point to mediate physics interaction with the wheel's rigidbody. This "grab
    /// point" contains an XRGrabInteractable component, as well as a rigidbody. It's fused to the wheel using a Fixed Joint.
    /// </summary>
    /// <param name="interactor">Interactor which is making the selection.</param>
    void SpawnGrabPoint(XRBaseInteractor interactor)
    {
        // If there is an active grab point, cancel selection.
        if (grabPoint)
        {
            interactionManager.CancelInteractableSelection(grabPoint.GetComponent<IXRSelectInteractable>());
        }

        // Instantiate new grab point at interactor's position.
        grabPoint = new GameObject($"{transform.name}'s grabPoint", typeof(VRWC_GrabPoint), typeof(Rigidbody), typeof(FixedJoint));

        grabPoint.transform.position = interactor.transform.position;

        // Attach grab point to this wheel using fixed joint.
        grabPoint.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();

        // Force selection between current interactor and new grab point.
        //interactionManager.ForceSelect((XRBaseInteractor)interactor, grabPoint.GetComponent<XRGrabInteractable>());

        interactionManager.SelectEnter(interactor, grabPoint.GetComponent<IXRSelectInteractable>());
    }

    IEnumerator BrakeAssist(XRBaseInteractor interactor)
    {
        HandVelocitySupplier interactorVelocity = interactor.GetComponent<HandVelocitySupplier>();

        if (interactorVelocity != null)
        {
            while (grabPoint)
            {
                // If the interactor's forward/backward movement approximates zero, it is considered to be braking.
                if (interactorVelocity.velocity.z < 0.05f && interactorVelocity.velocity.z > -0.05f)
                {
                    rb.AddTorque(-rb.angularVelocity.normalized * wheelStopTorque);

                    SpawnGrabPoint(interactor);
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator MonitorDetachDistance(XRBaseInteractor interactor)
    {
        // While this wheel has an active grabPoint.
        while (grabPoint)
        {
            // If interactor drifts beyond the threshold distance from wheel, force deselection.
            if (Vector3.Distance(transform.position, interactor.transform.position) >= wheelRadius + deselectionThreshold)
            {
                interactionManager.CancelInteractorSelection((IXRSelectInteractor)interactor);
            }

            yield return null;
        }
    }

    IEnumerator SendHapticFeedback(XRBaseInteractor interactor)
    {
        // Interval between iterations of coroutine, in seconds.
        float runInterval = 0.1f;

        ActionBasedController controller = interactor.GetComponent<ActionBasedController>();


        if (controller != null)
        {
            Vector3 lastAngularVelocity = new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x, 0f, 0f);

            while (grabPoint)
            {
                Vector3 currentAngularVelocity = new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x, 0f, 0f);
                Vector3 angularAcceleration = (currentAngularVelocity - lastAngularVelocity) / runInterval;

                // If current velocity and acceleration have perpendicular or opposite directions, the wheel is decelerating.
                if (Vector3.Dot(currentAngularVelocity.normalized, angularAcceleration.normalized) < 0f)
                {
                    float impulseAmplitude = Mathf.Abs(angularAcceleration.x);

                    if (impulseAmplitude > 1.5f)
                    {
                        float remappedImpulseAmplitude = Remap(impulseAmplitude, 1.5f, 40f, 0f, 1f);

                        controller.SendHapticImpulse(remappedImpulseAmplitude, runInterval * 2f);
                    }
                }

                lastAngularVelocity = currentAngularVelocity;
                yield return new WaitForSeconds(runInterval);
            }
        }
    }

    /// <summary>
    /// This is a utility method which remaps a float value from one range to another.
    /// </summary>
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        //float normal = Mathf.InverseLerp(aLow, aHigh, value);
        //float bValue = Mathf.Lerp(bLow, bHigh, normal);
    }

    IEnumerator CheckForSlope()
    {
        while (true)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit))
            {
                onSlope = hit.normal != Vector3.up;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
