using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    public class PushButtonInteractor : XRBaseInteractable
    {
        // ��ȣ�ۿ��ϴ� ���� Interactor�� ���� ������ �����ϴ� class.
        // �� ������ ��ư�� ������ ���� ��ư�� ���� ���ͷ����� ��ġ�� ���� ���¸� �����ϴµ� ����.
        class PressInfo
        {
            internal IXRHoverInteractor buttonInteractor;   // ��ư�� ��ȣ�ۿ��ϴ� Interactor
            internal bool inButtonPressRegion = false;      // ��ư�� ������ ���� �ȿ� �ִ��� bool��
            internal bool wrongButtonSide = false;          // ��ư�� �ƴѰ��� ��ġ�ϴ��� bool��
        }

        [Serializable]
        public class ValueChangeEvent : UnityEvent<float> { }

        [SerializeField]
        [Tooltip("���� ��ư�� �ð������� ��Ÿ���� ���� Transform ��ü")]
        Transform buttonTransform = null;

        [SerializeField]
        [Tooltip("��ư�� ���� �� �ִ� �ִ� �Ÿ�")] 
        float buttonPressDistance = 0.1f;

        [SerializeField]
        [Tooltip("��ư�� ������ ���� �߰� �Ÿ�")]
        float buttonPressBuffer = 0.01f;

        [SerializeField]
        [Tooltip("������ ��ư�� ��ư ���̽��κ����� ��ġ")]
        float buttonTransformOffset = 0.0f;

        [SerializeField]
        [Tooltip("��ư�� ���� �� �ִ� ǥ���� ũ��")]
        float buttonTransformSize = 0.1f;

        [SerializeField]
        [Tooltip("��ư�� Toggle�� ����� �� ���θ� ��Ÿ��")]
        bool isToggleButton = false;

        [SerializeField]
        [Tooltip("��ư�� ���� �� ȣ��Ǵ� UnityEvent")]
        UnityEvent OnPressEvent;

        [SerializeField]
        [Tooltip("��ư�� ���� �� ȣ��Ǵ� UnityEvent")]
        UnityEvent OnReleaseEvent;

        [SerializeField]
        [Tooltip("��ư�� ���� ����Ǹ� ȣ��Ǵ� �̺�Ʈ")]
        ValueChangeEvent OnValueChangedEvent;

        bool isPressed = false; // ��ư�� ���� �����ִ���
        bool isToggled = false; // ��ư�� ���� Toggle ��������
        float buttonValue = 0f;     // ��ư�� ��(���� ����)
        Vector3 baseButtonPosition = Vector3.zero;    // ��ư�� ���̽� ��ġ

        // ��ȣ�ۿ��ϴ� Interactor �� PressInfo ������ �����ϴ� Dictionary
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

        // ��ȣ�ۿ��� �߻��� �� ���� ȣ��Ǵ� �Լ�. ��ư�� ������ ������ ó��
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

        // Interactor�� ��ġ�� Button�� ��ġ�� �������� ��ư�� ������ ������ ����ϰ�, ��ư�� ���¿� ���� ������Ʈ
        void UpdatePress()
        {
            var minimumHeight = 0.0f;   // ��ư�� ���� ����. �ʱⰪ�� 0f

            if (isToggleButton && isToggled)
                minimumHeight = -buttonPressDistance;

            // Interactor ���� ��ȸ
            foreach (var pressInfo in hoverInteractors.Values)
            {
                // ��ȣ�ۿ����� Transform
                var interactorTransform = pressInfo.buttonInteractor.GetAttachTransform(this);
                // ��ư�� ��ȣ�ۿ����� ��� ��ġ
                var localOffset = transform.InverseTransformVector(interactorTransform.position - baseButtonPosition);

                // Interactor�� ��ư ���� ������ �ִ��� ���
                var withinButtonRegion = (Mathf.Abs(localOffset.x) < buttonTransformSize && Mathf.Abs(localOffset.z) < buttonTransformSize);
                if (withinButtonRegion)
                {
                    if (!pressInfo.inButtonPressRegion)
                    {
                        pressInfo.wrongButtonSide = (localOffset.y < buttonTransformOffset);
                    }
                    
                    // �ùٸ� ��ġ���� ��ư�� ���� ���, minimumHeight ������Ʈ
                    if (!pressInfo.wrongButtonSide)
                        minimumHeight = Mathf.Min(minimumHeight, localOffset.y - buttonTransformOffset);
                }

                // ���� �ȿ� ������ inButtonPressRegion ���� true��
                pressInfo.inButtonPressRegion = withinButtonRegion;
            }

            minimumHeight = Mathf.Max(minimumHeight, -(buttonPressDistance + buttonPressBuffer));

            var pressed = isToggleButton ? (minimumHeight <= -(buttonPressDistance + buttonPressBuffer)) : (minimumHeight < -buttonPressDistance);

            // currentDistance : ���� ��ư�� �󸶳� ���ȴ���
            var currentDistance = Mathf.Max(0f, -minimumHeight - buttonPressBuffer);
            // buttonValue : ��ư�� ���� ����
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

        // ��ư ���� ������ �ð�ȭ
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

        // ��ũ��Ʈ�� ������ �� ��ư�� ���̸� ������Ʈ�Ͽ� Scene View ���� ���� ������ �ǽð����� Ȯ���� �� �ֵ��� ��
        void OnValidate()
        {
            SetButtonHeight(0.0f);
        }
    }
}
