using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    /// <summary>
    /// XRGrabInteractable을 확장하여 물체와 컨트롤러 사이의 위치와 회전 오프셋을 유지하도록 변경한 스크립트
    /// </summary>
    public class GrabInteractor : XRGrabInteractable
    {
        class SavedTransform
        {
            public Vector3 OriginalPosition;    // 기존 위치
            public Quaternion OriginalRotation; // 기존 회전
        }

        // savedTransforms 변수는 XRBaseInteractor와 SavedTransform 사이의 매핑을 저장하는 Dictionary
        Dictionary<XRBaseInteractor, SavedTransform> savedTransforms = new Dictionary<XRBaseInteractor, SavedTransform>();
        Rigidbody rb;

        // [Header("레이어 마스크")]
        // [SerializeField] private LayerMask interactionLayerMask = ~0;

        protected override void Awake()
        {
            base.Awake();

            // 기존 Base Class 에서 이미 가져오지만 외부에 노출되지 않으므로 다시 가져옴
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            // 현재 상호작용하는 interactorObject를 가져옴
            var interactor = args.interactorObject;

            // 현재 interactor가 DirectInteractor인지 확인
            if (interactor is XRDirectInteractor directInteractor)
            {
                // 원래 위치와 회전을 갖고있는 savedTransform instance 생성
                SavedTransform savedTransform = new SavedTransform
                {
                    OriginalPosition = directInteractor.attachTransform.localPosition,
                    OriginalRotation = directInteractor.attachTransform.localRotation
                };

                // savedTrasnforms[directInteractor] 에 해당 interactor와 SavedTrasnform을 매핑하여 저장
                savedTransforms[directInteractor] = savedTransform;

                // attachTransform이 할당되어 있는지 여부를 저장
                bool haveAttach = attachTransform != null;

                // 물체의 위치와 회전을 컨트롤러의 위치와 회전으로 설정
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
                // Dictionary에 저장된 SavedTransform 값을 가져옴
                if (savedTransforms.TryGetValue(directInteractor, out SavedTransform savedTransform))
                {
                    // 원래 위치와 회전으로 복원
                    directInteractor.attachTransform.localPosition = savedTransform.OriginalPosition;
                    directInteractor.attachTransform.localRotation = savedTransform.OriginalRotation;

                    // Dictionary에서 해당 interactor의 매핑을 제거
                    savedTransforms.Remove(directInteractor);
                }
            }

            base.OnSelectExited(args);
        }

        // 해당 물체를 선택 가능한지 여부를 결정하는 오버라이드 함수 -> 수정 필요
        [System.Obsolete]
        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            int interactorLayerMask = 1 << interactor.gameObject.layer;
            return base.IsSelectableBy(interactor) && (interactionLayerMask.value & interactorLayerMask) != 0;
        }
    }
}