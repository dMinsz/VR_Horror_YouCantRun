using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// KeyCard를 받는 함수
public class KeyCardReceiver : MonoBehaviour
{
    public UnityEvent OnKeyCardCollision;  // KeyCard 의 Collider가 닿을 경우 실행되는 이벤트
    public bool DestroyedOnTriggered;      // 실행 후 오브젝트를 파괴할지 선택

    // public AudioSource audioSource;

    private void Awake()
    {
        // audioSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)  // Trigger Collider를 추가해 충돌 판단
    {
        var proj = other.GetComponent<KeyCard>();   // KeyCard 컴포넌트가 있는지 확인

        // KeyCard 컴포넌트가 있다면(null이 아니면) 이벤트 실행.
        if (proj != null)
        {
            // proj -> KeyCard 컴포넌트를 가진 객체를 파괴
            Destroy(proj.gameObject);
            OnKeyCardCollision.Invoke();
            // audioSource.Play();

            // DestroyedOnTriggered 활성화 되있으면 컴포넌트 파괴 => 카드 키를 판정하는 스크립트 제거
            if (DestroyedOnTriggered)
                Destroy(this);
        }
    }
}
