using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ldw
{
    public class KeyPadButton : PushButtonInteractor
    {
        public int keyPadNum;

        [SerializeField]
        UnityEvent KeyPadClicked;

        public void OnKeyPadButtonClicked()
        {
            Debug.Log($"insert {keyPadNum}");

            KeyPadClicked?.Invoke();
        }
    }
}