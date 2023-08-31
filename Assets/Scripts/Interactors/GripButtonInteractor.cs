using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    public class GripButtonInteractor : XRBaseInteractable
    {
        [SerializeField]
        [Tooltip("버튼이 시각적으로 눌려진다고 표시되는 오브젝트")]
        Transform m_Button = null;

        [SerializeField]
        [Tooltip("버튼이 눌릴 수 있는 거리")]
        float m_PressDistance = 0.1f;

        [SerializeField]
        [Tooltip("버튼을 온/오프 토글로 다룰 것인지 여부")]
        bool m_ToggleButton = false;

        [SerializeField]
        [Tooltip("버튼이 눌렸을 때 트리거될 이벤트들")]
        UnityEvent m_OnPress;

        [SerializeField]
        [Tooltip("버튼이 떼어졌을 때 트리거될 이벤트들")]
        UnityEvent m_OnRelease;

        bool m_Hovered = false;     // 버튼이 호버(hover) 중인지 여부
        bool m_Selected = false;    // 버튼이 선택된 상태인지 여부
        bool m_Toggled = false;     // 버튼이 토글되었는지 여부

        // 시각적으로 눌려진 버튼 오브젝트의 속성
        public Transform button
        {
            get => m_Button;
            set => m_Button = value;
        }

        // 버튼이 눌릴 수 있는 거리의 속성
        public float pressDistance
        {
            get => m_PressDistance;
            set => m_PressDistance = value;
        }

        // 버튼이 눌렸을 때 트리거될 이벤트의 속성
        public UnityEvent onPress => m_OnPress;

        // 버튼이 떼어졌을 때 트리거될 이벤트의 속성
        public UnityEvent onRelease => m_OnRelease;

        // 스크립트가 시작될 때 호출되는 함수
        void Start()
        {
            SetButtonHeight(0.0f);
        }

        // 오브젝트가 활성화될 때 호출되는 함수 (상속)
        protected override void OnEnable()
        {
            base.OnEnable();

            // 토글 버튼인 경우 selectEntered 이벤트에 StartTogglePress 함수를 연결
            if (m_ToggleButton)
                selectEntered.AddListener(StartTogglePress);
            // 아닌 경우 각종 이벤트에 각 함수를 연결
            else
            {
                selectEntered.AddListener(StartPress);
                selectExited.AddListener(EndPress);
                hoverEntered.AddListener(StartHover);
                hoverExited.AddListener(EndHover);
            }
        }

        // 오브젝트가 비활성화될 때 호출되는 함수 (상속)
        protected override void OnDisable()
        {
            // 토글 버튼인 경우 selectEntered 이벤트에서 StartTogglePress 함수를 제거
            if (m_ToggleButton)
                selectEntered.RemoveListener(StartTogglePress);
            // 아닌 경우 각종 이벤트에서 각 함수를 제거하고, 기본 OnDisable 함수 호출
            else
            {
                selectEntered.RemoveListener(StartPress);
                selectExited.RemoveListener(EndPress);
                hoverEntered.RemoveListener(StartHover);
                hoverExited.RemoveListener(EndHover);
                base.OnDisable();
            }
        }

        // 토글 버튼인 경우 버튼을 토글하는 함수
        void StartTogglePress(SelectEnterEventArgs args)
        {
            m_Toggled = !m_Toggled;

            if (m_Toggled)
            {
                SetButtonHeight(-m_PressDistance);
                m_OnPress.Invoke();
            }
            else
            {
                SetButtonHeight(0.0f);
                m_OnRelease.Invoke();
            }
        }

        // 버튼이 눌린 경우 버튼을 누르는 함수
        void StartPress(SelectEnterEventArgs args)
        {
            SetButtonHeight(-m_PressDistance);
            m_OnPress.Invoke();
            m_Selected = true;
        }

        // 버튼이 떼어진 경우 버튼을 떼는 함수
        void EndPress(SelectExitEventArgs args)
        {
            if (m_Hovered)
                m_OnRelease.Invoke();

            SetButtonHeight(0.0f);
            m_Selected = false;
        }

        // 버튼이 호버 중일 때 호출되는 함수
        void StartHover(HoverEnterEventArgs args)
        {
            m_Hovered = true;
            if (m_Selected)
                SetButtonHeight(-m_PressDistance);
        }

        // 버튼이 호버를 벗어났을 때 호출되는 함수
        void EndHover(HoverExitEventArgs args)
        {
            m_Hovered = false;
            SetButtonHeight(0.0f);
        }

        // 버튼의 높이를 설정하는 함수
        void SetButtonHeight(float height)
        {
            if (m_Button == null)
                return;

            Vector3 newPosition = m_Button.localPosition;
            newPosition.y = height;
            m_Button.localPosition = newPosition;
        }

        // 게임에서 선택된 버튼을 시각적으로 표시하기 위한 기즈모를 그리는 함수
        void OnDrawGizmosSelected()
        {
            var pressStartPoint = transform.position;
            var pressDownDirection = -transform.up;

            if (m_Button != null)
            {
                pressStartPoint = m_Button.position;
                pressDownDirection = -m_Button.up;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(pressStartPoint, pressStartPoint + (pressDownDirection * m_PressDistance));
        }

        // 인스펙터에서 값이 변경될 때 호출되는 함수
        void OnValidate()
        {
            SetButtonHeight(0.0f);
        }
    }
}
