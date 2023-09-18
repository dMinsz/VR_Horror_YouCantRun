using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{   
    //�÷��̾��� transform�� ����ȭ�� �Ҵ�
    public Transform playerTransform;

    // ǥ�õ� �Ÿ�? ����ȭ�� ���氡��
    public float distance;

    public float smoothSpeed;

    private Vector3 Velocity = Vector3.zero;

    private void Update()
    {
        if(playerTransform != null)
        {
            Vector3 targetTransform = playerTransform.position + playerTransform.forward * distance;

            //transform.position = targetTransform;
            transform.position = Vector3.SmoothDamp(transform.position, targetTransform, ref Velocity, smoothSpeed * Time.deltaTime); 
            //transform.position = new Vector3(targetTransform.x, 1.8f, targetTransform.y);

            Quaternion targetRotation = playerTransform.rotation;
            transform.rotation = targetRotation;    
        }
    }
}