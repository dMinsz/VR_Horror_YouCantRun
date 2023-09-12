using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrainWater : MonoBehaviour
{
    private Vector3 initialScale; // �ʱ� ũ��
    private Vector3 targetScale = new Vector3(0.008f, 0.0015f, 0.035f); // ��ǥ ũ��

    public float duration = 10f; // ��ȯ �Ⱓ (��)
    private float startTime; // ���� �ð�

    private Vector3 initialPosition;

    private void Start()
    {
        initialScale = transform.localScale;  // �ʱ� ũ�� ����
        initialPosition = transform.position; // �ʱ� ��ġ ����
        // �ڷ�ƾ ����   
        
    }

    public void StartDrain()
    {
        StartCoroutine(ChangeSizeOverTime());
    }

    private IEnumerator ChangeSizeOverTime()
    {
        yield return new WaitForSeconds(0.1f);
        float elapsedTime = 0f;
        startTime = Time.time; // ���� �ð� ����
        while (elapsedTime < duration)
        {
            // ��� �ð� ���
            elapsedTime = Time.time - startTime;

            // Lerp�� ����Ͽ� ũ�� ����
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, -0.308f, 0), t);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        // ��ǥ ũ��� ���� ����
        //transform.localScale = targetScale;
    }
}
