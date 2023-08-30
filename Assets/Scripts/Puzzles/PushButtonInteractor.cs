using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw.Interactor
{
    public class PushButtonInteractor : XRBaseInteractable
    {
        // 상호작용하는 개별 Interactor에 대한 정보를 저장하는 class.
        // 이 정보는 버튼을 누르는 동안 버튼에 대한 인터랙터의 위치와 동작 상태를 추적하는데 사용됨.
        class PressInfo
        {
            internal IXRHoverInteractor m_Interactor;
            internal bool m_InPressRegion = false;
            internal bool m_WrongSide = false;
        }

        [Serializable]
        public class ValueChangeEvent : UnityEvent<float> { }

        [SerializeField]
        [Tooltip("The object that is visually pressed down")] // 눌린 버튼을 시각적으로 나타내기 위한 Transform 객체
        Transform m_Button = null;

        [SerializeField]
        [Tooltip("The distance the button can be pressed")] // 버튼이 눌릴 수 있는 최대 거리
        float m_PressDistance = 0.1f;

        [SerializeField]
        [Tooltip("Extra distance for clicking the button down")]
        float m_PressBuffer = 0.01f;

        [SerializeField]
        [Tooltip("Offset from the button base to start testing for push")]
        float m_ButtonOffset = 0.0f;

        [SerializeField]
        [Tooltip("How big of a surface area is available for pressing the button")]
        float m_ButtonSize = 0.1f;

        [SerializeField]
        [Tooltip("Treat this button like an on/off toggle")] // 버튼을 Toggle로 사용할 지 여부를 나타냄
        bool m_ToggleButton = false;

        [SerializeField]
        [Tooltip("Events to trigger when the button is pressed")] // 버튼을 누를 때 호출되는 UnityEvent
        UnityEvent m_OnPress;

        [SerializeField]
        [Tooltip("Events to trigger when the button is released")] // 버튼을 놓을 때 호출되는 UnityEvent
        UnityEvent m_OnRelease;

        [SerializeField]
        [Tooltip("Events to trigger when the button pressed value is updated. Only called when the button is pressed")]
        ValueChangeEvent m_OnValueChange;

        bool m_Pressed = false;
        bool m_Toggled = false;
        float m_Value = 0f;
        Vector3 m_BaseButtonPosition = Vector3.zero;

        Dictionary<IXRHoverInteractor, PressInfo> m_HoveringInteractors = new Dictionary<IXRHoverInteractor, PressInfo>();

        /// <summary>
        /// The object that is visually pressed down
        /// </summary>
        public Transform button
        {
            get => m_Button;
            set => m_Button = value;
        }

        /// <summary>
        /// The distance the button can be pressed
        /// </summary>
        public float pressDistance
        {
            get => m_PressDistance;
            set => m_PressDistance = value;
        }

        /// <summary>
        /// The distance (in percentage from 0 to 1) the button is currently being held down
        /// </summary>
        public float value => m_Value;

        /// <summary>
        /// Events to trigger when the button is pressed
        /// </summary>
        public UnityEvent onPress => m_OnPress;

        /// <summary>
        /// Events to trigger when the button is released
        /// </summary>
        public UnityEvent onRelease => m_OnRelease;

        /// <summary>
        /// Events to trigger when the button distance value is changed. Only called when the button is pressed
        /// </summary>
        public ValueChangeEvent onValueChange => m_OnValueChange;

        /// <summary>
        /// Whether or not a toggle button is in the locked down position
        /// </summary>
        public bool toggleValue
        {
            get => m_ToggleButton && m_Toggled;
            set
            {
                if (!m_ToggleButton)
                    return;

                m_Toggled = value;
                if (m_Toggled)
                    SetButtonHeight(-m_PressDistance);
                else
                    SetButtonHeight(0.0f);
            }
        }

        public override bool IsHoverableBy(IXRHoverInteractor interactor)
        {
            if (interactor is XRRayInteractor)
                return false;

            return base.IsHoverableBy(interactor);
        }

        void Start()
        {
            if (m_Button != null)
                m_BaseButtonPosition = m_Button.position;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_Toggled)
                SetButtonHeight(-m_PressDistance);
            else
                SetButtonHeight(0.0f);

            hoverEntered.AddListener(StartHover);
            hoverExited.AddListener(EndHover);
        }

        protected override void OnDisable()
        {
            hoverEntered.RemoveListener(StartHover);
            hoverExited.RemoveListener(EndHover);
            base.OnDisable();
        }

        void StartHover(HoverEnterEventArgs args)
        {
            m_HoveringInteractors.Add(args.interactorObject, new PressInfo { m_Interactor = args.interactorObject });
        }

        void EndHover(HoverExitEventArgs args)
        {
            m_HoveringInteractors.Remove(args.interactorObject);

            if (m_HoveringInteractors.Count == 0)
            {
                if (m_ToggleButton && m_Toggled)
                    SetButtonHeight(-m_PressDistance);
                else
                    SetButtonHeight(0.0f);
            }
        }

        // 상호작용이 발생할 때 마다 호출되는 함수. 버튼을 누르는 동작을 처리
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (m_HoveringInteractors.Count > 0)
                {
                    UpdatePress();
                }
            }
        }

        // Interactor의 위치와 Button의 위치를 기준으로 버튼이 눌리는 정도를 계산하고, 버튼의 상태와 값을 업데이트
        void UpdatePress()
        {
            var minimumHeight = 0.0f;

            if (m_ToggleButton && m_Toggled)
                minimumHeight = -m_PressDistance;

            // Go through each interactor
            foreach (var pressInfo in m_HoveringInteractors.Values)
            {
                var interactorTransform = pressInfo.m_Interactor.GetAttachTransform(this);
                var localOffset = transform.InverseTransformVector(interactorTransform.position - m_BaseButtonPosition);

                var withinButtonRegion = (Mathf.Abs(localOffset.x) < m_ButtonSize && Mathf.Abs(localOffset.z) < m_ButtonSize);
                if (withinButtonRegion)
                {
                    if (!pressInfo.m_InPressRegion)
                    {
                        pressInfo.m_WrongSide = (localOffset.y < m_ButtonOffset);
                    }

                    if (!pressInfo.m_WrongSide)
                        minimumHeight = Mathf.Min(minimumHeight, localOffset.y - m_ButtonOffset);
                }

                pressInfo.m_InPressRegion = withinButtonRegion;
            }

            minimumHeight = Mathf.Max(minimumHeight, -(m_PressDistance + m_PressBuffer));

            // If button height goes below certain amount, enter press mode
            var pressed = m_ToggleButton ? (minimumHeight <= -(m_PressDistance + m_PressBuffer)) : (minimumHeight < -m_PressDistance);

            var currentDistance = Mathf.Max(0f, -minimumHeight - m_PressBuffer);
            m_Value = currentDistance / m_PressDistance;

            if (m_ToggleButton)
            {
                if (pressed)
                {
                    if (!m_Pressed)
                    {
                        m_Toggled = !m_Toggled;

                        if (m_Toggled)
                            m_OnPress.Invoke();
                        else
                            m_OnRelease.Invoke();
                    }
                }
            }
            else
            {
                if (pressed)
                {
                    if (!m_Pressed)
                        m_OnPress.Invoke();
                }
                else
                {
                    if (m_Pressed)
                        m_OnRelease.Invoke();
                }
            }
            m_Pressed = pressed;

            // Call value change event
            if (m_Pressed)
                m_OnValueChange.Invoke(m_Value);

            SetButtonHeight(minimumHeight);
        }

        void SetButtonHeight(float height)
        {
            if (m_Button == null)
                return;

            Vector3 newPosition = m_Button.localPosition;
            newPosition.y = height;
            m_Button.localPosition = newPosition;
        }

        // 버튼 눌림 영역을 시각화
        void OnDrawGizmosSelected()
        {
            var pressStartPoint = Vector3.zero;

            if (m_Button != null)
            {
                pressStartPoint = m_Button.localPosition;
            }

            pressStartPoint.y += m_ButtonOffset - (m_PressDistance * 0.5f);

            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(pressStartPoint, new Vector3(m_ButtonSize, m_PressDistance, m_ButtonSize));
        }

        // 스크립트가 수정될 때 버튼의 높이를 업데이트하여 Scene View 에서 변경 사항을 실시간으로 확인할 수 있도록 함
        void OnValidate()
        {
            SetButtonHeight(0.0f);
        }
    }
}
