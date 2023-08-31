using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// KeyCard를 받는 함수
public class KeyCardReceiver : MonoBehaviour
{
    public UnityEvent OnKeyCardCollision;  // KeyCard 의 Collider가 닿을 경우 실행되는 이벤트
    public bool DestroyedOnTriggered;      // 실행 후 오브젝트를 파괴할지 선택

    public void OnTriggerEnter(Collider other)  // Trigger Collider를 추가해 충돌 판단
    {
        var proj = other.GetComponent<KeyCard>();   // KeyCard 컴포넌트가 있는지 확인

        // KeyCard 컴포넌트가 있다면(null이 아니면) 이벤트 실행.
        if (proj != null)
        {
            OnKeyCardCollision.Invoke();

            // DestroyedOnTriggered 활성화 되있으면 오브젝트 파괴(1회용 KeyCard)
            if (DestroyedOnTriggered)
                Destroy(this);
        }
    }
}
