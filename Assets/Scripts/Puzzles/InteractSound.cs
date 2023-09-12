using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ldw
{
    public class InteractSound : MonoBehaviour
    {
        public AudioSource audioSource;

        [SerializeField]
        private AudioClip[] sounds;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 랜덤 이벤트로 실행시킬 함수
        public void PlayRandomSound()
        {
            int rand = Random.Range(0, sounds.Length);
            PlaySE(sounds[rand]);
        }

        // 재생하는 함수
        public void PlaySE(AudioClip _clip)
        {
            audioSource.clip = _clip;
            audioSource.Play();
        }
    }
}