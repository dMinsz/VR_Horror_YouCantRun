using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// KeyCard�� �޴� �Լ�
public class KeyCardReceiver : MonoBehaviour
{
    public UnityEvent OnKeyCardCollision;  // KeyCard �� Collider�� ���� ��� ����Ǵ� �̺�Ʈ
    public bool DestroyedOnTriggered;      // ���� �� ������Ʈ�� �ı����� ����

    // public AudioSource audioSource;

    private void Awake()
    {
        // audioSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)  // Trigger Collider�� �߰��� �浹 �Ǵ�
    {
        var proj = other.GetComponent<KeyCard>();   // KeyCard ������Ʈ�� �ִ��� Ȯ��

        // KeyCard ������Ʈ�� �ִٸ�(null�� �ƴϸ�) �̺�Ʈ ����.
        if (proj != null)
        {
            // proj -> KeyCard ������Ʈ�� ���� ��ü�� �ı�
            Destroy(proj.gameObject);
            OnKeyCardCollision.Invoke();
            // audioSource.Play();

            // DestroyedOnTriggered Ȱ��ȭ �������� ������Ʈ �ı� => ī�� Ű�� �����ϴ� ��ũ��Ʈ ����
            if (DestroyedOnTriggered)
                Destroy(this);
        }
    }
}
