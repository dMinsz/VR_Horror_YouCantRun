using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ldw
{
    /// <summary>
    /// SocketInteractor와 함께 작용하는 특별한 스크립트.
    /// Socket Type을 정의하고, 이 SocketType이 SocketInteractor의 SocketType과 일치하지 않는 경우
    /// 해당 소켓을 유효한 대상으로 인정하지 않음.
    /// </summary>
    [RequireComponent(typeof(XRBaseInteractable))]
    public class SocketTargetName : MonoBehaviour
    {
        public string SocketType;               // SocketType 지정
        public SelectEnterEvent SocketedEvent;  // 소켓에 물체가 끼워졌을 때 실행되는 이벤트
        public bool DisableSocketOnSocketed;    // 소켓에 물체가 끼워졌을 때 소켓을 비활성화 할 지 여부를 결정

        void Awake()
        {
            var interactable = GetComponent<XRBaseInteractable>();

            // 선택(Select) 이벤트가 발생했을 때 SelectedSwitch 메서드를 호출
            interactable.selectEntered.AddListener(SelectedSwitch);
        }

        // 소켓에 물체가 끼워졌을 때 동작을 처리하는 함수
        public void SelectedSwitch(SelectEnterEventArgs args)
        {
            var interactor = args.interactorObject;
            var socketInteractor = interactor as SocketInteractor;

            // 소켓 인터랙터가 SocketInteractor인지 확인
            if (socketInteractor == null)
                return;

            // SocketType이 소켓 인터랙터의 acceptedType과 일치하지 않으면 처리하지 않음
            if (SocketType != socketInteractor.acceptedType)
                return;

            // 혹시 DisableSocketOnSocketed 가 true라면 소켓에 물체가 끼워진 후 0.5초 뒤 소켓 비활성화
            if (DisableSocketOnSocketed)
            {
                // 소켓이 끼워진 후 일정 시간이 지난 뒤 소켓을 비활성화하는 코루틴을 실행
                // 소켓 비활성화에 대해 더 좋은 방법을 고려할 수 있음 / 사용하지 않을 수도 있음
                StartCoroutine(DisableSocketDelayed(socketInteractor));
            }

            // SocketedEvent 이벤트를 호출
            SocketedEvent.Invoke(args);
        }

        IEnumerator DisableSocketDelayed(SocketInteractor interactor)
        {
            // 0.5초의 지연 후에 소켓을 비활성화
            yield return new WaitForSeconds(0.5f);
            interactor.socketActive = false;
        }
    }
}