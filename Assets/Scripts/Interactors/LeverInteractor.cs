using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    /// <summary>
    /// 직접 상호작용자에 의해 켜지거나 끌 수 있는 레버 형태의 상호작용 가능한 객체
    /// </summary>
    public class LeverInteractor : XRBaseInteractable
    {
        const float k_LeverDeadZone = 0.1f; // 중앙에 정확히 위치할 때 활성/비활성화가 즉시 전환되는 것을 방지(데드존)

        [SerializeField]
        [Tooltip("시각적으로 잡고 조작하는 레버 객체")]
        Transform m_Handle = null;

        [SerializeField]
        [Tooltip("레버의 값")]
        bool m_Value = false;

        [SerializeField]
        [Tooltip("활성화/비활성화 상태에서 레버가 놓여질 때 값 위치로 즉시 이동할지 여부")]
        bool m_LockToValue;

        [SerializeField]
        [Tooltip("레버의 '켜진' 위치에서의 각도")]
        [Range(-90.0f, 90.0f)]
        float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("레버의 '꺼진' 위치에서의 각도")]
        [Range(-90.0f, 90.0f)]
        float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("레버가 활성화될 때 트리거할 이벤트들")]
        UnityEvent m_OnLeverActivate = new UnityEvent();

        [SerializeField]
        [Tooltip("레버가 비활성화될 때 트리거할 이벤트들")]
        UnityEvent m_OnLeverDeactivate = new UnityEvent();

        IXRSelectInteractor m_Interactor;

        /// <summary>
        /// 시각적으로 잡고 조절하는 레버 객체
        /// </summary>
        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        /// <summary>
        /// 레버의 값
        /// </summary>
        public bool value
        {
            get => m_Value;
            set => SetValue(value, true);
        }

        /// <summary>
        /// 활성화/비활성화 상태에서 레버가 놓여질 때 값 위치로 즉시 이동할지 여부
        /// </summary>
        public bool lockToValue { get; set; }

        /// <summary>
        /// 레버의 '켜진' 위치의 각도
        /// </summary>
        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        /// <summary>
        /// 레버의 '꺼진' 위치의 각도
        /// </summary>
        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        /// <summary>
        /// 레버가 활성화될 때 트리거할 이벤트들
        /// </summary>
        public UnityEvent onLeverActivate => m_OnLeverActivate;

        /// <summary>
        /// 레버가 비활성화될 때 트리거할 이벤트들
        /// </summary>
        public UnityEvent onLeverDeactivate => m_OnLeverDeactivate;

        // 현재 레버의 값을 초기화하고 설정된 값으로 설정
        void Start()
        {
            SetValue(m_Value, true);
        }

        // 상호작용 가능한 객체가 활성화될 때 호출
        // 선택된 경우에 대한 처리를 수행하기 위해 select Entered/Exited 이벤트리스너 추가
        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        // 상호작용 가능한 객체가 비활성화될 때 호출
        // select EventListener 제거
        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        // 선택된 경우에 호출, 레버를 잡았을 때 실행
        // 상호작용자를 m_Interactor 에 할당
        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
        }

        // 선택이 해제될 때 호출, 레버를 놓았을 때 실행
        // 현재 레버의 값을 설정하고 상호작용자를 초기화
        void EndGrab(SelectExitEventArgs args)
        {
            SetValue(m_Value, true);
            m_Interactor = null;
        }

        // Interaction 업데이트 단계 중 Dynamic 단계에서 호출
        // 레버가 선택된 경우에만 실행, 레버의 값을 업데이트하고 적절한 처리를 수행
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateValue();
                }
            }
        }

        // 레버를 잡은 상호작용자의 시선 방향을 계산하여 반환
        // 레버의 위치와 상호작용자의 위치 차이를 계산하고 이를 로컬 좌표계로 변환하여 반환
        Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        // 레버의 값을 업데이트하고 적절한 처리를 수행
        // 상호작용자의 시선 방향을 기준으로 레버의 각도를 계산하고, 그에 따라 레버의 값을 업데이트
        void UpdateValue()
        {
            // 시선 방향 획득
            var lookDirection = GetLookDirection();
            // 시선 각도를 계산
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.z) * Mathf.Rad2Deg;

            // 최소 각도와 최대 각도사이에서 각도를 제한
            if (m_MinAngle < m_MaxAngle)
                lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            else
                lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);

            // 최대 각도와 현재 각도 사이의 거리 / 최소 각도와 현재 각도 사이의 거리를 계산
            var maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
            var minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);

            // 현재 레버의 값이 true이면 최대 각도 거리에 레버 데드존을 적용, 그렇지 않으면 최소 거리에 적용
            if (m_Value)
                maxAngleDistance *= (1.0f - k_LeverDeadZone);
            else
                minAngleDistance *= (1.0f - k_LeverDeadZone);

            // 최대 각도 거리가 최소 각도 거리보다 작으면 새로운 값은 true, 아니면 false
            var newValue = (maxAngleDistance < minAngleDistance);

            // 핸들 각도 설정
            SetHandleAngle(lookAngle);

            // newValue로 레버 값 설정
            SetValue(newValue);
        }

        // 레버의 값을 설정하고 해당 값에 따른 처리를 수행
        // 기존 값과 새로운 값이 같으면서 강제 회전된게 아니라면 레버의 각도를 설정만 함
        // 값이 변경될 경우 활성화 또는 비활성화 이벤트를 호출, 레버가 선택되지 않을 경우 값 위치로 레버를 회전
        void SetValue(bool isOn, bool forceRotation = false)
        {
            if (m_Value == isOn)
            {
                if (forceRotation)
                    SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);

                return;
            }

            m_Value = isOn;

            if (m_Value)
            {
                m_OnLeverActivate.Invoke();
            }
            else
            {
                m_OnLeverDeactivate.Invoke();
            }

            if (!isSelected && (m_LockToValue || forceRotation))
                SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }

        // 레버 핸들의 각도를 수정하는 함수
        // 레버 핸들의 로컬 회전을 설정된 각도로 변경함
        void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        }

        // 레버의 활성화 각도, 비활성화 각도에 대한 기즈모를 그림
        // 레버가 활성화되는 방향과 비활성화되는 방향을 시각적으로 나타냄
        void OnDrawGizmosSelected()
        {
            var angleStartPoint = transform.position;

            if (m_Handle != null)
                angleStartPoint = m_Handle.position;

            const float k_AngleLength = 0.25f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        void OnValidate()
        {
            SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }
    }
}
