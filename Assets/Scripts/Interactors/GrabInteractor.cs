using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    /// <summary>
    /// XRGrabInteractable�� Ȯ���Ͽ� ��ü�� ��Ʈ�ѷ� ������ ��ġ�� ȸ�� �������� �����ϵ��� ������ ��ũ��Ʈ
    /// </summary>
    public class GrabInteractor : XRGrabInteractable
    {
        class SavedTransform
        {
            public Vector3 OriginalPosition;    // ���� ��ġ
            public Quaternion OriginalRotation; // ���� ȸ��
        }

        // savedTransforms ������ XRBaseInteractor�� SavedTransform ������ ������ �����ϴ� Dictionary
        Dictionary<XRBaseInteractor, SavedTransform> savedTransforms = new Dictionary<XRBaseInteractor, SavedTransform>();
        Rigidbody rb;

        // [Header("���̾� ����ũ")]
        // [SerializeField] private LayerMask interactionLayerMask = ~0;

        protected override void Awake()
        {
            base.Awake();

            // ���� Base Class ���� �̹� ���������� �ܺο� ������� �����Ƿ� �ٽ� ������
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            // ���� ��ȣ�ۿ��ϴ� interactorObject�� ������
            var interactor = args.interactorObject;

            // ���� interactor�� DirectInteractor���� Ȯ��
            if (interactor is XRDirectInteractor directInteractor)
            {
                // ���� ��ġ�� ȸ���� �����ִ� savedTransform instance ����
                SavedTransform savedTransform = new SavedTransform
                {
                    OriginalPosition = directInteractor.attachTransform.localPosition,
                    OriginalRotation = directInteractor.attachTransform.localRotation
                };

                // savedTrasnforms[directInteractor] �� �ش� interactor�� SavedTrasnform�� �����Ͽ� ����
                savedTransforms[directInteractor] = savedTransform;

                // attachTransform�� �Ҵ�Ǿ� �ִ��� ���θ� ����
                bool haveAttach = attachTransform != null;

                // ��ü�� ��ġ�� ȸ���� ��Ʈ�ѷ��� ��ġ�� ȸ������ ����
                directInteractor.attachTransform.position = haveAttach ? attachTransform.position : rb.worldCenterOfMass;
                directInteractor.attachTransform.rotation = haveAttach ? attachTransform.rotation : rb.rotation;
            }

            base.OnSelectEntered(args);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            var interactor = args.interactorObject;
            if (interactor is XRDirectInteractor directInteractor)
            {
                // Dictionary�� ����� SavedTransform ���� ������
                if (savedTransforms.TryGetValue(directInteractor, out SavedTransform savedTransform))
                {
                    // ���� ��ġ�� ȸ������ ����
                    directInteractor.attachTransform.localPosition = savedTransform.OriginalPosition;
                    directInteractor.attachTransform.localRotation = savedTransform.OriginalRotation;

                    // Dictionary���� �ش� interactor�� ������ ����
                    savedTransforms.Remove(directInteractor);
                }
            }

            base.OnSelectExited(args);
        }

        // �ش� ��ü�� ���� �������� ���θ� �����ϴ� �������̵� �Լ� -> ���� �ʿ�
        [System.Obsolete]
        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            int interactorLayerMask = 1 << interactor.gameObject.layer;
            return base.IsSelectableBy(interactor) && (interactionLayerMask.value & interactorLayerMask) != 0;
        }
    }
}