using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    public class GripButtonInteractor : XRBaseInteractable
    {
        [SerializeField]
        [Tooltip("��ư�� �ð������� �������ٰ� ǥ�õǴ� ������Ʈ")]
        Transform m_Button = null;

        [SerializeField]
        [Tooltip("��ư�� ���� �� �ִ� �Ÿ�")]
        float m_PressDistance = 0.1f;

        [SerializeField]
        [Tooltip("��ư�� ��/���� ��۷� �ٷ� ������ ����")]
        bool m_ToggleButton = false;

        [SerializeField]
        [Tooltip("��ư�� ������ �� Ʈ���ŵ� �̺�Ʈ��")]
        UnityEvent m_OnPress;

        [SerializeField]
        [Tooltip("��ư�� �������� �� Ʈ���ŵ� �̺�Ʈ��")]
        UnityEvent m_OnRelease;

        bool m_Hovered = false;     // ��ư�� ȣ��(hover) ������ ����
        bool m_Selected = false;    // ��ư�� ���õ� �������� ����
        bool m_Toggled = false;     // ��ư�� ��۵Ǿ����� ����

        // �ð������� ������ ��ư ������Ʈ�� �Ӽ�
        public Transform button
        {
            get => m_Button;
            set => m_Button = value;
        }

        // ��ư�� ���� �� �ִ� �Ÿ��� �Ӽ�
        public float pressDistance
        {
            get => m_PressDistance;
            set => m_PressDistance = value;
        }

        // ��ư�� ������ �� Ʈ���ŵ� �̺�Ʈ�� �Ӽ�
        public UnityEvent onPress => m_OnPress;

        // ��ư�� �������� �� Ʈ���ŵ� �̺�Ʈ�� �Ӽ�
        public UnityEvent onRelease => m_OnRelease;

        // ��ũ��Ʈ�� ���۵� �� ȣ��Ǵ� �Լ�
        void Start()
        {
            SetButtonHeight(0.0f);
        }

        // ������Ʈ�� Ȱ��ȭ�� �� ȣ��Ǵ� �Լ� (���)
        protected override void OnEnable()
        {
            base.OnEnable();

            // ��� ��ư�� ��� selectEntered �̺�Ʈ�� StartTogglePress �Լ��� ����
            if (m_ToggleButton)
                selectEntered.AddListener(StartTogglePress);
            // �ƴ� ��� ���� �̺�Ʈ�� �� �Լ��� ����
            else
            {
                selectEntered.AddListener(StartPress);
                selectExited.AddListener(EndPress);
                hoverEntered.AddListener(StartHover);
                hoverExited.AddListener(EndHover);
            }
        }

        // ������Ʈ�� ��Ȱ��ȭ�� �� ȣ��Ǵ� �Լ� (���)
        protected override void OnDisable()
        {
            // ��� ��ư�� ��� selectEntered �̺�Ʈ���� StartTogglePress �Լ��� ����
            if (m_ToggleButton)
                selectEntered.RemoveListener(StartTogglePress);
            // �ƴ� ��� ���� �̺�Ʈ���� �� �Լ��� �����ϰ�, �⺻ OnDisable �Լ� ȣ��
            else
            {
                selectEntered.RemoveListener(StartPress);
                selectExited.RemoveListener(EndPress);
                hoverEntered.RemoveListener(StartHover);
                hoverExited.RemoveListener(EndHover);
                base.OnDisable();
            }
        }

        // ��� ��ư�� ��� ��ư�� ����ϴ� �Լ�
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

        // ��ư�� ���� ��� ��ư�� ������ �Լ�
        void StartPress(SelectEnterEventArgs args)
        {
            SetButtonHeight(-m_PressDistance);
            m_OnPress.Invoke();
            m_Selected = true;
        }

        // ��ư�� ������ ��� ��ư�� ���� �Լ�
        void EndPress(SelectExitEventArgs args)
        {
            if (m_Hovered)
                m_OnRelease.Invoke();

            SetButtonHeight(0.0f);
            m_Selected = false;
        }

        // ��ư�� ȣ�� ���� �� ȣ��Ǵ� �Լ�
        void StartHover(HoverEnterEventArgs args)
        {
            m_Hovered = true;
            if (m_Selected)
                SetButtonHeight(-m_PressDistance);
        }

        // ��ư�� ȣ���� ����� �� ȣ��Ǵ� �Լ�
        void EndHover(HoverExitEventArgs args)
        {
            m_Hovered = false;
            SetButtonHeight(0.0f);
        }

        // ��ư�� ���̸� �����ϴ� �Լ�
        void SetButtonHeight(float height)
        {
            if (m_Button == null)
                return;

            Vector3 newPosition = m_Button.localPosition;
            newPosition.y = height;
            m_Button.localPosition = newPosition;
        }

        // ���ӿ��� ���õ� ��ư�� �ð������� ǥ���ϱ� ���� ����� �׸��� �Լ�
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

        // �ν����Ϳ��� ���� ����� �� ȣ��Ǵ� �Լ�
        void OnValidate()
        {
            SetButtonHeight(0.0f);
        }
    }
}
