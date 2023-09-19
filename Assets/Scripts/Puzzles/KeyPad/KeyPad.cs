using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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

        [SerializeField]
        AudioSource audioSource;
        public AudioClip touchClip;
        public AudioClip correctClip;
        public AudioClip disCorrectClip;

        private Coroutine coolTimeCoroutine;

        public void SetIsClicked()
        {
            isClicked = false;
        }

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
                    if (userInput.Length < 4)
                    {
                        userInput += keyPadNum;
                        userInputText.text = $"{userInput}";
                    }
                }
                audioSource.clip = touchClip;
                audioSource.Play();

                coolTimeCoroutine = StartCoroutine(ButtonCoolTime());
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
                userInputText.text = userInput;
            }
        }

        public void InputButton()
        {
            if (userInput == password)
            {
                userInputText.text = "";

                audioSource.clip = correctClip;
                audioSource.Play();
                CorrectPassword.Invoke();
            }
            else
            {
                PasswordReset();
                userInputText.text = "";

                audioSource.clip = disCorrectClip;
                audioSource.Play();
                DisCorrectPassword.Invoke();
            }
        }

        public void OnDisable()
        {
            if(coolTimeCoroutine != null)
                StopCoroutine(coolTimeCoroutine);
        }
    }
}
