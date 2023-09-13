using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ldw
{
    public class KeyPadButton : PushButtonInteractor
    {
        // Ű �е� ��ȣ
        public int keyPadNum;

        // Ű �е� Ŭ�� �� �߻��Ǵ� �̺�Ʈ
        [SerializeField]
        UnityEvent KeyPadClicked;

        public void OnKeyPadButtonClicked()
        {
            Debug.Log($"insert {keyPadNum}");

            KeyPadClicked?.Invoke();
        }
    }
}