using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    public class SocketInteractor : XRSocketInteractor
    {
        public string acceptedType;

        // Obsolete -> ���� �ʿ�
        [System.Obsolete]
        public override bool CanSelect(XRBaseInteractable interactable)
        {
            SocketTargetName socketTargetName = interactable.GetComponent<SocketTargetName>();

            if (socketTargetName == null)
                return false;

            // socketTargetName�� ���� ��츸 ���� ����
            return base.CanSelect(interactable) && (socketTargetName.SocketType == acceptedType);
        }

        [System.Obsolete]
        public override bool CanHover(XRBaseInteractable interactable)
        {
            return CanSelect(interactable);
        }
    }
}