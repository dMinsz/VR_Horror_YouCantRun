using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainWater : MonoBehaviour
{
    public Vector3 targetScale; // 목표 크기
    public float duration = 10f; // 변환 기간 (초)

    private Vector3 initialScale; // 초기 크기
    private float startTime; // 시작 시간

    private Vector3 initialPosition;

    private void Start()
    {
        initialScale = transform.localScale; // 초기 크기 저장
        initialPosition = transform.position;
        targetScale = new Vector3(0.014f, 0.001f, 0.038f);
        // 코루틴 시작
        StartCoroutine(ChangeSizeOverTime());
    }

    private IEnumerator ChangeSizeOverTime()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;
        startTime = Time.time; // 시작 시간 저장
        while (elapsedTime < duration)
        {
            // 경과 시간 계산
            elapsedTime = Time.time - startTime;

            // Lerp를 사용하여 크기 변경
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, 0.484f, 0), t);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        // 목표 크기로 최종 설정
        transform.localScale = targetScale;
    }
}
