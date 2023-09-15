using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*//�Ķ���͸� ������ ���� �̺�Ʈ
[System.Serializable]
public class MyEvent : UnityEvent<MyParameters> { }

//�̺�Ʈ ������ ���� �Ķ����
[System.Serializable]
public class MyParameters
{
    public int parameter1;
    public string parameter2;
}*/

public class Slot1 : XRSocketInteractor
{
    private Vector3 slotSize = new Vector3(0.2f, 0.1f, 0.2f);
    private Vector3 itemSize;
    private Vector3 itemCenter;

    private Transform attachRepos;
    private float ratio;

    public int slotNum;
    public string itemName;


    public UnityEvent<int, string> AddItemEvent;
    public UnityEvent<int> RemoveItemEvent;


    protected override void Start()
    {
        base.Start();
    }

    [System.Obsolete]
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;

        //item Center 메쉬랜더의 중간
        itemCenter = obj.GetComponentInChildren<MeshRenderer>().bounds.center;

        //item크기
        itemSize = obj.GetComponentInChildren<MeshRenderer>().bounds.size;

        //Attach 포인트가 있을경우 ? 깊은복사
        /*if (args.interactableObject.transform.Find("Attach"))
        {
            Transform AttachPoint = args.interactableObject.transform.Find("Attach");
            attachRepos.position = new Vector3(AttachPoint.position.x, AttachPoint.position.y, AttachPoint.position.z);
            attachRepos.rotation = new Quaternion(AttachPoint.rotation.x, AttachPoint.rotation.y, AttachPoint.rotation.z, AttachPoint.rotation.w);
        }*/
        //grabinteractable 의 attachTransform바꾸기
        //args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.position = itemCenter;

        //크기바꾸기
        if (itemSize.x > 0.2f || itemSize.z > 0.2f)
        {
            if (itemSize.x >= itemSize.z)
                ratio = slotSize.x / itemSize.x;
            else
                ratio = (slotSize.z / itemSize.z);

            args.interactableObject.transform.localScale *= ratio;
        }
        else
            ratio = 1.0f;

        //차트면 돌리기
        if (args.interactableObject.transform.gameObject.name == "Hint_Chart_1")
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        //args.interactableObject.transform.GetChild(3).localRotation = Quaternion.Euler(0f, 180f, 0f);

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
        if (args.interactableObject.transform.gameObject.name == "Hint_Chart_1")
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform.localRotation = Quaternion.Euler(90f, -90f, 90f);
        base.OnSelectExited(args);
        args.interactableObject.transform.localScale *= (1.0f / ratio);
    }

    public void socketCheck()
    {
        IXRSelectInteractable socketObj = this.GetOldestInteractableSelected();


        itemName = socketObj.transform.name;
        //socketObj.transform.gameObject.GetComponent<XRGrabInteractable>().attachTransform.position = socketObj.transform.position;

        AddItemEvent?.Invoke(slotNum, itemName);

        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    // ���� ���빰 ���
    public void socketDrop()
    {
        IXRSelectInteractable obj = this.GetOldestInteractableSelected();


        itemName = "";
        RemoveItemEvent?.Invoke(slotNum);

    }
}