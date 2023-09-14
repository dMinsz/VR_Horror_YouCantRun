using ldw;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*//파라미터를 던지기 위한 이벤트
[System.Serializable]
public class MyEvent : UnityEvent<MyParameters> { }

//이벤트 던지기 위한 파라미터
[System.Serializable]
public class MyParameters
{
    public int parameter1;
    public string parameter2;
}*/

public class Slot1 : XRSocketInteractor
{
    public int slotNum;
    public string itemName;

    private Transform itemTransform;

    public UnityEvent<int, string> AddItemEvent;
    public UnityEvent<int> RemoveItemEvent;


    protected override void Start()
    {
        base.Start();
        GameObject emptyObject = new GameObject("EmptyObject");

        // GameObject의 Transform 가져오기
        Transform itemTransform = emptyObject.transform;

        // Transform을 초기화하려면 위치, 회전 및 스케일 값을 설정합니다.
        itemTransform.position = Vector3.zero;
        itemTransform.rotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }

    [System.Obsolete]
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        //크기줄이기
        if (args.interactableObject.transform.gameObject.name == "Hint_Chart_1")
        {
            itemTransform.position = args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.position;
            itemTransform.localRotation = args.interactableObject.transform.localRotation;
            itemTransform.localScale = args.interactableObject.transform.localScale;

            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.position = args.interactableObject.transform.position;
            args.interactableObject.transform.GetChild(3).localRotation = Quaternion.Euler(0f, 180f, 0f);
            args.interactableObject.transform.localScale = Vector3.one * 0.1f;
        }
        //args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.position = args.interactableObject.transform.position;
        //args.interactableObject.transform.GetChild(3).localRotation = Quaternion.Euler(0f, 180f, 0f);
        //args.interactableObject.transform.localScale = Vector3.one * 0.1f;

        base.OnSelectEntering(args);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (args.interactableObject.transform.name == "Hint_Chart_1")
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.position = itemTransform.position;
            args.interactableObject.transform.GetChild(3).localRotation = itemTransform.localRotation;
            args.interactableObject.transform.localScale = itemTransform.localScale;
        }
    }




    //소켓 select아이템 확인후 추가
    public void socketCheck()
    {
        //select된 object 확인
        IXRSelectInteractable socketObj = this.GetOldestInteractableSelected();


        itemName = socketObj.transform.name;
        //socketObj.transform.gameObject.GetComponent<XRGrabInteractable>().attachTransform.position = socketObj.transform.position;

        //아이템 추가 이벤트
        AddItemEvent?.Invoke(slotNum, itemName);

        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    // 소켓 내용물 드랍
    public void socketDrop()
    {
        IXRSelectInteractable obj = this.GetOldestInteractableSelected();


        itemName = "";
        //삭제 이벤트발생
        RemoveItemEvent?.Invoke(slotNum);

    }
}