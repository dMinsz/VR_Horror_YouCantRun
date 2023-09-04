using UnityEngine;

namespace ldw
{
    /// <summary>
    /// 물체 간의 움직임을 제어하고 물리시뮬레이션을 조절하는데 사용됨.
    /// </summary>
    public class TransformJoint : MonoBehaviour, ISerializationCallbackReceiver
    {
        const float k_MinMass = 0.01f;          // 최소 질량
        const float k_MaxForceDistance = 0.01f; // 최대 힘 거리

        [SerializeField]
        [Tooltip("연결된 다른 Transform을 참조하는 변수. 이 변수를 통해 다른 물체의 Transform과 Joint를 연결할 수 있음")]
        Transform m_ConnectedBody;

        [SerializeField]
        [Tooltip("Joint의 움직임이 제약되는 기준 앵커의 위치")]
        Vector3 m_Anchor;

        [SerializeField]
        [Tooltip("Joint의 움직임이 제약되는 기준 앵커의 회전")]
        Vector3 m_AnchorAngle;

        [SerializeField]
        [Tooltip("연결된 앵커를 자동으로 계산해야 하는지 여부")]
        bool m_AutoConfigureConnectedAnchor;

        [SerializeField]
        [Tooltip("연결된 Transform을 기준으로 한 앵커의 상대 위치")]
        Vector3 m_ConnectedAnchor;

        [SerializeField]
        [Tooltip("연결된 Transform을 기준으로 한 앵커의 상대 회전")]
        Vector3 m_ConnectedAnchorAngle;

        [SerializeField]
        [Tooltip("Joint로 연결된 물체 간의 충돌을 활성화할지 여부")]
        bool m_EnableCollision;

        [SerializeField]
        [Tooltip("Joint와 연결된 Transform사이에 장애물이 있을 경우 적용되는 기준 힘")]
        float m_BaseForce = 0.25f;

        [SerializeField]
        [Tooltip("Joint와 연결된 Transform사이의 거리를 기준으로 추가 적용되는 힘")]
        float m_SpringForce = 1f;

        [SerializeField]
        [Tooltip("Joint가 앵커로부터 일정 거리 이상 벗어나면 텔레포트 되는 거리")]
        float m_BreakDistance = 1.5f;

        [SerializeField]
        [Tooltip("Joint가 앵커로부터 일정 거리 이상 벗어나면 텔레포트 되는 각도")]
        float m_BreakAngle = 120f;

        [SerializeField]
        [Tooltip("회전 각도를 일치시킬지 여부")]
        bool m_MatchRotation = true;

        [SerializeField]
        [Tooltip("Rigidbody 의 질량을 임시로 조절해야 하는지 여부 (매우 강한 움직임을 안정화)")]
        bool m_AdjustMass = true;


        /// <summary>
        /// A reference to another transform this joint connects to.
        /// </summary>
        public Transform connectedBody
        {
            get => m_ConnectedBody;
            set
            {
                if (m_ConnectedBody == value)
                    return;

                m_ConnectedBody = value;
                SetupConnectedBodies(true);
            }
        }

        /// <summary>
        /// The Position of the anchor around which the joints motion is constrained.
        /// </summary>
        public Vector3 anchor
        {
            get => m_Anchor;
            set => m_Anchor = value;
        }

        /// <summary>
        /// The Rotation of the anchor around which the joints motion is constrained
        /// </summary>
        public Vector3 anchorAngle
        {
            get => m_AnchorAngle;
            set
            {
                m_AnchorAngle = value;
                m_AnchorRotation.eulerAngles = m_AnchorAngle;
            }
        }

        /// <summary>
        /// Should the connectedAnchor be calculated automatically?
        /// </summary>
        public bool autoConfigureConnectedAnchor
        {
            get => m_AutoConfigureConnectedAnchor;
            set
            {
                m_AutoConfigureConnectedAnchor = value;
                SetupConnectedBodies(true);
            }
        }

        /// <summary>
        /// Position of the anchor relative to the connected transform.
        /// </summary>
        public Vector3 connectedAnchor
        {
            get => m_ConnectedAnchor;
            set => m_ConnectedAnchor = value;
        }

        /// <summary>
        /// The Rotation of the anchor relative to the connected transform.
        /// </summary>
        public Vector3 connectedAnchorAngle
        {
            get => m_ConnectedAnchorAngle;
            set
            {
                m_ConnectedAnchorAngle = value;
                m_ConnectedAnchorRotation.eulerAngles = m_ConnectedAnchorAngle;
            }
        }

        /// <summary>
        /// Enable collision between bodies connected with the joint.
        /// </summary>
        public bool enableCollision
        {
            get => m_EnableCollision;
            set
            {
                m_EnableCollision = value;
                SetupConnectedBodies();
            }
        }

        /// <summary>
        /// Should the mass of the rigidbody be temporarily adjusted to stabilize very strong motion?
        /// </summary>
        public bool adjustMass
        {
            get => m_AdjustMass;
            set => m_AdjustMass = value;
        }

        /// <summary>
        /// Baseline force applied when an obstacle is between the joint and the connected transform.
        /// </summary>
        public float baseForce
        {
            get => m_BaseForce;
            set => m_BaseForce = value;
        }

        /// <summary>
        /// Additional force applied based on the distance between joint and connected transform
        /// </summary>
        public float springForce
        {
            get => m_SpringForce;
            set => m_SpringForce = value;
        }

        /// <summary>
        /// The distance this joint must be from the anchor before teleporting.
        /// </summary>
        public float breakDistance
        {
            get => m_BreakDistance;
            set => m_BreakDistance = value;
        }

        /// <summary>
        /// The angular distance this joint must be from the anchor before teleporting.
        /// </summary>
        public float breakAngle
        {
            get => m_BreakAngle;
            set => m_BreakAngle = value;
        }

        /// <summary>
        /// The angular distance this joint must be from the anchor before teleporting.
        /// </summary>
        public bool matchRotation
        {
            get => m_MatchRotation;
            set => m_MatchRotation = value;
        }


        Quaternion m_AnchorRotation;
        Quaternion m_ConnectedAnchorRotation;

        Transform m_Transform;
        Rigidbody m_Rigidbody;

        bool m_FixedSyncFrame;
        bool m_ActiveCollision;
        bool m_CollisionFrame;
        bool m_LastCollisionFrame;

        Vector3 m_LastPosition;
        Vector3 m_LastDirection;

        Collider m_SourceCollider;
        Collider m_ConnectedCollider;

        float m_BaseMass = 1f;
        float m_AppliedForce;
        float m_OldForce;

        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_SourceCollider = GetComponent<Collider>();

            m_Transform = transform;

            m_AnchorRotation.eulerAngles = m_AnchorAngle;
            m_ConnectedAnchorRotation.eulerAngles = m_ConnectedAnchorAngle;

            if (m_Rigidbody != null && m_Rigidbody.mass > k_MinMass)
                m_BaseMass = m_Rigidbody.mass;

            // Set up connected anchor if attached
            SetupConnectedBodies(true);
        }

        void OnDestroy()
        {
            if (m_Rigidbody != null)
                m_Rigidbody.mass = m_BaseMass;
        }

        void SetupConnectedBodies(bool updateAnchor = false)
        {
            // Handle undoing old setup
            // If any properties are pre-existing and have changed, reset the last saved collision ignore pairing
            if (m_SourceCollider != null && m_ConnectedCollider != null)
            {
                Physics.IgnoreCollision(m_SourceCollider, m_ConnectedCollider, false);
                m_ConnectedCollider = null;
            }

            // Handle current setup
            if (m_ConnectedBody != null)
            {
                if (m_AutoConfigureConnectedAnchor && updateAnchor)
                {
                    // Calculate what offsets are currently, set them as anchor
                    m_ConnectedAnchor = m_ConnectedBody.InverseTransformPoint(m_Rigidbody.position + Vector3.Scale((m_Rigidbody.rotation * m_Anchor), m_Transform.lossyScale));
                    m_ConnectedAnchorRotation = (m_Rigidbody.rotation * m_AnchorRotation);
                    m_ConnectedAnchorAngle = m_ConnectedAnchorRotation.eulerAngles;
                }
                if (m_EnableCollision)
                {
                    // Get collider on connected body
                    m_ConnectedCollider = m_ConnectedBody.GetComponent<Collider>();

                    if (m_SourceCollider != null && m_ConnectedCollider != null)
                    {
                        Physics.IgnoreCollision(m_SourceCollider, m_ConnectedCollider, true);
                    }
                }
            }
        }

        void LateUpdate()
        {
            // Move freely unless collision has occurred - then rely on physics
            if ((m_CollisionFrame || m_ActiveCollision) && !m_FixedSyncFrame)
            {
                m_Transform.position = m_Rigidbody.position;

                if (m_MatchRotation)
                    m_Transform.rotation = m_Rigidbody.rotation;
            }

            m_FixedSyncFrame = false;
        }

        void FixedUpdate()
        {
            m_FixedSyncFrame = true;
            m_OldForce = m_AppliedForce;
            m_AppliedForce = 0f;

            // Zero out any existing velocity, we are going to set force manually if needed
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;

            UpdateBufferedCollision();
            UpdatePosition();

            if (m_MatchRotation)
                UpdateRotation();

            if (m_AdjustMass)
            {
                var offset = (m_AppliedForce / m_BaseMass) * Time.fixedDeltaTime * Time.fixedDeltaTime * 0.5f;
                var massScale = Mathf.Max((offset / k_MaxForceDistance), 1f);
                m_Rigidbody.mass = m_BaseMass * massScale;
            }
            // and acc = f/m
            // offset = acc * fixedTimestep * fixedTimestep * .5

            // Is offset over certain desirable distance?  ie. .1m
            // scale offset down by scaling mass up
            // offset*scale = acc * ftp^2 * .5
            // offset = acc *
            //
            // Based on total force, scale mass
        }

        void UpdateBufferedCollision()
        {
            // We buffer collision over three updates
            // Once from the actual collision to the first fixed update (m_ActiveCollision)
            // Once for an entire fixedUpdate-to-fixedUpdate cycle (m_CollisionFrame)
            // And once when a collision is lost - to correct against potential errors when a moving a parent transform
            m_LastCollisionFrame = m_CollisionFrame;
            m_CollisionFrame = m_ActiveCollision;
            m_ActiveCollision = false;
        }

        void UpdatePosition()
        {
            // Assume transform is synced to the rigid body position from late update
            // Convert anchors to world space
            var worldSourceAnchor = m_Rigidbody.position + Vector3.Scale((m_Rigidbody.rotation * m_Anchor), m_Transform.lossyScale);
            var worldDestAnchor = m_ConnectedBody.TransformPoint(m_ConnectedAnchor);

            // Get the delta between these two positions
            // Use this to calculate the target world position for the rigidbody
            var positionDelta = worldDestAnchor - worldSourceAnchor;
            var offset = positionDelta.magnitude;
            var direction = positionDelta.normalized;
            var targetPos = m_Rigidbody.position + positionDelta;

            // Convert the target and actual positions to world space
            var worldPos = m_Rigidbody.position;

            if (offset > Mathf.Epsilon)
            {
                // Are we past the break distance?
                if (offset > m_BreakDistance)
                {
                    // Warp back to the target
                    m_Rigidbody.position = targetPos;
                    m_Transform.position = targetPos;
                    m_LastDirection = direction;
                    return;
                }

                // Can we move back unobstructed? Do that
                if (!m_CollisionFrame)
                {
                    if (m_Rigidbody.SweepTest(direction, out var hitInfo, offset))
                    {
                        targetPos = worldPos + (hitInfo.distance * direction);
                        m_CollisionFrame = true;
                    }
                    else
                    {
                        // If there was a collision during the previous update, we let one more update cycle pass at the current location
                        // This helps prevent teleporting through objects during scenarios where many things are playing into the object's position
                        if (m_LastCollisionFrame)
                        {
                            // Compare last direction to this direction
                            // If they are facing opposite directions, no worry of collision
                            if (Vector3.Dot(direction, m_LastDirection) > 0)
                            {
                                targetPos = worldPos;
                                m_AppliedForce = m_OldForce;
                            }
                        }
                    }
                    m_Rigidbody.position = targetPos;
                    m_Transform.position = targetPos;
                }

                if (m_CollisionFrame)
                {
                    // Apply a constant force based on spring logic
                    //Debug.Log(m_Rigidbody.velocity);
                    var force = (m_BaseForce + offset * m_SpringForce);
                    m_AppliedForce = force;
                    m_Rigidbody.AddForce(direction * force, ForceMode.Impulse);
                    m_LastPosition = m_Rigidbody.position;
                }
                m_LastDirection = direction;
            }
        }

        void UpdateRotation()
        {
            // Assume transform is synced to the rigid body position from late update
            // Convert anchor rotations to world space
            var worldSourceAnchor = m_Rigidbody.rotation * m_AnchorRotation;
            var worldDestAnchor = m_ConnectedBody.rotation * m_ConnectedAnchorRotation;

            // Get the delta between these two positions
            // Use this to calculate the target world position for the rigidbody
            var rotationDelta = worldDestAnchor * Quaternion.Inverse(worldSourceAnchor);
            var targetRotation = rotationDelta * m_Rigidbody.rotation;

            rotationDelta.ToAngleAxis(out var angleInDegrees, out var rotationAxis);
            if (angleInDegrees > 180f)
                angleInDegrees -= 360f;

            var angleOffset = Mathf.Abs(angleInDegrees);

            if (angleOffset > Mathf.Epsilon)
            {
                // Are we past the break distance?
                if (angleOffset > m_BreakAngle)
                {
                    // Warp back to the target
                    m_Rigidbody.rotation = targetRotation;
                    m_Transform.rotation = targetRotation;
                }

                // Can we move back unobstructed? Do that
                if (!m_CollisionFrame)
                {
                    m_Rigidbody.rotation = targetRotation;
                    m_Transform.rotation = targetRotation;
                }
                else
                {
                    var force = ((angleInDegrees / 360f) * (m_BaseForce + m_SpringForce));
                    m_Rigidbody.AddTorque(rotationAxis * force, ForceMode.Impulse);
                }
            }
        }

        void OnCollisionEnter()
        {
            // While in a collision state, we change state so that the regular transform/visual updates are locked to the fixed update rate
            m_ActiveCollision = true;
            m_CollisionFrame = true;
        }

        void OnCollisionStay()
        {
            m_ActiveCollision = true;
            m_CollisionFrame = true;
        }

        void OnCollisionExit()
        {
            if (!enabled)
                return;

            // When exiting collision, we lock to the last known rigidbody position.
            // This is because we can end up putting fairly strong forces on this object
            // If a parent or pure transform change invalidates the collision these forces can cause an object to move through things
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.position = m_LastPosition;
            transform.position = m_LastPosition;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            m_AnchorRotation.eulerAngles = m_AnchorAngle;
            m_ConnectedAnchorRotation.eulerAngles = m_ConnectedAnchorAngle;
        }
    }
}
