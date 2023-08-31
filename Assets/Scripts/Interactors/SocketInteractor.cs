using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    public class SocketInteractor : XRSocketInteractor
    {
        public string acceptedType;

        // Obsolete -> 수정 필요
        [System.Obsolete]
        public override bool CanSelect(XRBaseInteractable interactable)
        {
            SocketTargetName socketTargetName = interactable.GetComponent<SocketTargetName>();

            if (socketTargetName == null)
                return false;

            // socketTargetName과 같을 경우만 선택 가능
            return base.CanSelect(interactable) && (socketTargetName.SocketType == acceptedType);
        }

        [System.Obsolete]
        public override bool CanHover(XRBaseInteractable interactable)
        {
            return CanSelect(interactable);
        }
    }
}