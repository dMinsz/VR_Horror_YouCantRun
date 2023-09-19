using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;
using TMPro;
using System.Linq;

namespace ldw
{
    public class KeyPad : MonoBehaviour
    {
        [SerializeField]
        TMP_Text userInputText; // 현재 유저가 입력한 번호 Text

        [SerializeField]
        UnityEvent CorrectPassword; // 비밀번호를 맞췄을 때 이벤트

        [SerializeField]
        UnityEvent DisCorrectPassword;  // 비밀번호가 틀렸을 때 이벤트

        [SerializeField]
        [Tooltip("비밀번호")]
        public string password;

        public string userInput = "";

        public bool isClicked;

        private void Start()
        {
            PasswordReset();
            isClicked = false;
        }

        private void PasswordReset()
        {
            userInput = "";
        }

        public void OnClickKeyButton(int keyPadNum)
        {
            if (!isClicked)
            {
                if (keyPadNum == 10)
                    EraseButton();
                else if (keyPadNum == 11)
                    InputButton();
                else
                {
                    if (userInput.Length >= 4)
                    {

                    }
                    else
                    {
                        userInput += keyPadNum;
                        Debug.Log($"{keyPadNum} insert, userInput : {userInput}");
                        userInputText.text = $"{userInput}";
                    }
                }
                StartCoroutine(ButtonCoolTime());
            }
        }

        IEnumerator ButtonCoolTime()
        {
            isClicked = true;
            yield return new WaitForSeconds(1f);
            isClicked = false;
            yield return null;
        }

        public void EraseButton()
        {
            if (userInput.Length > 0)
            {
                userInput = userInput.Substring(0, userInput.Length - 1);
                Debug.Log($"{userInput}");
                userInputText.text = userInput;
            }
            else
            {
                Debug.Log("userInput is null");
            }
        }

        public void InputButton()
        {
            if (userInput == password)
            {
                Debug.Log($"{userInput} is Right Password");
                userInputText.text = "";

                CorrectPassword.Invoke();
            }
            else
            {
                Debug.Log($"{userInput} is Wrong Password, Password Reset");
                PasswordReset();
                userInputText.text = "";

                DisCorrectPassword.Invoke();
            }

        }
    }

}
