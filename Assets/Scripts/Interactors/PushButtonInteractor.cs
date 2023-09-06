using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    public class PushButtonInteractor : XRBaseInteractable
    {
        // 상호작용하는 개별 Interactor에 대한 정보를 저장하는 class.
        // 이 정보는 버튼을 누르는 동안 버튼에 대한 인터랙터의 위치와 동작 상태를 추적하는데 사용됨.
        class PressInfo
        {
            internal IXRHoverInteractor buttonInteractor;   // 버튼과 상호작용하는 Interactor
            internal bool inButtonPressRegion = false;      // 버튼이 눌리는 영역 안에 있는지 bool값
            internal bool wrongButtonSide = false;          // 버튼이 아닌곳에 위치하는지 bool값
        }

        [Serializable]
        public class ValueChangeEvent : UnityEvent<float> { }

        [SerializeField]
        [Tooltip("눌린 버튼을 시각적으로 나타내기 위한 Transform 객체")]
        Transform buttonTransform = null;

        [SerializeField]
        [Tooltip("버튼을 누를 수 있는 최대 거리")] 
        float buttonPressDistance = 0.1f;

        [SerializeField]
        [Tooltip("버튼을 누르기 위한 추가 거리")]
        float buttonPressBuffer = 0.01f;

        [SerializeField]
        [Tooltip("누르는 버튼의 버튼 베이스로부터의 위치")]
        float buttonTransformOffset = 0.0f;

        [SerializeField]
        [Tooltip("버튼을 누를 수 있는 표면적 크기")]
        float buttonTransformSize = 0.1f;

        [SerializeField]
        [Tooltip("버튼을 Toggle로 사용할 지 여부를 나타냄")]
        bool isToggleButton = false;

        [SerializeField]
        [Tooltip("버튼을 누를 때 호출되는 UnityEvent")]
        UnityEvent OnPressEvent;

        [SerializeField]
        [Tooltip("버튼을 놓을 때 호출되는 UnityEvent")]
        UnityEvent OnReleaseEvent;

        [SerializeField]
        [Tooltip("버튼의 값이 변경되면 호출되는 이벤트")]
        ValueChangeEvent OnValueChangedEvent;

        bool isPressed = false; // 버튼이 현재 눌려있는지
        bool isToggled = false; // 버튼이 현재 Toggle 상태인지
        float buttonValue = 0f;     // 버튼의 값(눌린 정도)
        Vector3 baseButtonPosition = Vector3.zero;    // 버튼의 베이스 위치

        // 상호작용하는 Interactor 의 PressInfo 정보를 저장하는 Dictionary
        Dictionary<IXRHoverInteractor, PressInfo> hoverInteractors = new Dictionary<IXRHoverInteractor, PressInfo>();

        public Transform button
        {
            get => buttonTransform;
            set => buttonTransform = value;
        }

        public float pressDistance
        {
            get => buttonPressDistance;
            set => buttonPressDistance = value;
        }

        public float value => buttonValue;

        public UnityEvent onPress => OnPressEvent;

        public UnityEvent onRelease => OnReleaseEvent;

        public ValueChangeEvent onValueChange => OnValueChangedEvent;

        public bool toggleValue
        {
            get => isToggleButton && isToggled;
            set
            {
                if (!isToggleButton)
                    return;

                isToggled = value;
                if (isToggled)
                    SetButtonHeight(-buttonPressDistance);
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
            if (buttonTransform != null)
                baseButtonPosition = buttonTransform.position;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (isToggled)
                SetButtonHeight(-buttonPressDistance);
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
            hoverInteractors.Add(args.interactorObject, new PressInfo { buttonInteractor = args.interactorObject });
            Debug.Log("Hoverd");
        }

        void EndHover(HoverExitEventArgs args)
        {
            hoverInteractors.Remove(args.interactorObject);

            if (hoverInteractors.Count == 0)
            {
                if (isToggleButton && isToggled)
                    SetButtonHeight(-buttonPressDistance);
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
                if (hoverInteractors.Count > 0)
                {
                    UpdatePress();
                }
            }
        }

        // Interactor의 위치와 Button의 위치를 기준으로 버튼이 눌리는 정도를 계산하고, 버튼의 상태와 값을 업데이트
        void UpdatePress()
        {
            var minimumHeight = 0.0f;   // 버튼이 눌린 정도. 초기값은 0f

            if (isToggleButton && isToggled)
                minimumHeight = -buttonPressDistance;

            // Interactor 정보 순회
            foreach (var pressInfo in hoverInteractors.Values)
            {
                // 상호작용자의 Transform
                var interactorTransform = pressInfo.buttonInteractor.GetAttachTransform(this);
                // 버튼과 상호작용자의 상대 위치
                var localOffset = transform.InverseTransformVector(interactorTransform.position - baseButtonPosition);

                // Interactor가 버튼 눌림 영역에 있는지 계산
                var withinButtonRegion = (Mathf.Abs(localOffset.x) < buttonTransformSize && Mathf.Abs(localOffset.z) < buttonTransformSize);
                if (withinButtonRegion)
                {
                    if (!pressInfo.inButtonPressRegion)
                    {
                        pressInfo.wrongButtonSide = (localOffset.y < buttonTransformOffset);
                    }
                    
                    // 올바른 위치에서 버튼을 누른 경우, minimumHeight 업데이트
                    if (!pressInfo.wrongButtonSide)
                        minimumHeight = Mathf.Min(minimumHeight, localOffset.y - buttonTransformOffset);
                }

                // 영역 안에 있으면 inButtonPressRegion 값을 true로
                pressInfo.inButtonPressRegion = withinButtonRegion;
            }

            minimumHeight = Mathf.Max(minimumHeight, -(buttonPressDistance + buttonPressBuffer));

            var pressed = isToggleButton ? (minimumHeight <= -(buttonPressDistance + buttonPressBuffer)) : (minimumHeight < -buttonPressDistance);

            // currentDistance : 현재 버튼이 얼마나 눌렸는지
            var currentDistance = Mathf.Max(0f, -minimumHeight - buttonPressBuffer);
            // buttonValue : 버튼의 눌린 정도
            buttonValue = currentDistance / buttonPressDistance;

            if (isToggleButton)
            {
                if (pressed)
                {
                    if (!isPressed)
                    {
                        isToggled = !isToggled;

                        if (isToggled)
                            OnPressEvent.Invoke();
                        else
                            OnReleaseEvent.Invoke();
                    }
                }
            }
            else
            {
                if (pressed)
                {
                    if (!isPressed)
                        OnPressEvent.Invoke();
                }
                else
                {
                    if (isPressed)
                        OnReleaseEvent.Invoke();
                }
            }
            isPressed = pressed;

            // Call value change event
            if (isPressed)
                OnValueChangedEvent.Invoke(buttonValue);

            SetButtonHeight(minimumHeight);
        }

        void SetButtonHeight(float height)
        {
            if (buttonTransform == null)
                return;

            Vector3 newPosition = buttonTransform.localPosition;
            newPosition.y = height;
            buttonTransform.localPosition = newPosition;
        }

        // 버튼 눌림 영역을 시각화
        void OnDrawGizmosSelected()
        {
            var pressStartPoint = Vector3.zero;

            if (buttonTransform != null)
            {
                pressStartPoint = buttonTransform.localPosition;
            }

            pressStartPoint.y += buttonTransformOffset - (buttonPressDistance * 0.5f);

            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(pressStartPoint, new Vector3(buttonTransformSize, buttonPressDistance, buttonTransformSize));
        }

        // 스크립트가 수정될 때 버튼의 높이를 업데이트하여 Scene View 에서 변경 사항을 실시간으로 확인할 수 있도록 함
        void OnValidate()
        {
            SetButtonHeight(0.0f);
        }
    }
}
