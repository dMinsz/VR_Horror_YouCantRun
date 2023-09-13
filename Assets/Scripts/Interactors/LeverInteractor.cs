using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    /// <summary>
    /// ���� ��ȣ�ۿ��ڿ� ���� �����ų� �� �� �ִ� ���� ������ ��ȣ�ۿ� ������ ��ü
    /// </summary>
    public class LeverInteractor : XRBaseInteractable
    {
        const float k_LeverDeadZone = 0.1f; // �߾ӿ� ��Ȯ�� ��ġ�� �� Ȱ��/��Ȱ��ȭ�� ��� ��ȯ�Ǵ� ���� ����(������)

        [SerializeField]
        [Tooltip("�ð������� ��� �����ϴ� ���� ��ü")]
        Transform m_Handle = null;

        [SerializeField]
        [Tooltip("������ ��")]
        bool m_Value = false;

        [SerializeField]
        [Tooltip("Ȱ��ȭ/��Ȱ��ȭ ���¿��� ������ ������ �� �� ��ġ�� ��� �̵����� ����")]
        bool m_LockToValue;

        [SerializeField]
        [Tooltip("������ '����' ��ġ������ ����")]
        [Range(-90.0f, 90.0f)]
        float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("������ '����' ��ġ������ ����")]
        [Range(-90.0f, 90.0f)]
        float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("������ Ȱ��ȭ�� �� Ʈ������ �̺�Ʈ��")]
        UnityEvent m_OnLeverActivate = new UnityEvent();

        [SerializeField]
        [Tooltip("������ ��Ȱ��ȭ�� �� Ʈ������ �̺�Ʈ��")]
        UnityEvent m_OnLeverDeactivate = new UnityEvent();

        IXRSelectInteractor m_Interactor;

        /// <summary>
        /// �ð������� ��� �����ϴ� ���� ��ü
        /// </summary>
        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        /// <summary>
        /// ������ ��
        /// </summary>
        public bool value
        {
            get => m_Value;
            set => SetValue(value, true);
        }

        /// <summary>
        /// Ȱ��ȭ/��Ȱ��ȭ ���¿��� ������ ������ �� �� ��ġ�� ��� �̵����� ����
        /// </summary>
        public bool lockToValue { get; set; }

        /// <summary>
        /// ������ '����' ��ġ�� ����
        /// </summary>
        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        /// <summary>
        /// ������ '����' ��ġ�� ����
        /// </summary>
        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        /// <summary>
        /// ������ Ȱ��ȭ�� �� Ʈ������ �̺�Ʈ��
        /// </summary>
        public UnityEvent onLeverActivate => m_OnLeverActivate;

        /// <summary>
        /// ������ ��Ȱ��ȭ�� �� Ʈ������ �̺�Ʈ��
        /// </summary>
        public UnityEvent onLeverDeactivate => m_OnLeverDeactivate;

        // ���� ������ ���� �ʱ�ȭ�ϰ� ������ ������ ����
        void Start()
        {
            SetValue(m_Value, true);
        }

        // ��ȣ�ۿ� ������ ��ü�� Ȱ��ȭ�� �� ȣ��
        // ���õ� ��쿡 ���� ó���� �����ϱ� ���� select Entered/Exited �̺�Ʈ������ �߰�
        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        // ��ȣ�ۿ� ������ ��ü�� ��Ȱ��ȭ�� �� ȣ��
        // select EventListener ����
        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        // ���õ� ��쿡 ȣ��, ������ ����� �� ����
        // ��ȣ�ۿ��ڸ� m_Interactor �� �Ҵ�
        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
        }

        // ������ ������ �� ȣ��, ������ ������ �� ����
        // ���� ������ ���� �����ϰ� ��ȣ�ۿ��ڸ� �ʱ�ȭ
        void EndGrab(SelectExitEventArgs args)
        {
            SetValue(m_Value, true);
            m_Interactor = null;
        }

        // Interaction ������Ʈ �ܰ� �� Dynamic �ܰ迡�� ȣ��
        // ������ ���õ� ��쿡�� ����, ������ ���� ������Ʈ�ϰ� ������ ó���� ����
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

        // ������ ���� ��ȣ�ۿ����� �ü� ������ ����Ͽ� ��ȯ
        // ������ ��ġ�� ��ȣ�ۿ����� ��ġ ���̸� ����ϰ� �̸� ���� ��ǥ��� ��ȯ�Ͽ� ��ȯ
        Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        // ������ ���� ������Ʈ�ϰ� ������ ó���� ����
        // ��ȣ�ۿ����� �ü� ������ �������� ������ ������ ����ϰ�, �׿� ���� ������ ���� ������Ʈ
        void UpdateValue()
        {
            // �ü� ���� ȹ��
            var lookDirection = GetLookDirection();
            // �ü� ������ ���
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.z) * Mathf.Rad2Deg;

            // �ּ� ������ �ִ� �������̿��� ������ ����
            if (m_MinAngle < m_MaxAngle)
                lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            else
                lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);

            // �ִ� ������ ���� ���� ������ �Ÿ� / �ּ� ������ ���� ���� ������ �Ÿ��� ���
            var maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
            var minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);

            // ���� ������ ���� true�̸� �ִ� ���� �Ÿ��� ���� �������� ����, �׷��� ������ �ּ� �Ÿ��� ����
            if (m_Value)
                maxAngleDistance *= (1.0f - k_LeverDeadZone);
            else
                minAngleDistance *= (1.0f - k_LeverDeadZone);

            // �ִ� ���� �Ÿ��� �ּ� ���� �Ÿ����� ������ ���ο� ���� true, �ƴϸ� false
            var newValue = (maxAngleDistance < minAngleDistance);

            // �ڵ� ���� ����
            SetHandleAngle(lookAngle);

            // newValue�� ���� �� ����
            SetValue(newValue);
        }

        // ������ ���� �����ϰ� �ش� ���� ���� ó���� ����
        // ���� ���� ���ο� ���� �����鼭 ���� ȸ���Ȱ� �ƴ϶�� ������ ������ ������ ��
        // ���� ����� ��� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ �̺�Ʈ�� ȣ��, ������ ���õ��� ���� ��� �� ��ġ�� ������ ȸ��
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

        // ���� �ڵ��� ������ �����ϴ� �Լ�
        // ���� �ڵ��� ���� ȸ���� ������ ������ ������
        void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        }

        // ������ Ȱ��ȭ ����, ��Ȱ��ȭ ������ ���� ����� �׸�
        // ������ Ȱ��ȭ�Ǵ� ����� ��Ȱ��ȭ�Ǵ� ������ �ð������� ��Ÿ��
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
