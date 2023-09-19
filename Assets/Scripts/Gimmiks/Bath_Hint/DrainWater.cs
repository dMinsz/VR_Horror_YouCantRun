using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//마개를 뺏을시 물이 빠지는 코드 -> 물의 scale 과 높이감소
public class DrainWater : MonoBehaviour
{
    private Vector3 initialScale; // 초기 크기
    private Vector3 targetScale = new Vector3(0.008f, 0.0015f, 0.035f); // 목표 크기

    public float duration = 10f; // 변환 기간 (초)
    private float startTime; // 시작 시간

    private Vector3 initialPosition;

    private void Start()
    {
        initialScale = transform.localScale;  // 초기 크기 저장
        initialPosition = transform.position; // 초기 위치 저장
        // 코루틴 시작   
        
    }


    //코루틴으로 수행
    public void StartDrain()
    {
        StartCoroutine(ChangeSizeOverTime());
    }

    private IEnumerator ChangeSizeOverTime()
    {
        yield return new WaitForSeconds(0.1f);
        float elapsedTime = 0f;
        startTime = Time.time; // 시작 시간 저장
        while (elapsedTime < duration)
        {
            // 경과 시간 계산
            elapsedTime = Time.time - startTime;

            // Lerp를 사용하여 크기 변경
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, -0.308f, 0), t);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        // 목표 크기로 최종 설정
        //transform.localScale = targetScale;
    }
}
