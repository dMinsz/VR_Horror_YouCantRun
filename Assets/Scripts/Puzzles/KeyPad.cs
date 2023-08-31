using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ldw
{
    public class KeyPad : MonoBehaviour
    {
        [SerializeField]
        KeyPadButton[] keyButtons;

        [SerializeField]
        UnityEvent CorrectPassword;

        public string password = "1324";
        public string userInput = "";

        private void Start()
        {
            PasswordReset();
        }

        private void PasswordReset()
        {
            userInput = "";
        }

        public void OnClickKeyButton(int keyPadNum)
        {
            userInput += keyPadNum;
            if(userInput.Length >= 4)
            {
                if(userInput == password)
                {
                    Debug.Log($"{userInput} is Right Password");
                    CorrectPassword.Invoke();
                }
                else
                {
                    Debug.Log($"{userInput} is Wrong Password, Password Reset");
                    PasswordReset();
                }
            }
        }
    }

}
