using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainWater : MonoBehaviour
{
    public Vector3 targetScale; // ��ǥ ũ��
    public float duration = 10f; // ��ȯ �Ⱓ (��)

    private Vector3 initialScale; // �ʱ� ũ��
    private float startTime; // ���� �ð�

    private Vector3 initialPosition;

    private void Start()
    {
        initialScale = transform.localScale; // �ʱ� ũ�� ����
        initialPosition = transform.position;
        targetScale = new Vector3(0.014f, 0.001f, 0.038f);
        // �ڷ�ƾ ����
        StartCoroutine(ChangeSizeOverTime());
    }

    private IEnumerator ChangeSizeOverTime()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;
        startTime = Time.time; // ���� �ð� ����
        while (elapsedTime < duration)
        {
            // ��� �ð� ���
            elapsedTime = Time.time - startTime;

            // Lerp�� ����Ͽ� ũ�� ����
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, 0.484f, 0), t);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        // ��ǥ ũ��� ���� ����
        transform.localScale = targetScale;
    }
}
