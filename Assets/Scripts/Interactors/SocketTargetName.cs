using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    /// <summary>
    /// SocketInteractor�� �Բ� �ۿ��ϴ� Ư���� ��ũ��Ʈ.
    /// Socket Type�� �����ϰ�, �� SocketType�� SocketInteractor�� SocketType�� ��ġ���� �ʴ� ���
    /// �ش� ������ ��ȿ�� ������� �������� ����.
    /// </summary>
    [RequireComponent(typeof(XRBaseInteractable))]
    public class SocketTargetName : MonoBehaviour
    {
        public string SocketType;               // SocketType ����
        public SelectEnterEvent SocketedEvent;  // ���Ͽ� ��ü�� �������� �� ����Ǵ� �̺�Ʈ
        public bool DisableSocketOnSocketed;    // ���Ͽ� ��ü�� �������� �� ������ ��Ȱ��ȭ �� �� ���θ� ����

        void Awake()
        {
            var interactable = GetComponent<XRBaseInteractable>();

            // ����(Select) �̺�Ʈ�� �߻����� �� SelectedSwitch �޼��带 ȣ��
            interactable.selectEntered.AddListener(SelectedSwitch);
        }

        // ���Ͽ� ��ü�� �������� �� ������ ó���ϴ� �Լ�
        public void SelectedSwitch(SelectEnterEventArgs args)
        {
            var interactor = args.interactorObject;
            var socketInteractor = interactor as SocketInteractor;

            // ���� ���ͷ��Ͱ� SocketInteractor���� Ȯ��
            if (socketInteractor == null)
                return;

            // SocketType�� ���� ���ͷ����� acceptedType�� ��ġ���� ������ ó������ ����
            if (SocketType != socketInteractor.acceptedType)
                return;

            // Ȥ�� DisableSocketOnSocketed �� true��� ���Ͽ� ��ü�� ������ �� 0.5�� �� ���� ��Ȱ��ȭ
            if (DisableSocketOnSocketed)
            {
                // ������ ������ �� ���� �ð��� ���� �� ������ ��Ȱ��ȭ�ϴ� �ڷ�ƾ�� ����
                // ���� ��Ȱ��ȭ�� ���� �� ���� ����� ����� �� ���� / ������� ���� ���� ����
                StartCoroutine(DisableSocketDelayed(socketInteractor));
            }

            // SocketedEvent �̺�Ʈ�� ȣ��
            SocketedEvent.Invoke(args);
        }

        IEnumerator DisableSocketDelayed(SocketInteractor interactor)
        {
            // 0.5���� ���� �Ŀ� ������ ��Ȱ��ȭ
            yield return new WaitForSeconds(0.5f);
            interactor.socketActive = false;
        }
    }
}