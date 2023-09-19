using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ldw
{
    public class KeyPadButton : PushButtonInteractor
    {
        // 키 패드 번호
        public int keyPadNum;

        // 키 패드 클릭 시 발생되는 이벤트
        [SerializeField]
        UnityEvent KeyPadClicked;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("KeyPad"))
                OnKeyPadButtonClicked();
        }

        public void OnKeyPadButtonClicked()
        {
            Debug.Log($"insert {keyPadNum}");

            KeyPadClicked?.Invoke();
        }
    }
}