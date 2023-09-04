using System.Collections;
using System.Collections.Generic;
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

        public void OnKeyPadButtonClicked()
        {
            Debug.Log($"insert {keyPadNum}");

            KeyPadClicked?.Invoke();
        }
    }
}